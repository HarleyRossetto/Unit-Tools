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

using static Unit_Info.Helpers.LocalDataDirectoryHelper;
using static Unit_Info.Helpers.LocalDirectories;
using Unit_Info.Helpers;
using Macquarie.Handbook.Data.Shared;

namespace Unit_Info
{
    class Program
    {
        async static Task Main(string[] args) {
            Program program = new();
            
            LocalDataMap.LoadCache();

            var course = await MacquarieHandbook.GetCourse("C000006", 2021);

            var unit = await MacquarieHandbook.GetUnit("COMP2100", 2021);

            await LocalDataMap.SaveCacheAsync();
        }

        

        
        
    }
}