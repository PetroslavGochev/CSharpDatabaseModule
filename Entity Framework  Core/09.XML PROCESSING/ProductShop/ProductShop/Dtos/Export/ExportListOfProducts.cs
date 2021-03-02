using ProductShop.Models;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("SoldProducts")]
    public class ExportListOfProducts
    {

        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public ExportProductNameAndPriceDto[] Products { get; set; }
    }

}
