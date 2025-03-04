using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusTicketAPI.Models;

namespace YourNamespace.Models
{
    public class Bilet
    {
        [Key]
        public int Id { get; set; } // 📌 Bilet ID (Primary Key)

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; } // 📌 Bileti alan kullanıcı
        public User? User { get; set; }

        [Required]
        [ForeignKey("Sefer")]
        public int SeferId { get; set; } // 📌 Hangi sefere ait?
        public Sefer? Sefer { get; set; }

        [Required]
        public int KoltukNumarasi { get; set; } // 📌 Hangi koltuk numarası?

        [Required]
        public decimal Fiyat { get; set; } // 📌 Seferin fiyatı eklenmeli!

        [Required]
        public DateTime SatinAlmaTarihi { get; set; } = DateTime.UtcNow; // 📌 Satın alma zamanı
    }
}
