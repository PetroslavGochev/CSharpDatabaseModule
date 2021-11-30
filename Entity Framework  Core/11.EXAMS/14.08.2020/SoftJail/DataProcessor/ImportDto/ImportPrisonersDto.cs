using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonersDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FullName { get; set; }
        [Required]
        [RegularExpression("^(The\\s)([A-Z]{1}[a-z]*)$")]
        public string Nickname { get; set; }
        [Required]
        [Range(18,65)]
        public int Age { get; set; }
        [Required]
        public string IncarcerationDate { get; set; }
        public string ReleaseDate { get; set; }
        [Range(0.00, 79228162514264337593543950335.00)]
        public decimal? Bail { get; set; }
        public int? CellId { get; set; }
        public ImportMailsDto[] Mails { get; set; }
    }
}
