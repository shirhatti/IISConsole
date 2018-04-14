using McMaster.Extensions.CommandLineUtils;
using System;
using System.Diagnostics;
using System.Threading;

namespace IISConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitEvent = new ManualResetEvent(false);
            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                exitEvent.Set();
            };

            var app = new CommandLineApplication
            {
                Name = "iisconsole",
                FullName = "IIS Console",
                Description = "Commandline Utility for running IIS worker process"
            };

            app.HelpOption("-h|--help");

            app.OnExecute(() =>
            {
                var workerProcessHelper = new WorkerProcessHelper();
                workerProcessHelper.Start();
                Console.WriteLine("Listening. Press Ctrl + C to stop listening...");
                exitEvent.WaitOne();
                Console.WriteLine("Exiting");
                workerProcessHelper.Stop();
                return;
            });

            app.Execute(args);
        }
    }
}
