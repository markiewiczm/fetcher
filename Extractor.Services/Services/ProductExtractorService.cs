using Model;
using Extractor.Data.Entities;
using Extractor.Persistance.Repositories;

namespace Extractor.Services.Services
{
    public interface IProductExtractorService
    {
        void ImportData(List<ProductModel> products);
    }

    public class ProductExtractorService : IProductExtractorService
    {
        private readonly IProductRepository _productRepository;

        public ProductExtractorService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void ImportData(List<ProductModel> products)
        {
            foreach (var product in products)
            {
                _productRepository.SaveProduct(new Product
                {
                    Name = product.Name,
                    Id = product.Id,
                    Quantity = product.Quantity,
                    Price = product.Price
                });
            }
        }
    }
}