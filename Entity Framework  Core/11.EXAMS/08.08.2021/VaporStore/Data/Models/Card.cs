using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VaporStore.Data.Models.Enums;

namespace VaporStore.Data.Models
{
    public class Card
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Cvc { get; set; }
        [Required]
        public CardType Type { get; set; }
        [ForeignKey(nameof(User))]
        [Required]
        public int UserId { get; set; }
        [Required]
        public User User { get; set; }
        public List<Purchase> Purchases { get; set; } = new List<Purchase>();
    }
}
