using Extractor.Repository.Entities;

namespace Extractor.Data.Entities
{
    public class Product : IEntityBase<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}