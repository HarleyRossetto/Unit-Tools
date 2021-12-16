namespace Unit_Info;

using System.Linq;
using System.Threading.Tasks;
using Macquarie.Handbook;
using Unit_Info.Demonstrations;
using Unit_Info.Helpers;


class Program
{
    async static Task Main(string[] args) {
        /*
            0 = Resource to get type (unit|course)
            1 = Resource code (COMP1010|C000006)
        */

        if (args.Length >= 2) {

            LocalDataMap.LoadCache();
            var handbook = new MacquarieHandbook();

            switch (args[0].ToLower()) {
                case "unit":
                    _ = await handbook.GetUnit(args[1], 2021);
                    break;
                case "course":
                    _ = await handbook.GetCourse(args[1], 2021);
                    break;
            }

            await LocalDataMap.SaveCacheAsync();
        }

        //await Demo.ProcessPrereqs();

        // var course = await MacquarieHandbook.GetCourse("C000006", 2021);

        // var unit = await MacquarieHandbook.GetUnit("COMP2100", 2021);

    }
}
