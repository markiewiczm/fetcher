using System.IO;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Extractor.Repository;
using Extractor.Jobs;
using Microsoft.EntityFrameworkCore;
using Extractor.Jobs.Config;
using Extractor.Jobs.Bridge;
using Extractor.Persistance.Repositories;
using Extractor.Services;
using Extractor.Services.Validatiors;
using Extractor.Services.Parsers;
using Extractor.Services.Services;

namespace Extractor.Worker
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddEnvironmentVariables();
                }).ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    var environment = hostContext.HostingEnvironment;
                    configApp.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
                    configApp.AddEnvironmentVariables();
                    Configuration = configApp.Build();
                }).ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    GlobalConfiguration.Configuration.UseSqlServerStorage(Configuration["Data:AppConnection:ConnectionString"]);

                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseSqlServer(Configuration["Data:AppConnection:ConnectionString"], b => { b.CommandTimeout(15); });
                    }, ServiceLifetime.Scoped);


                    services.AddHangfire(Configuration);
                    services.AddSingleton(Configuration);
                    services.AddSingleton(Configuration.GetSection("ParserJobConfig").Get<ParserConfig>());

                    services.AddScoped<IProductParser, CsvProductParser>();
                    services.AddScoped<IProductParser, XmlProductParser>();
                    services.AddScoped<IXmlSchemaValidator, XmlSchemaValidator>();
                    services.AddScoped<IProductRepository, ProductRepository>();
                    services.AddScoped<IProductExtractorService, ProductExtractorService>();
                    services.AddScoped<ProductParserFactory>();

                    var serviceProvider = services.BuildServiceProvider();
                    var conf = serviceProvider.GetRequiredService<ParserConfig>();

                    if (conf.Enabled)
                    {
                        RecurringJob.AddOrUpdate<ParserBridge>(conf.JobId, e => e.ExecuteJob(conf), conf.CronExpression);
                    }
                    else
                    {
                        RecurringJob.RemoveIfExists(conf.JobId);
                    }
                });

            await builder.RunConsoleAsync();
        }


    }
}