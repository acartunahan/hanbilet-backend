using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace.Models
{
    public class Koltuk
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SeferId { get; set; }

        [ForeignKey("SeferId")]
        public Sefer? Sefer { get; set; }

        [Required]
        public int KoltukNumarasi { get; set; } // 1-40 arasındaki numara

        [Required]
        public bool Dolu { get; set; } = false; // Varsayılan olarak boş
    }
}
