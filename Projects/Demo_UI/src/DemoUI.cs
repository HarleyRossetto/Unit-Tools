using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;
using System.Threading.Tasks;
using Demo_UI.src.Commands;
using Microsoft.Extensions.Hosting;

namespace Demo_UI.src
{
    public static class DemoUI
    {
        private static Parser parser;

        public static async Task Run(String[] args) {
            parser = BuildCommandLine()
                        .UseHost(_ => Host.CreateDefaultBuilder(args), (builder) =>
                        {
                            builder.ConfigureServices((hostContext, services) => {
                                var configuration = hostContext.Configuration;
                                //Dependancies can follow..
                            })
                            .UseCommandHandler<QuitCommand, QuitCommand.Handler>()
                            .UseCommandHandler<GetUnitCommand, GetUnitCommand.Handler>()
                            .UseCommandHandler<GetCourseCommand, GetCourseCommand.Handler>();
                        }).UseDefaults().Build();

            //If we have been provided any initial arguments
            if (args.Any())
                await parser.InvokeAsync(args);

            while (RunCommandLoop) {
                await parser.InvokeAsync(Console.ReadLine()); //Make the readline some form of interface.
            }
        }

        private static CommandLineBuilder BuildCommandLine() {
            var rootCommand = new RootCommand();
            rootCommand.AddCommand(new GetUnitCommand());
            rootCommand.AddCommand(new GetCourseCommand());
            rootCommand.AddCommand(new QuitCommand());
            return new CommandLineBuilder(rootCommand);
        }

        public static bool RunCommandLoop { get; set; } = true;

    }
}