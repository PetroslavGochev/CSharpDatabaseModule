using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class ImportOfficersDto
    {
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        [XmlElement("Name")]
        public string Name { get; set; }
        [Required]
        [Range(0.00, 79228162514264337593543950335.00)]
        [XmlElement("Money")]
        public decimal Money { get; set; }
        [Required]
        [XmlElement("Position")]
        public string Position { get; set; }
        [Required]
        [XmlElement("Weapon")]
        public string Weapon { get; set; }
        [Required]
        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }
        [XmlArray("Prisoners")]
        public PrisonersDto[] Prisoners { get; set; }
    }
}
