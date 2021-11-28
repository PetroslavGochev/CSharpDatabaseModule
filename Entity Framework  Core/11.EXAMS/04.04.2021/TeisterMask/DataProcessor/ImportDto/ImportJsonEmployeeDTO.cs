using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class ImportJsonEmployeeDTO
    {
        [MaxLength(40)]
        [MinLength(3)]
        [Required]
        [RegularExpression(@"^[A-Za-z0-9]{3,}$")]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(\d{3})\-(\d{3})\-(\d{4})$")]
        public string Phone { get; set; }
        public int[] Tasks { get; set; }
    }
}
