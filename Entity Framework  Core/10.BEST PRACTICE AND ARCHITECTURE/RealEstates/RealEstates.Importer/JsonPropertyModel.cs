namespace RealEstates.Importer
{
    public class JsonPropertyModel
    {
        public int Size { get; set; }

        public byte Floor { get; set; }

        public byte TotalFloors { get; set; }

        public string District { get; set; }

        public int Year { get; set; }

        public string Type { get; set; }

        public string BuildingType { get; set; }

        public decimal Price { get; set; }
    }
}
