namespace Extractor.Jobs.Config
{
    public class ParserConfig
    {
        public string JobId { get; set; }
        public string CronExpression { get; set; }
        public bool Enabled { get; set; }

        public string FileName { get; set; }
        public string SchemaFileName { get; set; }
        public string FileType { get; set; }
    }
}
