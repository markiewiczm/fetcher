using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Xml.Serialization;

namespace Extractor.Test
{
    [XmlType("product")]
    public class Product
    {
        [XmlAttribute("id", DataType = "int")]
        public int Id { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("quantity")]
        public int Quantity { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

    }


    [XmlRoot("products")]
    public class CollectionOfProducts
    {
        [XmlElement("product")]
        public List<Product> Products { get; set; } = new List<Product>();
    }

}
