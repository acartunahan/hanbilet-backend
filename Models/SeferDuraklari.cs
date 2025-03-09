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
        public required Sefer Sefer { get; set; }

        [Required]
        public required string Durak { get; set; }

        [Required]
        public int Sira { get; set; }

        [Required]
        public decimal Fiyat { get; set; }
    }
}