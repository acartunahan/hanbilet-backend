using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace.Models
{
    public class SeferDuraklari
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Sefer")]
        public int SeferId { get; set; }
        public required Sefer Sefer { get; set; }  // İlişkilendirme

        [Required]
        public required string Durak { get; set; }  // Durak ismi (Ankara, Eskişehir, vs.)

        [Required]
        public int Sira { get; set; }  // Otobüsün duraktaki sırası

        [Required]
        public decimal Fiyat { get; set; } // Bu durağa kadar olan fiyat
    }
}