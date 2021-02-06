
using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
// using System.Text.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

using Macquarie.Handbook;
using Macquarie.Handbook.Data;
using Macquarie.Handbook.WebApi;
using Macquarie.Handbook.Data.Unit;
using Macquarie.Handbook.Data.Shared;

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
            HandbookApiRequestBuilder apiRequest = new UnitApiRequestBuilder() { ImplementationYear = 2021 };
            
            var unitCollection = await MacquarieHandbook.GetDataResponseCollection<Macquarie.Handbook.Data.MacquarieUnit>(apiRequest);
            
            MacquarieUnit unit = unitCollection[0];
            unit.DeserialiseInnerJson();
            Console.WriteLine(unit.UnitData.ClassName);

            apiRequest = new CourseApiRequestBuilder() { ImplementationYear = 2021, Code = "C000105"};
            var courseCollection = await MacquarieHandbook.GetDataResponseCollection<Macquarie.Handbook.Data.MacquarieCourse>(apiRequest);
            MacquarieCourse course = courseCollection[0];
            course.DeserialiseInnerJson();
            Console.WriteLine(course.CourseData.CourseSearchTitle);
        }
    }
}