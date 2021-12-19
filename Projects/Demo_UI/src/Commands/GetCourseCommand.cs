using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Macquarie.Handbook;

namespace Demo_UI.src.Commands;

public class GetCourseCommand : Command
{
    private const string CommandName = "GetCourse";
    private const string CommandDescription = "Retreives the specified course";

    public GetCourseCommand() : base(CommandName, CommandDescription) {
        AddArgument(new Argument<string>("CourseCode"));
        AddOption(new Option<int>("--year"));
    }

    public override string Name {
        get => base.Name;
        set => base.Name = value;
    }

    public new class Handler : ICommandHandler
    {
        public async Task<int> InvokeAsync(InvocationContext context) {
            var courseCode = context.ParseResult.ValueForArgument<string>("CourseCode");

            int? year = null;
            if (context.ParseResult.HasOption("--year")) {
                year = context.ParseResult.ValueForOption<int>("--year");
            }

            var handbook = new MacquarieHandbook(default);
            var unit = await handbook.GetCourse(courseCode, year);

            Console.WriteLine(unit.ToString());

            return 0;
        }
    }
}
