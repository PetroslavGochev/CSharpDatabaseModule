using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Theatre.Data.Models.Enums;

namespace Theatre.Data.Models
{
    public class Play
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }
        [Required]
        public float Rating { get; set; }
        [Required]
        public Genre Genre { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Screenwriter { get; set; }
        public List<Cast> Casts { get; set; } = new List<Cast>();
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();

    }
}
