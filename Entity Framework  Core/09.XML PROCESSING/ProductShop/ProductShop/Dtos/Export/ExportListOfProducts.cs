using ProductShop.Models;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("SoldProducts")]
    public class ExportListOfProducts
    {
        public ExportListOfProducts()
        {
            this.Products = new List<Product>();
        }
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlElement("products")]
        public ICollection<Product> Products { get; set; }
    }

}
