using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusTicketAPI.Models;

namespace YourNamespace.Models
{
    public class Bilet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        [ForeignKey("Sefer")]
        public int SeferId { get; set; }
        public Sefer? Sefer { get; set; }

        [Required]
        public int KoltukNumarasi { get; set; }

        [Required]
        public decimal Fiyat { get; set; }

        [Required]
        public DateTime SatinAlmaTarihi { get; set; } = DateTime.UtcNow;
    }
}
