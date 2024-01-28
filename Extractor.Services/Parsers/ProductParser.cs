using Model;
using Extractor.Services.Models;

namespace Extractor.Services
{

    public interface IProductParser
    {
        FileSrcEnum SourceFileType { get; set; }
        List<ProductModel> FetchProductsFromFile(string filePath);
    }

    public abstract class ProductParser : IProductParser
    {
        public ProductParser(FileSrcEnum sourceFileType)
        {
            SourceFileType = sourceFileType;
        }

        public FileSrcEnum SourceFileType { get; set; }
        public abstract List<ProductModel> FetchProductsFromFile(string filePath);
        
    }




}
