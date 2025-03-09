using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace.Models
{
    public class Otobus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Plaka { get; set; }

        [Required]
        public int KoltukSayisi { get; set; }

        [Required]
        public required string OtobusModel { get; set; }


        [Required]
        public int FirmaId { get; set; }

        [ForeignKey("FirmaId")]
        public Firma? Firma { get; set; }

        public List<Sefer> Seferler { get; set; } = new List<Sefer>();
    }
}
