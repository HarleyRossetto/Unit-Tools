//#define IGNORE_UNNECESSARY

using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using static Macquarie.JSON.JsonSerialisationHelper;

using Macquarie.Handbook;
using Macquarie.Handbook.Data;
using Macquarie.Handbook.WebApi;
namespace Unit_Info
{
    class Program
    {
        async static Task Main(string[] args) {
            Program program = new Program();

            //var courses = await MacquarieHandbook.GetAllCourses(2021);
            //var unit = await MacquarieHandbook.GetAllUnits(2021);

            //await program.WriteCoursesGroupedBySchool(courses.AsEnumerable());

            
            //await program.GetAllCoursesAndSaveGroupedBySchool();

            //await program.GetAllCoursesAndWriteToFile();

            //await program.GetAllCoursesAndSaveGroupedBySchool();

            //await program.GetCourse("C000006");

            //await program.GetUnit("COMP1000", true);
            await program.GetUnit("EDTE3010");
            /*
                EDTE3010 - has larger pre-requsite chain
            */
            //Downloads all units and saves a copy of only the prerequisite enrolment rules.
            //await program.GetAllUnitPrerequsiteForDevelopment();
        }

        /// <summary>
        /// Demonstrates Unit request API creation, data collection and access.
        /// </summary>
        public async Task GetUnit(String unitCode, bool writeToFile = true) {
            var unit = await MacquarieHandbook.GetUnit(unitCode, 2021);

            if (unit != null) {
                Console.WriteLine(unit.UnitData.Title);

                if (writeToFile)
                    await SerialiseObjectToJsonFile(unit, $"data/units/individual/{unit.Code}");

            } else {
                Console.WriteLine("Unit with code '{0}' was not found.", unitCode);
            }
        }

        /// <summary>
        /// Demonstrates Unit request API creation, data collection and access.
        /// Requests all 2021 Units, with a limit of 3000 results.
        /// </summary>
        public async Task GetAllUnitsAndWriteToFile() {
            var unitCollection = await MacquarieHandbook.GetAllUnits(2021);

            foreach (var unit in unitCollection) {
                await SerialiseObjectToJsonFile(unit, $"data/units/individual/{unit.Code}", false, false);
            }

            Console.WriteLine($"{unitCollection.Count} unit{(unitCollection.Count > 0 ? "s" : "")} retrieved and written to disk.");
        }

        /// <summary>
        /// Demonstrates Course data collection and access.
        /// </summary>
        /// <param name="courseCode">
        /// The course code to attempt to retreive.
        /// </param> 
        public async Task GetCourse(String courseCode) {
            var course = await MacquarieHandbook.GetCourse(courseCode, 2021);

            if (course != null) {
                Console.WriteLine(course.CourseData.CourseSearchTitle);
                await SerialiseObjectToJsonFile(course, $"data/courses/individual/{course.Code}");
            } else {
                Console.WriteLine($"No course with code '{courseCode}' was found.");
            }
        }

        public async Task<IEnumerable<MacquarieCourse>> GetAllCourses() {
            var courses = await MacquarieHandbook.GetAllCourses(2021);
            return courses.AsEnumerable();
        }

        public async Task GetAllCoursesAndSaveGroupedBySchool() {
            var courseCollection = await GetAllCourses();

            if (courseCollection.Count() > 0) {
                await WriteCoursesGroupedBySchool(courseCollection);
            }
        }

        /// <summary>
        /// Demonstrates Course request API creation, data collection and access.
        /// Requests all 2021 courses..
        /// </summary>
        public async Task GetAllCoursesAndWriteToFile() {
            var courseCollection = await GetAllCourses();

            if (courseCollection.Count() == 0) {
                Console.WriteLine($"No courses were found.");
                return;
            }

            await WriteCoursesToIndividualFiles(courseCollection);
        }

        public async Task WriteCoursesToIndividualFiles(IEnumerable<MacquarieCourse> courses) {
            foreach (var course in courses) {
                await SerialiseObjectToJsonFile(course, $"data/courses/individual/{course.Code}");
            }
        }

        public async Task WriteCoursesGroupedBySchool(IEnumerable<MacquarieCourse> courses) {
            if (courses != null && courses.Count() > 0) {
                var grouped = courses.AsEnumerable().OrderBy(crs => crs.Code).GroupBy(crs => crs.CourseData.School.Value);

                foreach (var g in grouped) {
                    await SerialiseObjectToJsonFile(g, $"data/courses/filtered/bySchool/{g.Key}");
                }
            }
        }
        public async Task GetAllUnitPrerequsiteForDevelopment() {
            Stopwatch sw = new Stopwatch();

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

                await SerialiseObjectToJsonFile(orderedList, "data/units/prerequisites/unparsed/Macquarie_EnrolmentRules_ASC_LENGTH");
            }


            sw.Stop();

            Console.WriteLine("{0} milliseconds for {1} unit query & deserialisation.", sw.ElapsedMilliseconds, unitCollection.Count);
        }

        public async Task SaveListOfUnitCodesAndTitles() {
            var unitCodes = await GetListOfUnitCodes();

            await SerialiseObjectToJsonFile(unitCodes, "data/units/filtered/Macquarie_Unit_Codes");
        }

        public async Task<IEnumerable<IGrouping<string, MacquarieBasicItemInfo>>> GetListOfUnitCodes() {
            var apiRequest = new UnitApiRequestBuilder() { ImplementationYear = 2021, Limit = 2500 };
            var unitCollection = await MacquarieHandbook.GetCMSDataCollection<MacquarieUnit>(apiRequest);
            // List<CourseBasicInfo> courseList =
            //             (from course in courseCollection.Collection
            //             select course.Code, course.).ToList();

            var enumerable = (from unit in unitCollection.AsEnumerable()
                              select new { unit.Code, unit.Title, unit.UnitData.School.Value });
            var query = enumerable.Select(item => new MacquarieBasicItemInfo(item.Code, item.Title, item.Value)).OrderBy(item => item.Code).GroupBy(item => item.Department);

            return query;
        }

        public async Task SaveListOfCourseCodesAndTitles() {
            var courseCodes = await GetListOfCourseCodes();

            await SerialiseObjectToJsonFile(courseCodes, "data/courses/Macquarie_Course_Codes");
        }

        public async Task<IEnumerable<IGrouping<string, MacquarieBasicItemInfo>>> GetListOfCourseCodes() {
            var apiRequest = new CourseApiRequestBuilder() { ImplementationYear = 2021, Limit = 250 };
            var courseCollection = await MacquarieHandbook.GetCMSDataCollection<MacquarieCourse>(apiRequest);

            var enumerable = (from course in courseCollection.AsEnumerable()
                              select new { course.Code, course.Title, course.CourseData.School.Value });
            var query = enumerable.Select(item => new MacquarieBasicItemInfo(item.Code, item.Title, item.Value)).OrderBy(item => item.Code).GroupBy(item => item.Department);

            return query;
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