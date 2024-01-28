using Model;
using Extractor.Services.Models;

namespace Extractor.Services.Parsers
{
    public class CsvProductParser : ProductParser, IProductParser
    {
        public CsvProductParser() : base(FileSrcEnum.CSV)
        {
        }

        public override List<ProductModel> FetchProductsFromFile(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
