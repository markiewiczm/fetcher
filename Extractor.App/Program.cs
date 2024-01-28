using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Extractor.ConsoleApp;
using Extractor.ConsoleApp.Config;
using Extractor.ConsoleApp.Models;

namespace ConsoleApp1
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        static async Task Main(string[] args)
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
                }).ConfigureServices(async (hostContext, services) =>
                {
                    services.AddOptions();
                    services.AddSingleton(Configuration);
                    services.AddSingleton(Configuration.GetSection("ProductAPI").Get<ProductApiConfig>());
                    services.AddScoped<IProductsHttpClient, ProductsHttpClient>();
                    services.AddHttpClient("ProductAPI", client =>
                    {
                        client.BaseAddress = new Uri(Configuration["ProductAPI:Host"]);
                    });

                    var serviceProvider = services.BuildServiceProvider();

                    var client = serviceProvider.GetRequiredService<IProductsHttpClient>();
                    var products = await client.GetProducts();
                    Console.WriteLine(ShowProducts(products));

                });

            await builder.RunConsoleAsync();
        }

        private static string ShowProducts(List<ProductModel> products)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Produkty:");

            foreach (var item in products)
            {
                sb.AppendLine($"Id : {item.Id}");
                sb.AppendLine($"Nazwa : {item.Name}");
                sb.AppendLine($"Cena : {item.Price}");
                sb.AppendLine($"Ilość : {item.Quantity}");
                sb.AppendLine($"-----------------------------");
            }

            return sb.ToString();
        }
    }
}
