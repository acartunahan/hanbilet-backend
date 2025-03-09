using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YourNamespace.Models
{
    public class Firma
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string FirmaAdi { get; set; }


        public List<Sefer> Seferler { get; set; } = new List<Sefer>();


        public List<Otobus> Otobusler { get; set; } = new List<Otobus>();
    }
}
