using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YourNamespace.Models;

namespace BusTicketAPI.Models
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
        public int KoltukNumarasi { get; set; } 

        [Required]
        public bool Dolu { get; set; } = false; 

        public int? UserId { get; set; } 

        [ForeignKey("UserId")]
        public User? User { get; set; } 
    }
}
