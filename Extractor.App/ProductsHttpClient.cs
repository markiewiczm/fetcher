using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Extractor.ConsoleApp.Config;
using Extractor.ConsoleApp.Models;

namespace Extractor.ConsoleApp
{
    public interface IProductsHttpClient
    {
        Task<List<ProductModel>> GetProducts();
    }

    public class ProductsHttpClient : IProductsHttpClient
    {
        private readonly ProductApiConfig _cfg;
        private readonly IHttpClientFactory _clientFactory;

        public ProductsHttpClient(IHttpClientFactory httpClientFactory, ProductApiConfig config)
        {
            _clientFactory = httpClientFactory;
            _cfg = config;
        }

        public async Task<List<ProductModel>> GetProducts()
        {
            try
            {
                using (HttpClient client = _clientFactory.CreateClient(_cfg.ClientName))
                {
                    var response = await client.GetAsync($"{_cfg.Endpoint}");
                    response.EnsureSuccessStatusCode();
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<List<ProductModel>>(responseBody);
                    return products;
                }
            }
            catch (HttpRequestException rex)
            {
                throw;
            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}
