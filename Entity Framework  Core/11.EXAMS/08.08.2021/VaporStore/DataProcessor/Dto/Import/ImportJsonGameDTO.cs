using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class ImportJsonGameDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0.00, 79228162514264337593543950335.00)]
        public decimal Price { get; set; }
        [Required]
        public string ReleaseDate { get; set; }
        [Required]
        public string Developer { get; set; }
        [Required]
        public string Genre { get; set; }
        public string[] Tags { get; set; }
    }
}
