namespace VaporStore.DataProcessor.Dto.Export
{
    public class ExportGenresDto
    {
        public int Id { get; set; }
        public string Genre { get; set; }
        public ExportGamesDto[] Games { get; set; }
        public int TotalPlayers { get; set; }
    }
}
