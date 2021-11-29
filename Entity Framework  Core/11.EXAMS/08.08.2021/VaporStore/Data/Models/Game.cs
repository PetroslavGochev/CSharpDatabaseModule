using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaporStore.Data.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [ForeignKey(nameof(Developer))]
        [Required]
        public int DeveloperId { get; set; }
        [Required]
        public Developer Developer { get; set; }
        [ForeignKey(nameof(Genre))]
        [Required]
        public int GenreId { get; set; }
        [Required]
        public Genre Genre { get; set; }
        public List<Purchase> Purchases { get; set; } = new List<Purchase>();
        public List<GameTag> GameTags { get; set; } = new List<GameTag>();
    }
}
