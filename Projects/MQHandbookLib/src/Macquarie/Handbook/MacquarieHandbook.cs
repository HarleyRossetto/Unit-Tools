//#define WRITE_ALL_JSON_TO_DISK

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

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
        static readonly HttpClient httpClient = new();

        public static TimeSpan WebRequestTimeout { get => httpClient.Timeout; set => httpClient.Timeout = value; }

        static MacquarieHandbook() {
            //httpClient.BaseAddress = ""
        }

        public static async Task<string> DownloadString(HandbookApiRequestBuilder apiRequest) {
            return await DownloadString(apiRequest.ToString());
        }

        public static async Task<string> DownloadString(string url) {
            Console.WriteLine(url);
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) {
                System.Console.WriteLine(response.ToString());
                //Return an emptry string to parse.
                return String.Empty;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<MacquarieDataCollection<T>> GetCMSDataCollection<T>(HandbookApiRequestBuilder apiRequest) where T : MacquarieMetadata {
            return await GetCMSDataCollection<T>(apiRequest.ToString());
        }

        public static async Task<MacquarieDataCollection<T>> GetCMSDataCollection<T>(string url) where T : MacquarieMetadata {
            return DeserialiseJsonObject<MacquarieDataCollection<T>>(await DownloadString(url));
        }

        //False for course. Make enum later
        public static async Task<MacquarieDataCollection<T>> LoadAllLocalData<T>() where T : MacquarieMetadata {
            var dirPath = GetDirectory(Unit_Filtered);
            if (Directory.Exists(dirPath)) {
                try {
                    var filesToLoad = new List<string>(Directory.GetFiles(dirPath, "*.json", SearchOption.AllDirectories));

                    var results = new MacquarieDataCollection<T>();

                    foreach (var file in filesToLoad) {
                        var jsonString = await File.ReadAllTextAsync(file);
                        var data = DeserialiseJsonObject<T>(jsonString);
                        results.Add(data);
                    }   
                    return results;

                } catch (Exception ex) {
                    System.Console.WriteLine(ex.ToString());
                    return null;
                }
            } else {
                return null;
            }
        }

        public static async Task<MacquarieUnit> GetUnit(string unitCode, int? implementationYear = null) {
            MacquarieUnit result = await TryLoadLocalData<MacquarieUnit>(unitCode, APIResourceType.Unit);

            if (result is null) {
                System.Console.WriteLine($"Unable to load {unitCode} from local cache, loading from CMS..");
                HandbookApiRequestBuilder apiRequestBuilder = new(unitCode, implementationYear, APIResourceType.Unit);
                var resultsCollection = await GetCMSDataCollection<MacquarieUnit>(apiRequestBuilder);
                if (resultsCollection.Count >= 1)
                    result = resultsCollection[0];
            }

            //Save to local cache  while we at it
            await SerialiseObjectToJsonFile(result, CreateFilePath(Unit_Individual, unitCode));

            return result;
        }

        public static async Task<T> TryLoadLocalData<T>(string code, APIResourceType resourceType) where T : MacquarieMetadata {
            LocalDirectories path = resourceType switch
            {
                APIResourceType.Course  => LocalDirectories.Course_Individual,
                APIResourceType.Unit    => LocalDirectories.Unit_Individual,
                _                       => LocalDirectories.NoDirectory
            };

            var localPath = CreateFilePath(path, $"{code}.json");

            if (File.Exists(localPath)) {
                System.Console.WriteLine($"Attempting to load {code} from local cache..");
                var result = await LoadObjectFromFile<T>(localPath);

                if (result is not null)
                    System.Console.WriteLine($"Successfully loaded {code} ");

                return result;
            }

            return default;
        }

        private static async Task<T> LoadObjectFromFile<T>(string file) where T : MacquarieMetadata {
            try {
                var jsonString = await File.ReadAllTextAsync(file);
                var obj = DeserialiseJsonObject<T>(jsonString);

                return obj;
            } catch (Exception ex) {
                System.Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static async Task<MacquarieDataCollection<MacquarieUnit>> GetAllUnits(int? implementationYear = null, int limit = 3000, bool readFromDisk = false) {
            implementationYear ??= DateTime.Now.Year;

            if (readFromDisk) {
                return await LoadAllLocalData<MacquarieUnit>();
            } else {
                var apiRequest = new HandbookApiRequestBuilder() { ImplementationYear = implementationYear, Limit = limit, ResourceType = APIResourceType.Unit };
                return await GetCMSDataCollection<MacquarieUnit>(apiRequest);
            }
        }

        public static async Task<MacquarieCourse> GetCourse(string courseCode, int? implementationYear = null) {
            MacquarieCourse result = await TryLoadLocalData<MacquarieCourse>(courseCode, APIResourceType.Course);

            if (result is null) {
                System.Console.WriteLine($"Unable to load {courseCode} from local cache, loading from CMS..");
                HandbookApiRequestBuilder apiRequestBuilder = new(courseCode, implementationYear, APIResourceType.Course);
                var resultsCollection = await GetCMSDataCollection<MacquarieCourse>(apiRequestBuilder);
                if (resultsCollection.Count >= 1)
                    result = resultsCollection[0];
            }

            return result;
        }

        public static async Task<MacquarieDataCollection<MacquarieCourse>> GetAllCourses(int? implementationYear = null, int limit = 3000) {
            implementationYear ??= DateTime.Now.Year;

            var apiRequest = new CourseApiRequestBuilder() { ImplementationYear = implementationYear, Limit = limit };

            return await GetCMSDataCollection<MacquarieCourse>(apiRequest);
        }
    }
}