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

            //await program.CustomApi_XXXXXXX();

            //await program.CustomAPI_CourseDownloadAndTranslation();

            await program.CustomAPI_UnitDownloadAndTranslation();

            //await program.CustomAPI_GetCourse("C000006");

            //await program.CustomAPI_GetUnit("COMP1000", true);
            //await program.CustomAPI_GetUnit("EDTE3010");
            /*
                EDTE3010 - has larger pre-requsite chain
            */

            //await program.SaveListOfCourseCodesAndTitles();

            //await program.SaveListOfUnitCodesAndTitles();

            //Downloads all units and saves a copy of only the prerequisite enrolment rules.
            //await program.CustomAPI_GetAllUnitPrerequsiteForDevelopment();
        }

        /// <summary>
        /// Demonstrates Unit request API creation, data collection and access.
        /// </summary>
        public async Task CustomAPI_GetUnit(String unitCode, bool writeToFile = true) {
            HandbookApiRequestBuilder apiRequest = new UnitApiRequestBuilder() { ImplementationYear = 2021, Code = unitCode };
            var unitCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieUnit>(apiRequest);

            if (unitCollection.Count > 0) {
                MacquarieUnit unit = unitCollection[0];
                Console.WriteLine(unit.UnitData.Title);

                if (writeToFile)
                    await SerialiseObjectToJsonFile(unit, $"data/units/{unit.Code}");

            } else {
                Console.WriteLine("Unit with code '{0}' was not found.", apiRequest.Code);
            }
        }

        /// <summary>
        /// Demonstrates Course request API creation, data collection and access.
        /// </summary>
        /// <param name="courseCode">
        /// The course code to attempt to retreive.
        /// </param> 
        public async Task CustomAPI_GetCourse(String courseCode) {
            Stopwatch sw = new Stopwatch();

            var apiRequest = new CourseApiRequestBuilder() { ImplementationYear = 2021, Code = courseCode };

            sw.Start();
            var courseCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieCourse>(apiRequest);
            sw.Stop();

            if (courseCollection.Count > 0) {
                MacquarieCourse course = courseCollection[0];

                Console.WriteLine(course.CourseData.CourseSearchTitle);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Course retrevial & deserialisation took {0} milliseconds.", sw.ElapsedMilliseconds);

                //await WriteObjectToJsonFileInDataDir(course, "Test_Course_Serialisation");
            } else {
                Console.WriteLine("No course with code '{0}' was found.", courseCode);
            }
        }

        /// <summary>
        /// Demonstrates Course request API creation, data collection and access.
        /// Requests all 2021 courses, with a limit of 250 results.
        /// </summary>
        public async Task CustomAPI_CourseDownloadAndTranslation() {
            Stopwatch sw = new Stopwatch();

            var apiRequest = new CourseApiRequestBuilder() { ImplementationYear = 2021, Limit = 250 };

            Console.WriteLine(apiRequest.ToString());

            sw.Restart();
            var courseCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieCourse>(apiRequest);
            sw.Stop();

            if (courseCollection.Collection.Count > 0) {
                var enumerable = courseCollection.Collection.AsEnumerable().OrderBy(crs => crs.Code).GroupBy(crs => crs.CourseData.School.Value);
                await SerialiseObjectToJsonFile(enumerable, "data/courses/Macquarie_Courses");

                Console.WriteLine("{0} milliseconds for {1} course query & deserialisation.", sw.ElapsedMilliseconds, courseCollection.Count);
            }
        }

        /// <summary>
        /// Demonstrates Unit request API creation, data collection and access.
        /// Requests all 2021 Units, with a limit of 3000 results.
        /// </summary>
        public async Task CustomAPI_UnitDownloadAndTranslation() {
            Stopwatch sw = new Stopwatch();

            var apiRequest = new UnitApiRequestBuilder() { ImplementationYear = 2021, Limit = 3000 };

            Console.WriteLine(apiRequest.ToString());

            sw.Restart();
            var unitCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieUnit>(apiRequest);

            //var enumerable = unitCollection.Collection.AsEnumerable().OrderBy(unit => unit.Code).GroupBy(unit => unit.UnitData.School.Value);
            
            foreach (var unit in unitCollection.Collection) {
                await SerialiseObjectToJsonFile(unit, $"data/units/{unit.Code}");
            }
            
            //await WriteObjectToJsonFile(enumerable, "data/Macquarie_Units");
            sw.Stop();

            Console.WriteLine("{0} milliseconds for {1} unit query & deserialisation.", sw.ElapsedMilliseconds, unitCollection.Count);
        }

        public async Task CustomAPI_GetAllUnitPrerequsiteForDevelopment() {
            Stopwatch sw = new Stopwatch();

            var apiRequest = new UnitApiRequestBuilder() { ImplementationYear = 2021, Limit = 3000 };

            Console.WriteLine(apiRequest.ToString());

            sw.Restart();
            var unitCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieUnit>(apiRequest);


            Boolean exportWithSubjectCode = false;

            if (exportWithSubjectCode) {
                var ruleAndCode = new List<Tuple<string, string>>();
                foreach (var i in unitCollection.Collection) {
                    foreach (var j in i.UnitData.EnrolmentRules) {
                        if (j.Type.Value == "prerequisite") {
                            ruleAndCode.Add(new Tuple<string, string>(i.Code, j.Description));
                        }
                    }
                }

                var orderedList = ruleAndCode.OrderBy(i => i.Item2.Length);

                await SerialiseObjectToJsonFile(orderedList, "data/units/prerequisites/unparsed/Macquarie_EnrolmentRules_Order_LENGTH_with_TITLE");
            } else {
                var prerequisites = from enrolementRule in (from t2 in unitCollection.Collection from enrolementRules in t2.UnitData.EnrolmentRules select enrolementRules).ToList()
                                    where enrolementRule.Type.Value == "prerequisite"
                                    orderby enrolementRule.Description.Length
                                    select enrolementRule.Description;

                await SerialiseObjectToJsonFile(prerequisites, "data/units/prerequisites/unparsed/Macquarie_EnrolmentRules_Order_LENGTH_Sanitised");
            }

            sw.Stop();

            Console.WriteLine("{0} milliseconds for {1} unit query & deserialisation.", sw.ElapsedMilliseconds, unitCollection.Count);
        }

        public async Task SaveListOfUnitCodesAndTitles() {
            var unitCodes = await GetListOfUnitCodes();

            await SerialiseObjectToJsonFile(unitCodes, "Macquarie_Unit_Codes");
        }

        public async Task<IEnumerable<IGrouping<string, MacquarieBasicItemInfo>>> GetListOfUnitCodes() {
            var apiRequest = new UnitApiRequestBuilder() { ImplementationYear = 2021, Limit = 2500 };
            var unitCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieUnit>(apiRequest);
            // List<CourseBasicInfo> courseList =
            //             (from course in courseCollection.Collection
            //             select course.Code, course.).ToList();

            var enumerable = (from unit in unitCollection.Collection
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
            var courseCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieCourse>(apiRequest);

            var enumerable = (from course in courseCollection.Collection
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