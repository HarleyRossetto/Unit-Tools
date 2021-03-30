//#define WRITE_ALL_JSON_TO_DISK

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

using Macquarie.Handbook.Data;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.WebApi;
using Unit_Info.Helpers;
using static Macquarie.JSON.JsonSerialisationHelper;

//Bad separation...
using static Unit_Info.Helpers.LocalDataDirectoryHelper;
using static Unit_Info.Helpers.LocalDirectories;
using System.Collections.Generic;

namespace Macquarie.Handbook
{
    public static class MacquarieHandbook
    {
        static readonly HttpClient httpClient = new HttpClient();

        public static TimeSpan WebRequestTimeout { get => httpClient.Timeout; set => httpClient.Timeout = value; }

        public static async Task<string> DownloadString(HandbookApiRequestBuilder apiRequest) {
            return await DownloadString(apiRequest.ToString());
        }

        public static async Task<string> DownloadString(string url) {
            Console.WriteLine(url);
            var response = await httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
        
        public static async Task<MacquarieDataCollection<T>> GetCMSDataCollection<T>(HandbookApiRequestBuilder apiRequest) where T : MacquarieMetadata {
            return await GetCMSDataCollection<T>(apiRequest.ToString());
        }

        public static async Task<MacquarieDataCollection<T>> GetCMSDataCollection<T>(string url) where T : MacquarieMetadata {
            return DeserialiseJsonObject<MacquarieDataCollection<T>>(await DownloadString(url));
        }

        //False for course. Make enum later
        public static async Task<MacquarieDataCollection<T>> GetLocalDataCollection<T>(bool unit = true, bool doConcurrent = false) where T : MacquarieMetadata {
            var dirPath = LocalDataDirectoryHelper.GetDirectory(Unit_Filtered);
            if (Directory.Exists(dirPath)) {
                try {
                    var filesToLoad = new List<string>(Directory.GetFiles(dirPath, "*.json", SearchOption.AllDirectories));

                    ConcurrentBag<T> tempCollection = new ConcurrentBag<T>();

                    if (doConcurrent) {

                        var tasks = filesToLoad.Select(async file =>
                        {
                            tempCollection.Add(DeserialiseJsonObject<T>(await File.ReadAllTextAsync(file)));
                        });

                        await Task.WhenAll(tasks);

                        return new MacquarieDataCollection<T>(tempCollection);
                    } else {

                        // var returnable = new MacquarieDataCollection<T>(filesToLoad.Count);

                        Parallel.ForEach(filesToLoad,
                                            new ParallelOptions() { MaxDegreeOfParallelism = 50 },
                                            async (file) =>
                                                {
                                                    tempCollection.Add(DeserialiseJsonObject<T>(await File.ReadAllTextAsync(file)));
                                                });

                        return new MacquarieDataCollection<T>(tempCollection);
                    }

                } catch (Exception ex) {
                    System.Console.WriteLine(ex.ToString());
                    return null;
                }
            } else {
                return null;
            }
        }

        public static async Task<MacquarieUnit> GetUnit(string unitCode, int? implementationYear = null) {
            var localPathIndividual = CreateFilePath(Unit_Individual, $"{unitCode}.json");
            string localPathFiltered;
            var localDirMapFound = LocalDataMap.GetDirectoryOut(unitCode, out localPathFiltered);
            localPathFiltered += $"{unitCode}.json";

            bool localDirMapValidPath = File.Exists(localPathFiltered);
            bool individualValidPath = File.Exists(localPathIndividual);

            string pathToAccess = "";
            if (localDirMapValidPath)
                pathToAccess = localPathFiltered;
            else if (individualValidPath) {
                pathToAccess = localPathIndividual;
            }

            //if we have a local copy source that.
            if (localDirMapValidPath || individualValidPath) {

                try {
                    System.Console.WriteLine($"Reading local copy of {unitCode} from disk..");
                    System.Console.WriteLine(pathToAccess);

                    var jsonString = await File.ReadAllTextAsync(pathToAccess);

                    //var units = DeserialiseJsonObject<MacquarieDataCollection<MacquarieUnit>>(jsonString);
                    var unit = DeserialiseJsonObject<MacquarieUnit>(jsonString);

                    if (unit != null) {
                        //var course = units.AsEnumerable().First();

                        if (unit.ImplementationYear == implementationYear?.ToString()) {
                            return unit;
                        } else {
                            System.Console.WriteLine($"Local copy of {unitCode}'s implementation year did not match {implementationYear}, was {unit.ImplementationYear}");
                        }
                    }
                } catch (Exception ex) {
                    System.Console.WriteLine(ex.ToString());
                }
            }

            System.Console.WriteLine($"Sourcing {unitCode} from CMS..");
            HandbookApiRequestBuilder apiRequest = new UnitApiRequestBuilder(unitCode, implementationYear);
            var unitResultsCollection = await GetCMSDataCollection<MacquarieUnit>(apiRequest);

            //The JSON returned from the request is always a list, even if just one value.
            //CMS requests through this function should only every return 1 value.
            if (unitResultsCollection.Count == 1) {
                return unitResultsCollection[0];
            } else {
                return null;
            }
        }

        public static async Task<MacquarieDataCollection<MacquarieUnit>> GetAllUnits(int? implementationYear = null, int limit = 3000, bool fromDisk = false) {
            if (implementationYear == null)
                implementationYear = DateTime.Now.Year;

            if (fromDisk) {
                return await GetLocalDataCollection<MacquarieUnit>(fromDisk);
            } else {

                var apiRequest = new UnitApiRequestBuilder() { ImplementationYear = implementationYear, Limit = limit };

                return await GetCMSDataCollection<MacquarieUnit>(apiRequest);
            }
        }

        public static async Task<MacquarieCourse> GetCourse(string courseCode, int? implementationYear = null) {
            string localPath = CreateFilePath(Course_Individual, $"{courseCode}.json");
            //if we have a local copy source that.
            if (File.Exists(localPath)) {
                try {
                    System.Console.WriteLine($"Reading local copy of {courseCode} from disk..");
                    System.Console.WriteLine(localPath);

                    var jsonString = await File.ReadAllTextAsync(localPath);

                    //var units = DeserialiseJsonObject<MacquarieDataCollection<MacquarieUnit>>(jsonString);
                    var course = DeserialiseJsonObject<MacquarieCourse>(jsonString);

                    if (course != null) {
                        //var course = units.AsEnumerable().First();

                        if (course.ImplementationYear == implementationYear?.ToString()) {
                            return course;
                        } else {
                            System.Console.WriteLine($"Local copy of {courseCode}'s implementation year did not match {implementationYear}, was {course.ImplementationYear}");
                        }
                    }
                } catch (Exception ex) {
                    System.Console.WriteLine(ex.ToString());
                }
            }

            System.Console.WriteLine($"Sourcing {courseCode} from CMS..");


            HandbookApiRequestBuilder apiRequest = new CourseApiRequestBuilder(courseCode, implementationYear);
            var courseResultsCollection = await GetCMSDataCollection<MacquarieCourse>(apiRequest);

            //The JSON returned from the request is always a list, even if just one value.
            //CMS requests through this function should only every return 1 value.
            if (courseResultsCollection.Count == 1) {
                return courseResultsCollection[0];
            } else {
                return null;
            }
        }

        public static async Task<MacquarieDataCollection<MacquarieCourse>> GetAllCourses(int? implementationYear = null, int limit = 3000) {
            if (implementationYear == null)
                implementationYear = DateTime.Now.Year;

            var apiRequest = new CourseApiRequestBuilder() { ImplementationYear = implementationYear, Limit = limit };

            return await GetCMSDataCollection<MacquarieCourse>(apiRequest);
        }
    }
}