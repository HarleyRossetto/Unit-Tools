using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Demo_UI.src.Commands;
using Microsoft.Extensions.Hosting;

namespace Demo_UI.src
{
    public class DemoUI
    {
        private Parser parser;

        public async Task Run(String[] args) {
            parser = BuildCommandLine()
                        .UseHost(_ => Host.CreateDefaultBuilder(args), (builder) =>
                        {
                            builder.ConfigureServices((hostContext, services) => {
                                var configuration = hostContext.Configuration;
                                //Dependancies
                            })
                            .UseCommandHandler<GetUnitCommand, GetUnitCommand.Handler>();
                        }).UseDefaults().Build();

            await parser.InvokeAsync(args);

            // root = new RootCommand("Root Command Description");
            // root.Handler = CommandHandler.Create(() => root.Invoke("-h"));

            // builder = new CommandLineBuilder(root);

            // parser = builder.UseDefaults().Build();
        }

        private static CommandLineBuilder BuildCommandLine() {
            var rootCommand = new RootCommand();
            rootCommand.AddCommand(new GetUnitCommand());
            return new CommandLineBuilder(rootCommand);
        }

    }
}