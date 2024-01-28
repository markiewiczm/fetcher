
using Extractor.Services.Models;
using Extractor.Services.Parsers;

namespace Extractor.Services
{
    public  class ProductParserFactory
    {
        public IProductParser GetParser(FileSrcEnum fileSrcEnum)
        {
            if (fileSrcEnum == FileSrcEnum.XML)
                return new XmlProductParser();

            return new CsvProductParser();
        }

    }

}
