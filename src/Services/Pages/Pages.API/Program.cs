using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pages.API.Infrastructure;
using Serilog;

namespace Pages.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build()
                .MigrateNeo4jContext<PagesContext>((context, services) => {
                    //var logger = services.GetService<ILogger<MarketingContextSeed>>();

                    //new MarketingContextSeed()
                        //.SeedAsync(context, logger)
                        //.Wait();
                })
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    var builtConfig = config.Build();
                    var configurationBuilder = new ConfigurationBuilder();
                    configurationBuilder.AddEnvironmentVariables();
                    config.AddConfiguration(configurationBuilder.Build());
                })
                .ConfigureLogging((hostingContext, builder) =>
                {
                    builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    builder.AddAzureWebAppDiagnostics();
                    builder.AddApplicationInsights();
                    builder.AddConsole();
                    builder.AddDebug();
                })
                .UseSerilog((builderContext, config) =>
                {
                    config
                        .MinimumLevel.Information()
                        .Enrich.FromLogContext()
                        .WriteTo.Console();
                });
    }
}
