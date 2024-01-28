using System.Reflection;
using System.ComponentModel;
using Extractor.Services;
using Extractor.Services.Validatiors;
using Extractor.Jobs.Config;
using Extractor.Services.Models;
using Extractor.Services.Services;

namespace Extractor.Jobs.Bridge
{
    public class ParserBridge
    {
        private readonly IProductExtractorService _productService;
        private readonly IXmlSchemaValidator _schemaValidator;
        private readonly ProductParserFactory _productParserFactory;

        public ParserBridge(
            IXmlSchemaValidator schemaValidator,
            ProductParserFactory productParserFactory,
            IProductExtractorService productService)
        {
            _productService = productService;
            _schemaValidator = schemaValidator;
            _productParserFactory = productParserFactory;
        }


        [DisplayName("{0}")]
        public Task ExecuteJob(ParserConfig config)
        {
            Console.WriteLine($"START: {config.JobId}");

            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fileType = (FileSrcEnum)Enum.Parse(typeof(FileSrcEnum), config.FileType);

            var filePath = @$"{applicationPath}\{config.FileName}";
            var parser = _productParserFactory.GetParser(fileType);

            try
            {
                if (parser.SourceFileType == FileSrcEnum.XML)
                {
                    var xsdPath = @$"{applicationPath}\{config.SchemaFileName}";
                    _schemaValidator.ValidateSchema(xsdPath, filePath);
                }

                var products = parser.FetchProductsFromFile(filePath);
                _productService.ImportData(products);
            }
            catch (FileNotFoundException fnfex)
            {
                Console.WriteLine($"File not found, ex: {fnfex}");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"ex: {ex}");
            }

            Console.WriteLine($"END: {config.JobId}");
            return Task.CompletedTask;

        }
    }
}
