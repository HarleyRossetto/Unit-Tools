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

            await program.CustomApiTest_RequestBuilder();
        }

        public async Task CustomApiTest_RequestBuilder()
        {
            Stopwatch sw = new Stopwatch();

            // HandbookApiRequestBuilder apiRequest = new UnitApiRequestBuilder() { ImplementationYear = 2021 };
            // var unitCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieUnit>(apiRequest);
            // MacquarieUnit unit = unitCollection[0];
            // Console.WriteLine(unit.UnitData.ClassName);

            // apiRequest = new CourseApiRequestBuilder() { ImplementationYear = 2021, Code = "C000105" };
            // sw.Start();
            // var courseCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieCourse>(apiRequest);
            // sw.Stop();

            // MacquarieCourse course = courseCollection[0];
            // Console.WriteLine(course.CourseData.CourseSearchTitle);
            // Console.ForegroundColor = ConsoleColor.Red;
            // Console.WriteLine("{0} milliseconds for 2nd request.", sw.ElapsedMilliseconds);

            // Console.ForegroundColor = ConsoleColor.White;

            var apiRequest = new CourseApiRequestBuilder() { ImplementationYear = 2021, Limit = 10 };
            Console.WriteLine(apiRequest.ToString());
            sw.Restart();
            var courseCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieCourse>(apiRequest);
            sw.Stop();
            Console.WriteLine(courseCollection.Collection[3].Title);
            Console.WriteLine("{0} milliseconds for 10 course query & deserialisation.", sw.ElapsedMilliseconds);

            Console.Read();
        }
    }
}