using System.Xml;
using System.Xml.Schema;

namespace Extractor.Services.Validatiors
{
    public interface IXmlSchemaValidator
    {
        void ValidateSchema(string xsdPath, string xmlPath);
    }
    public class XmlSchemaValidator : IXmlSchemaValidator
    {
        public void ValidateSchema(string xsdPath, string xmlPath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlPath);
            xml.Schemas.Add(null, xsdPath);

            try
            {
                xml.Validate(null);
            }

            catch (XmlSchemaValidationException)
            {
                throw;
            }
        }
    }
}
