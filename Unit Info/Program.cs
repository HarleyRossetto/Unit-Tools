﻿
using System;
using System.Threading.Tasks;
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

            await program.CustomApiTest_RequestBuilder();
        }

        public async Task CustomApiTest_RequestBuilder()
        {
            HandbookApiRequestBuilder apiRequest = new UnitApiRequestBuilder() { ImplementationYear = 2021 };
            
            var unitCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieUnit>(apiRequest);
            
            MacquarieUnit unit = unitCollection[0];
            Console.WriteLine(unit.UnitData.ClassName);

            apiRequest = new CourseApiRequestBuilder() { ImplementationYear = 2021, Code = "C000105"};
            var courseCollection = await MacquarieHandbook.GetDataResponseCollection<MacquarieCourse>(apiRequest);
            MacquarieCourse course = courseCollection[0];
            Console.WriteLine(course.CourseData.CourseSearchTitle);
        }
    }
}