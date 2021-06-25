using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Macquarie.Handbook;
using Macquarie.Handbook.Data;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.WebApi;
using static Macquarie.JSON.JsonSerialisationHelper;

using Unit_Info.Helpers;
using static Unit_Info.Helpers.LocalDataDirectoryHelper;
using static Unit_Info.Helpers.LocalDirectories;


namespace Unit_Info.Demonstrations
{
    public class Demo
    {
        /// <summary>
        /// Demonstrates Unit request API creation, data collection and access.
        /// </summary>
        public static async Task GetUnit(String unitCode, bool writeToFile = true) {
            var unit = await MacquarieHandbook.GetUnit(unitCode, DateTime.Now.Year);

            if (unit is null) {
                Console.WriteLine("Unit with code '{0}' was not found.", unitCode);
                return;
            }

            Console.WriteLine(unit.UnitData.Title);

            if (writeToFile)
                await WriteObjectToFile(unit, LocalDirectories.Unit_Individual);

        }

        /// <summary>
        /// Demonstrates Unit request API creation, data collection and access.
        /// Requests all 2021 Units, with a limit of 3000 results.
        /// </summary>
        public static async Task GetAllUnitsAndWriteToFile() {
            var unitCollection = await MacquarieHandbook.GetAllUnits(2021, 10);

            await WriteCollectionToIndividualFiles(unitCollection);

            Console.WriteLine($"{unitCollection.Count} unit{(unitCollection.Count > 0 ? "s" : "")} retrieved and written to disk.");
        }

            private static async Task WriteObjectToFile(MacquarieMetadata data, LocalDirectories directoryToSaveTo, bool saveWithTimeStamp = false, Newtonsoft.Json.Formatting formatting = Newtonsoft.Json.Formatting.Indented) {
            if (data is not null) {
                await SerialiseObjectToJsonFile(data, CreateFilePath(directoryToSaveTo, data.Code), saveWithTimeStamp, formatting);
            }
        }

        private static async Task WriteCollectionToIndividualFiles<T>(MacquarieDataCollection<T> collection) where T : MacquarieMetadata {
            foreach (var unit in collection) {
                await WriteObjectToFile(unit, LocalDirectories.Unit_Individual, false, Newtonsoft.Json.Formatting.None);
            }
        }

        
        /// <summary>
        /// Demonstrates Course data collection and access.
        /// </summary>
        /// <param name="courseCode">
        /// The course code to attempt to retreive.
        /// </param> 
        public static async Task<MacquarieCourse> GetCourse(String courseCode) {
            var course = await MacquarieHandbook.GetCourse(courseCode, 2021);

            if (course is null) {
                Console.WriteLine($"No course with code '{courseCode}' was found.");
                return null;
            }

            Console.WriteLine(course.CourseData.CourseSearchTitle);
            await SerialiseObjectToJsonFile(course, CreateFilePath(Course_Individual, courseCode));
            return course;
        }

        public static async Task<IEnumerable<MacquarieCourse>> GetAllCourses() {
            var courses = await MacquarieHandbook.GetAllCourses(2021);
            return courses.AsEnumerable();
        }

        public static async Task GetAllCoursesAndSaveGroupedBySchool() {
            var courseCollection = await GetAllCourses();

            if (courseCollection.Any()) {
                await WriteCoursesGroupedBySchool(courseCollection);
            }
        }


        /// <summary>
        /// Demonstrates Course request API creation, data collection and access.
        /// Requests all 2021 courses..
        /// </summary>
        public static async Task GetAllCoursesAndWriteToFile() {
            var courseCollection = await GetAllCourses();

            if (courseCollection.Count() == 0) {
                Console.WriteLine($"No courses were found.");
                return;
            }

            await WriteCoursesToIndividualFiles(courseCollection);
        }

        public static async Task WriteUnitsToIndividualFilesFiltered(IEnumerable<MacquarieUnit> units) {
            var schoolGroups = units.AsEnumerable().GroupBy(u => u.UnitData.School.Value);

            foreach (var group in schoolGroups) {
                var depts = group.GroupBy(u => u.UnitData.AcademicOrganisation.Value);

                foreach (var department in depts) {
                    var levels = department.GroupBy(u => u.Level);

                    foreach (var level in levels) {
                        foreach (var unit in level) {
                            var parentDir = CreateFilePath(Unit_Filtered_BySchool, $"{group.Key}/{department.Key.TrimEnd()}/");
                            var childpath = $"{level.Key}/{unit.Code}";
                            string filePath = parentDir + childpath;

                            LocalDataMap.Register(unit.Code, parentDir);
                            await SerialiseObjectToJsonFile(unit, filePath);
                        }
                    }
                }
            }
        }

        public static async Task WriteCoursesGroupedBySchool(IEnumerable<MacquarieCourse> courses) {
            if (courses is not null && courses.Any()) {
                var grouped = courses.AsEnumerable().OrderBy(crs => crs.Code).GroupBy(crs => crs.CourseData.School.Value);

                foreach (var g in grouped) {
                    await SerialiseObjectToJsonFile(g, CreateFilePath(Course_Filtered_BySchool, g.Key));
                }
            }
        }
                    
        //Downloads all units and saves a copy of only the prerequisite enrolment rules.
        public static async Task GetAllUnitPrerequsiteForDevelopment() {
            Stopwatch sw = new();

            sw.Restart();
            var unitCollection = await MacquarieHandbook.GetAllUnits(2021);

            if (unitCollection.Count > 0) {
                var ruleAndCode = new List<Tuple<string, string>>();
                foreach (var i in unitCollection) {
                    foreach (var j in i.UnitData.EnrolmentRules) {
                        if (j.Type.Value == "prerequisite") {
                            ruleAndCode.Add(new Tuple<string, string>(i.Code, j.Description));
                        }
                    }
                }

                var orderedList = ruleAndCode.OrderBy(i => i.Item2.Length);

                await SerialiseObjectToJsonFile(orderedList, CreateFilePath(Unit_PreRequisite_Unparsed, "Macquarie_EnrolmentRules_ASC_LENGTH"));
            }

            sw.Stop();

            Console.WriteLine("{0} milliseconds for {1} unit query & deserialisation.", sw.ElapsedMilliseconds, unitCollection.Count);
        }

        public static async Task SaveListOfUnitCodesAndTitles() {
            var unitCodes = await GetListOfUnitCodes();

            await SerialiseObjectToJsonFile(unitCodes, CreateFilePath(Unit_Filtered, "Macquarie_Unit_Codes"));
        }

        public static async Task<IEnumerable<IGrouping<string, MacquarieBasicItemInfo>>> GetListOfUnitCodes() {
            var apiRequest = new UnitApiRequestBuilder() { ImplementationYear = 2021, Limit = 2500 };
            var unitCollection = await MacquarieHandbook.GetCMSDataCollection<MacquarieUnit>(apiRequest);

            var enumerable = (from unit in unitCollection.AsEnumerable()
                              select new { unit.Code, unit.Title, unit.UnitData.School.Value });
            var query = enumerable.Select(item => new MacquarieBasicItemInfo(item.Code, item.Title, item.Value)).OrderBy(item => item.Code).GroupBy(item => item.Department);

            return query;
        }

        public static async Task SaveListOfCourseCodesAndTitles() {
            var courseCodes = await GetListOfCourseCodes();

            await SerialiseObjectToJsonFile(courseCodes, CreateFilePath(Course_Filtered, "Macquarie_Course_Codes"));
        }

        public static async Task<IEnumerable<IGrouping<string, MacquarieBasicItemInfo>>> GetListOfCourseCodes() {
            var apiRequest = new CourseApiRequestBuilder() { ImplementationYear = 2021, Limit = 250 };
            var courseCollection = await MacquarieHandbook.GetCMSDataCollection<MacquarieCourse>(apiRequest);

            var enumerable = (from course in courseCollection.AsEnumerable()
                              select new { course.Code, course.Title, course.CourseData.School.Value });
            var query = enumerable.Select(item => new MacquarieBasicItemInfo(item.Code, item.Title, item.Value)).OrderBy(item => item.Code).GroupBy(item => item.Department);

            return query;
        }

        public static async Task WriteCoursesToIndividualFiles(IEnumerable<MacquarieCourse> courses) {
            foreach (var course in courses) {
                await SerialiseObjectToJsonFile(course, CreateFilePath(Course_Individual, course.Code));
            }
        }

        public static async Task WriteUnitsToIndividualFiles(IEnumerable<MacquarieUnit> units) {
            foreach (var unit in units) {
                await SerialiseObjectToJsonFile(unit, CreateFilePath(Unit_Individual, unit.Code));
            }
        }
    }

    public struct MacquarieBasicItemInfo
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }

        public MacquarieBasicItemInfo(string pCode, string pTitle, string pDepartment) {
            this.Code = pCode;
            this.Title = pTitle;
            this.Department = pDepartment;
        }
    }
}