using System.Xml.Serialization;

namespace ProductShop.Dtos.Import
{
    [XmlType("CategoryProduct")]
    public class ImportCategoryProductsDto
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
    }
}
