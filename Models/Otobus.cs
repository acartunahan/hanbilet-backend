using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace.Models
{
    public class Otobus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Plaka { get; set; } // Örn: 34 ABC 123

        [Required]
        public int KoltukSayisi { get; set; } // Örn: 40 koltuk

        [Required]
        public required string OtobusModel { get; set; } // Örn: Travego, Setra

        // 📌 Otobüs bir firmaya ait olmalı
        [Required]
        public int FirmaId { get; set; } // 🔥 Firma ilişkisi

        [ForeignKey("FirmaId")]
        public Firma? Firma { get; set; } // Firma ile ilişki

        // 📌 Otobüs ile Sefer arasında ilişki (1 otobüs, birçok sefer yapabilir)
        public List<Sefer> Seferler { get; set; } = new List<Sefer>();
    }
}
