using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Demo_UI.src.Commands
{
    public class QuitCommand : Command
    {
        private const string CommandName = "Exit";
        private const string CommandDescription = "Exits the application";

        public QuitCommand() : base(CommandName, CommandDescription)
        {
        }

        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        public new class Handler : ICommandHandler
        {
            public async Task<int> InvokeAsync(InvocationContext context)
            {
                DemoUI.RunCommandLoop = false;
                return await Task.Run(() => 0); //Shuts up async warnings
            }
        }
    }
}