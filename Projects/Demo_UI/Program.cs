using System;
using System.Threading.Tasks;
using Demo_UI.src;

namespace Demo_UI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new DemoUI().Run(args);
        }
    }
}
