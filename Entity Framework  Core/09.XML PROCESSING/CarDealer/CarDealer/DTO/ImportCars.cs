using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.DTO
{
    [XmlType("Car")]
    public class ImportCars
    {
       
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }
            

        [XmlElement("TraveledDistance")]
        public long TraveledDistance { get; set; }

        [XmlArray("parts")]
        public PartsDto[] Parts { get; set; }
    }

    [XmlType("partId")]
    public class PartsDto
    {
        [XmlAttribute("id")]
        public int PartId { get; set; }
    }
}
