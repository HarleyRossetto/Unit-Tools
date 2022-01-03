using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Threading.Tasks;
using MQHandbookLib.src.Helpers;
using MQHandbookLib.src.Macquarie.Handbook;

namespace Demo_UI.src.Commands;

public class GetUnitCommand : Command
{
    private const string CommandName = "GetUnit";
    private const string CommandDescription = "Retreives the specified unit";

    public readonly Option<int?> OptionYear = new(new string[]
    {
            "--year",
            "--y"
    }, _ => DateTime.Now.Year);

    public GetUnitCommand() : base(CommandName, CommandDescription) {
        //AddArgument(new Argument<string>());
        AddOption(new Option<int?>(new string[]
            {
                    "--year",
                    "--y"
            },
            _ => DateTime.Now.Year));
        AddOption(new Option<string>(new string[]
        {
                "--unitcode",
                "--unit",
                "--u"
        }));
        AddOption(new Option<int>(new string[]
            {
                    "--l",
                    "--limit"
            },
            () => 10,
            "Maximum number of records to retrieve"));
    }

    public override string Name {
        get => base.Name;
        set => base.Name = value;
    }

    public new class Handler : ICommandHandler
    {
        public string Unit { get; set; }
        public int Limit { get; set; }
        public int? Year { get; set; }
        public IConsole Console { get; set; }

        public async Task<int> InvokeAsync(InvocationContext context) {
            var handbook = new MacquarieHandbook(default, default, new DateTimeProvider());
            if (Unit is not null) {
                var unit = await handbook.GetUnit(Unit, Year, false);

                if (unit is not null) {
                    Console.Out.WriteLine(unit.ToString());
                    return 0;
                }
                return -1;
            } else {
                var units = await handbook.GetAllUnits(Year, Limit);
                if (units is not null) {
                    return 0;
                }

                return -1;
            }
        }
    }
}
