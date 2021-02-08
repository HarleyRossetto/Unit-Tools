using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using Macquarie.Handbook;
using Macquarie.Handbook.Data;
using Macquarie.Handbook.WebApi;

namespace Unit_Info
{
    class Program
    {
        async static Task Main(string[] args)
        {
            Program program = new Program();

            //await program.CustomApi_XXXXXXX();

            //await program.CustomAPI_CourseDownloadAndTranslation();

            // await program.SaveListOfCourseCodesAndTitles();

            // await program.SaveListOfUnitCodesAndTitles();

            
        }

        /// <summary>
        /// Demonstrates Unit request API creation, data collection and access.
        /// </summary>
        public async Task CustomAPI_GetUnit()
        {
            HandbookApiRequestBuilder apiRequest = new UnitApiRequestBuilder() { ImplementationYear = 2021 };
            var unitCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieUnit>(apiRequest);
            MacquarieUnit unit = unitCollection[0];
            Console.WriteLine(unit.UnitData.ClassName);
        }

        /// <summary>
        /// Demonstrates Course request API creation, data collection and access.
        /// </summary>
        public async Task CustomAPI_GetCourse()
        {
            Stopwatch sw = new Stopwatch();

            var apiRequest = new CourseApiRequestBuilder() { ImplementationYear = 2021, Code = "C000105" };

            sw.Start();
            var courseCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieCourse>(apiRequest);
            sw.Stop();

            MacquarieCourse course = courseCollection[0];

            Console.WriteLine(course.CourseData.CourseSearchTitle);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0} milliseconds for 2nd request.", sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// Demonstrates Course request API creation, data collection and access.
        /// Requests all 2021 courses, with a limit of 250 results.
        /// </summary>
        public async Task CustomAPI_CourseDownloadAndTranslation()
        {
            Stopwatch sw = new Stopwatch();

            var apiRequest = new CourseApiRequestBuilder() { ImplementationYear = 2021, Limit = 250 };

            Console.WriteLine(apiRequest.ToString());

            sw.Restart();
            var courseCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieCourse>(apiRequest);
            sw.Stop();

            var jsonString = JsonConvert.SerializeObject(courseCollection, Formatting.Indented);
            await File.WriteAllTextAsync(string.Format("C:/Users/accou/Desktop/MQ Uni Data Tools/Unit Tools/Unit Info/data/{0}_{1}.json",
                                                        "Macquare_Courses",
                                                        DateTime.Now.ToString("yyMMdd_HHmmssfffff")),
                                                        jsonString);

            Console.WriteLine(courseCollection.Collection[3].Title);
            Console.WriteLine("{0} milliseconds for {1} course query & deserialisation.", sw.ElapsedMilliseconds, courseCollection.Count);
        }

        /// <summary>
        /// Demonstrates Unit request API creation, data collection and access.
        /// Requests all 2021 Units, with a limit of 250 results.
        /// </summary>
        public async Task CustomAPI_UnitDownloadAndTranslation()
        {
            Stopwatch sw = new Stopwatch();

            var apiRequest = new UnitApiRequestBuilder() { ImplementationYear = 2021, Limit = 3000 };

            Console.WriteLine(apiRequest.ToString());

            sw.Restart();
            var unitCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieUnit>(apiRequest);
            sw.Stop();

            Console.WriteLine("{0} milliseconds for {1} unit query & deserialisation.", sw.ElapsedMilliseconds, unitCollection.Count);

            Console.Read();
        }

        public async Task SaveListOfUnitCodesAndTitles()
        {
            var unitCodes = await GetListOfUnitCodes();
            var jsonString = JsonConvert.SerializeObject(unitCodes, Formatting.Indented);
            await File.WriteAllTextAsync(string.Format("C:/Users/accou/Desktop/MQ Uni Data Tools/Unit Tools/Unit Info/data/{0}_{1}.json",
                                                        "Macquare_Unit_Codes",
                                                        DateTime.Now.ToString("yyMMdd_HHmmssfffff")),
                                                        jsonString);
        }

        public async Task<IEnumerable<IGrouping<string, MacquarieBasicItemInfo>>> GetListOfUnitCodes()
        {
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

        public async Task SaveListOfCourseCodesAndTitles()
        {
            var courseCodes = await GetListOfCourseCodes();
            var jsonString = JsonConvert.SerializeObject(courseCodes, Formatting.Indented);
            await File.WriteAllTextAsync(string.Format("C:/Users/accou/Desktop/MQ Uni Data Tools/Unit Tools/Unit Info/data/{0}_{1}.json",
                                                        "Macquare_Course_Codes",
                                                        DateTime.Now.ToString("yyMMdd_HHmmssfffff")),
                                                        jsonString);
        }

        public async Task<IEnumerable<IGrouping<string, MacquarieBasicItemInfo>>> GetListOfCourseCodes()
        {
            var apiRequest = new CourseApiRequestBuilder() { ImplementationYear = 2021, Limit = 250 };
            var courseCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieCourse>(apiRequest);
            // List<CourseBasicInfo> courseList =
            //             (from course in courseCollection.Collection
            //             select course.Code, course.).ToList();

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

        public MacquarieBasicItemInfo(string pCode, string pTitle, string pDepartment)
        {
            this.Code = pCode;
            this.Title = pTitle;
            this.Department = pDepartment;
        }
    }
}