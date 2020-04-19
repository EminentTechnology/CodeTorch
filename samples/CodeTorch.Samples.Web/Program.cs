using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeTorch.Abstractions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CodeTorch.Samples.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Create a new scope
            using (var scope = host.Services.CreateScope())
            {
                // Get the DbContext instance
                var store = scope.ServiceProvider.GetRequiredService<IConfigurationStore>();

                //Do the migration asynchronously
                CodeTorch.Core.ConfigurationLoader.Store = store;
                await CodeTorch.Core.ConfigurationLoader.LoadConfiguration();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = 
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            builder.ConfigureLogging((context, b) =>
            {
                b.AddFilter("Microsoft", LogLevel.Warning);
                b.AddFilter("System", LogLevel.Warning);
                b.SetMinimumLevel(LogLevel.Debug);
                b.AddConsole();
            });

            return builder;

        }
    }
}
