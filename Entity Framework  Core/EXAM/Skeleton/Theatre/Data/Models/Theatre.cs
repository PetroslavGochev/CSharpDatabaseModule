using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Theatre.Data.Models
{
    public class Theatre
    { 
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public sbyte NumberOfHalls { get; set; }
        [Required]
        [MaxLength(30)]
        public string Director { get; set; }
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
