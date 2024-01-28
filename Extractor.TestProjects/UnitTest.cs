using System.Xml.Serialization;
using Extractor.Services.Parsers;
using Extractor.Services.Validatiors;
using Extractor.Test;

namespace TestProject
{
    public class ParserTests
    {

        private XmlProductParser xmlParser;
        private XmlSchemaValidator xmlSchemaValidator;
        //private
        string dirName = "C:\\tmp";
        string xmlFileName = "C:\\tmp\\products.xml";
        string xsdFileName = "C:\\tmp\\products.xsd";

        [SetUp]
        public void Setup()
        {
            Directory.CreateDirectory(this.dirName);
            CreateXml();
            CreateXsd();
        }

        private void CreateXsd()
        {
            //From xsd creator
            var xsdText = @"<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""products"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""product"" maxOccurs=""unbounded"" minOccurs=""1"">
          <xs:complexType>
            <xs:sequence>
              <xs:element type=""xs:string"" name=""name""/>
              <xs:element type=""xs:integer"" name=""quantity""/>
              <xs:element type=""xs:float"" name=""price""/>
            </xs:sequence>
            <xs:attribute type=""xs:integer"" name=""id"" use=""required""/>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";

            File.WriteAllText(xsdFileName, xsdText);

        }

        private void CreateXml()
        {
            xmlParser = new XmlProductParser();

            var products = new CollectionOfProducts()
            {
                Products = new List<Product>(){new Product
                {
                    Id = 1,
                    Name = "Stolik niewywracalny",
                    Quantity = 18,
                    Price = 15.0M,
                }
            }};

            SerializeToXml(products, this.xmlFileName);
        }

        [Test]
        public void ShouldParseDataFromXml()
        {
            string path = this.xmlFileName;
            var products = xmlParser.FetchProductsFromFile(path);

            Assert.IsNotNull(products);
            Assert.IsTrue(products.Any());

        }

        [Test]
        public void ShouldBeValidWithXsd()
        {
            xmlSchemaValidator = new XmlSchemaValidator();
            Assert.DoesNotThrow(() => xmlSchemaValidator.ValidateSchema(this.xsdFileName, this.xmlFileName));

        }


        private static void SerializeToXml<T>(T obj, string fileName)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Truncate))
            {
                var ser = new XmlSerializer(typeof(T));
                ser.Serialize(fileStream, obj);
            }
        }

    }
}