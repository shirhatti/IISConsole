using IISConsole.IISConfiguration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IISConsole
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration(configuration =>
                {
                    configuration.AddJsonFile("appsettings.json", optional: true);
                    configuration.AddCommandLine(args);
                })
                .ConfigureServices(services =>
                {
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .UseConsoleLifetime();

            await builder.RunConsoleAsync();
        }
    }
}
