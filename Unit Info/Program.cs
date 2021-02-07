using System;
using System.IO;
using System.Threading.Tasks;
using Macquarie.Handbook;
using Macquarie.Handbook.Data;
using Macquarie.Handbook.WebApi;
using System.Diagnostics;

namespace Unit_Info
{
    class Program
    {
        async static Task Main(string[] args)
        {
            Program program = new Program();

            //await program.CustomApi_XXXXXXX();
            await program.CustomAPI_UnitDownloadAndTranslation();
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
    }
}