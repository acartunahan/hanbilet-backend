using System.ComponentModel.DataAnnotations;

namespace YourNamespace.Models
{
    public class Sehir
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string SehirAdi { get; set; }  // Örn: Ankara, İstanbul, İzmir
    }
}
