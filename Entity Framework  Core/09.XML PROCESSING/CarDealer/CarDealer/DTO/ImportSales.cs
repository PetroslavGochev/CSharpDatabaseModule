using System.Xml.Serialization;

namespace CarDealer.DTO
{
    [XmlType("Sale")]
    public class ImportSales
    {
        [XmlElement("carId")]
        public int CarId { get; set; }

        [XmlElement("customerId")]
        public int CustomerId { get; set; }

        [XmlElement("discount")]
        public int Discount { get; set; }
    }
}
