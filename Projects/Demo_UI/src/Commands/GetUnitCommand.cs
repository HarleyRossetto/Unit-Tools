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
        private const string CommandDescription  = "Retreives the specified unit";

        public GetUnitCommand() : base(CommandName, CommandDescription) {
            AddArgument(new Argument<string>("UnitCode"));
            AddOption(new Option<int>("--year"));
        }

        public override string Name { get => base.Name; set => base.Name = value; }

        public new class Handler : ICommandHandler
        {
            public async Task<int> InvokeAsync(InvocationContext context) {

                var unitcode = context.ParseResult.ValueForArgument<string>("UnitCode");

                int? year = null;
                if (context.ParseResult.HasOption("--year")) {
                    year = context.ParseResult.ValueForOption<int>("--year");
                }

                var unit = await MacquarieHandbook.GetUnit(unitcode, year);

                Console.WriteLine(unit.ToString());

                return 0;
            }
        }
    }
}