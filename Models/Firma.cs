using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YourNamespace.Models
{
    public class Firma
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string FirmaAdi { get; set; }  // Örn: "Kamil Koç", "Metro Turizm"

        // 📌 Firma ile Sefer arasında ilişki (1 firma, birçok sefer yapabilir)
        public List<Sefer> Seferler { get; set; } = new List<Sefer>();

        // 📌 Firma ile Otobüs arasında ilişki (1 firma, birçok otobüse sahip olabilir)
        public List<Otobus> Otobusler { get; set; } = new List<Otobus>(); // 🔥 Eksik olan kısım eklendi!
    }
}
