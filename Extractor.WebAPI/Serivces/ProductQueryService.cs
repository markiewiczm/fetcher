using Extractor.Persistance.Repositories;
using Extractor.WebAPI.ViewModels;

namespace Extractor.WebAPI.Queries
{
    public interface IProductQueryService
    {
        List<ProductDTO> GetProducts();
    }

    public class ProductQueryService : IProductQueryService
    {
        private readonly IProductRepository _productRepository;

        public ProductQueryService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<ProductDTO> GetProducts()
        {
            return _productRepository.GetProducts()
                    .Select(x => new ProductDTO
                    {
                        Id = x.Id,
                        Quantity = x.Quantity,
                        Price = x.Price,
                        Name = x.Name,
                    }).ToList();
        }
    }
}
