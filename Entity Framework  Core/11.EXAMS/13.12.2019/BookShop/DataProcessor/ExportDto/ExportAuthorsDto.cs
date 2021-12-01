namespace BookShop.DataProcessor.ExportDto
{
    public class ExportAuthorsDto
    {
        public string AuthorName { get; set; }
        public ExportAuthorsBooksDto[] Books { get; set; }
    }
}
