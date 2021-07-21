using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Macquarie.Handbook;
using Macquarie.Handbook.Converters;

namespace Demo_UI.src.Commands
{
    public class GetUnitCommand : Command
    {
        private const string CommandName = "GetUnit";
        private const string CommandDescription  = "Retreives the specified unit";

        public GetUnitCommand() : base(CommandName, CommandDescription) { }

        public override string Name { get => base.Name; set => base.Name = value; }

        public new class Handler : ICommandHandler
        {
            public async Task<int> InvokeAsync(InvocationContext context) {
                await Task.Delay(1000);


                var unit = await MacquarieHandbook.GetUnit("COMP2100");

                System.Console.WriteLine(unit.ToString());

                return 0;
            }
        }
    }
}