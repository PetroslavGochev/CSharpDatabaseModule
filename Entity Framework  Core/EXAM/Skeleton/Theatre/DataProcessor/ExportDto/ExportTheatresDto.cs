namespace Theatre.DataProcessor.ExportDto
{
    public class ExportTheatresDto
    {
        public string Name { get; set; }
        public sbyte Halls { get; set; }
        public decimal TotalIncome { get; set; }
        public ExportTicketsDto[] Tickets { get; set; }
    }
}
