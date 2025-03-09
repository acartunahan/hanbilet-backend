using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace.Models
{
    public class Sefer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int KalkisSehirId { get; set; }

        [ForeignKey("KalkisSehirId")]
        public Sehir? KalkisSehir { get; set; }

        [Required]
        public int VarisSehirId { get; set; }
        [ForeignKey("VarisSehirId")]
        public Sehir? VarisSehir { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        [Required]
        public TimeSpan Saat { get; set; }

        [Required]
        public decimal Fiyat { get; set; }

        [Required]
        public int FirmaId { get; set; }

        [ForeignKey("FirmaId")]
        public Firma? Firma { get; set; }

        [Required]
        public int OtobusId { get; set; }

        [ForeignKey("OtobusId")]
        public Otobus? Otobus { get; set; }

        public List<SeferDuraklari> Duraklar { get; set; } = new List<SeferDuraklari>();
    }
}
