//#define IGNORE_UNNECESSARY

using System.Threading.Tasks;
using System.Linq;

using Unit_Info.Helpers;
using Macquarie.Handbook;

using Unit_Info.Demonstrations;

namespace Unit_Info
{
    class Program
    {
        async static Task Main(string[] args) {
            /*
                0 = Resource to get type (unit|course)
                1 = Resource code (COMP1010|C000006)
            */

            if (args.Length < 2) return;

            LocalDataMap.LoadCache();

            switch (args[0].ToLower()) {
                case "unit":
                    var unit = await MacquarieHandbook.GetUnit(args[1], 2021);
                    break;
                case "course":
                    var course = await MacquarieHandbook.GetCourse(args[1], 2021);
                    break;
            }

            // var course = await MacquarieHandbook.GetCourse("C000006", 2021);

            // var unit = await MacquarieHandbook.GetUnit("COMP2100", 2021);

            await LocalDataMap.SaveCacheAsync();
        }        
    }
}