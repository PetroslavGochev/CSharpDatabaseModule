using System.ComponentModel.DataAnnotations;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class ImportJsonUserDTO
    {
        [Required]
        [RegularExpression("^([A-Z]{1}[a-z]+)\\s([A-Z]{1}[a-z]+)$")]
        public string FullName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Range(3,103)]
        public int Age { get; set; }
        public ImportJsonCardsDTO[] Cards { get; set; }
    }
}
