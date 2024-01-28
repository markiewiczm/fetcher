using System;
using Hangfire;
using Hangfire.Console;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Extractor.Worker
{
    internal static class HangfireConfigurationExtensions
    {

        public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["Data:AppConnection:ConnectionString"];
            var serverName = configuration["Hangfire:ServerName"];
            var jobExpirationDays = Convert.ToInt32(configuration["Hangfire:JobExpirationInDays"]);
            var workerCount = Convert.ToInt32(configuration["Hangfire:WorkerCount"]);

            services.AddHangfire(c => c
                .UseSqlServerStorage(connectionString,
                    new SqlServerStorageOptions
                    {
                        PrepareSchemaIfNecessary = false
                    })
                .UseConsole(new ConsoleOptions
                {
                    ExpireIn = TimeSpan.FromDays(jobExpirationDays)
                })
                );

            services.AddHangfireServer(c =>
            {
                c.ServerName = serverName;
                c.WorkerCount = workerCount;
                c.Queues = new[] { "default" };
            });

            return services;
        }
    }
}
