using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using Macquarie.Handbook;
using Macquarie.Handbook.Converters;

namespace Demo_UI.src.Commands
{
    public class GetUnitCommand : Command
    {
        private const string CommandName = "GetUnit";
        private const string CommandDescription = "Retreives the specified unit";

        public readonly Option<int> OptionLimit = new(  new string[] {
                                                                "--l",
                                                                "--limit"
                                                            }, 
                                                            () => 10, 
                                                            "Maximum number of records to retrieve");

        public readonly Option<string> OptionUnitCode = new(new string[] {
            "--unitcode",
            "--unit",
            "--u"
        });

        public readonly Option<int?> OptionYear = new(new string[] {
            "--year",
            "--y"
        }, _ => DateTime.Now.Year);

        public GetUnitCommand() : base(CommandName, CommandDescription) {
            AddOption(OptionYear);
            AddOption(OptionUnitCode);
            AddOption(OptionLimit);
        }

        public override string Name { get => base.Name; set => base.Name = value; }

        public new class Handler : ICommandHandler
        {

            public string Unit { get; set; }
            public int Limit { get; set; }
            public int? Year { get; set; }

            public async Task<int> InvokeAsync(InvocationContext context) {
                if (Unit is not null) {
                    var unit = await MacquarieHandbook.GetUnit(Unit, Year);

                    if (unit is not null) {
                        Console.WriteLine(unit.ToString());
                        return 0;
                    }
                    return -1;
                } else {
                    var units = await MacquarieHandbook.GetAllUnits(Year, Limit);
                    if (units is not null) {
                        return 0;
                    }
                    return -1;
                }
            }
        }
    }
}