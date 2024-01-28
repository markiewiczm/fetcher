using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Extractor.Services.Models;

namespace Extractor.Services.Parsers
{

    public class XmlProductParser : ProductParser, IProductParser
    {
        public XmlProductParser() : base(FileSrcEnum.XML)
        {
        }

        public override List<ProductModel> FetchProductsFromFile(string filePath)
        {
            try
            {
                var xmlDoc = XDocument.Load(filePath);
                var result = from c in xmlDoc.Descendants("product")
                             select new ProductModel
                             {
                                 Id = (int)c.Attribute("id"),
                                 Price = (decimal)c.Element("price"),
                                 Quantity = (int)c.Element("quantity"),
                                 Name = (string)c.Element("name"),
                             };

                var list = result.ToList();
                return list;
            }

            catch (XmlException)
            {
                Console.WriteLine($"Xml is not valid");
                throw;
            }

            catch (Exception)
            {
                throw;
            }
        }

    }
}
