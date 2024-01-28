using Extractor.Data.Entities;
using Extractor.Repository;

namespace Extractor.Persistance.Repositories
{
    public interface IProductRepository
    {
        List<Product> GetProducts();
        void SaveProduct(Product product);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;
        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public List<Product> GetProducts()
        {
            var products = _appDbContext.Products.ToList();
            return products;
        }

        public void SaveProduct(Product product)
        {
            var productDb = _appDbContext.Products.FirstOrDefault(x => x.Id == product.Id);

            if (productDb == null)
            {
                _appDbContext.Products.Add(new Product { Id = product.Id, Name = product.Name, Quantity = product.Quantity, Price = product.Price });
            }

            else
            {
                productDb.Price = product.Price;
                productDb.Quantity = product.Quantity;
                productDb.Name = product.Name;
            }

            _appDbContext.SaveChanges();

        }
    }
}
