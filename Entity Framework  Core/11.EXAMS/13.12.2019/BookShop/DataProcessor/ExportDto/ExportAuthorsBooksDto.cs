using System.Text.Json.Serialization;

namespace BookShop.DataProcessor.ExportDto
{
    public class ExportAuthorsBooksDto
    {
        public string BookName { get; set; }
        public string BookPrice { get; set; }
    }
}
