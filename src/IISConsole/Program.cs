using McMaster.Extensions.CommandLineUtils;
using System;
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
            var requestQueueLimitOption = app.Option("-l|--requestqueue-limit", "The length of the Http.Sys Request queue", CommandOptionType.SingleValue);
            var urlsOption = app.Option("-u|--urls", "URLs to listen on", CommandOptionType.MultipleValue);
            app.OnExecute(() =>
            {
                var workerProcessOptions = new WorkerProcessOptions();
                urlsOption.Values.ForEach(url => workerProcessOptions.UrlPrefixes.Add(url));
                if (requestQueueLimitOption.HasValue())
                {
                    if (Int32.TryParse(requestQueueLimitOption.Value(), out int limit))
                    {
                        workerProcessOptions.RequestQueueLimit = limit;
                    }
                }

                Console.WriteLine("Listening. Press Ctrl + C to stop listening...");
                using (var workerProcessHelper = new WorkerProcessHelper(workerProcessOptions))
                {
                    workerProcessHelper.Start();
                    exitEvent.WaitOne();
                }
                Console.WriteLine("Exiting");
                return;
            });

            app.Execute(args);
        }
    }
}
