namespace SoftJail.DataProcessor.ExportDto
{
    public class ExportPrisonersDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CellNumber { get; set; }
        public ExportOfficersDto[] Officers { get; set; }
        public decimal TotalOfficerSalary { get; set; }
    }
}
