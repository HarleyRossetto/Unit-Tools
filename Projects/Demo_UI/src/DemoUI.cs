using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Linq;
using System.Threading.Tasks;
using Demo_UI.src.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQHandbookLib.src.Helpers;

namespace Demo_UI.src;

public static class DemoUI
{
    public static bool RunCommandLoop { get; set; } = true;
    private static Parser parser;

    public static async Task Run(String[] args)
    {
        parser = BuildCommandLine()
            .UseHost(_ => Host.CreateDefaultBuilder(args), (builder) =>
            {
                builder.ConfigureServices((hostContext, services) =>
                    {
                        var configuration = hostContext.Configuration;

                        //Dependancies can follow..
                    })
                    .ConfigureLogging(logging => { logging.SetMinimumLevel(LogLevel.Warning); })
                    .UseCommandHandler<QuitCommand, QuitCommand.Handler>()
                    .UseCommandHandler<GetUnitCommand, GetUnitCommand.Handler>()
                    .UseCommandHandler<GetCourseCommand, GetCourseCommand.Handler>();
            })
            .UseDefaults()
            .Build();

        LocalDataDirectoryHelper.DirectoryHeader = @"C:\Users\accou\Desktop\MQ Uni Data Tools\Unit Tools\Docs";

        //If we have been provided any initial arguments
        if (args.Any())
            await parser.InvokeAsync(args);

        while (RunCommandLoop)
        {
            await parser.InvokeAsync(Console.ReadLine()); //Make the readline some form of interface.
        }
    }

    private static CommandLineBuilder BuildCommandLine()
    {
        var rootCommand = new RootCommand();
        rootCommand.AddCommand(new GetUnitCommand());
        rootCommand.AddCommand(new GetCourseCommand());
        rootCommand.AddCommand(new QuitCommand());

        var result = new CommandLineBuilder(rootCommand);
        return result;
    }
}