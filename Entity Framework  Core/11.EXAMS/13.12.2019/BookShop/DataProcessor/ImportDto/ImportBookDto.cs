using BookShop.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ImportDto
{
    [XmlType("Book")]
    public class ImportBookDto
    {
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        [Range(1,3)]
        public int Genre { get; set; }
        [Range(0.00, 79228162514264337593543950335.00)]
        public decimal Price { get; set; }
        [Range(50,5000)]
        public int Pages { get; set; }
        [Required]
        public string PublishedOn { get; set; }
    }
}
