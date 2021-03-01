using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.DTO
{
    [XmlType("Car")]
    public class ImportCars
    {
        [XmlElement("Make")]
        public string Make { get; set; }

        [XmlElement("Model")]
        public string Model { get; set; }
            

        [XmlElement("TraveledDistance")]
        public long TraveledDistance { get; set; }

        [XmlElement("parts")]
        public ICollection<> Parts { get; set; }
    }
}
