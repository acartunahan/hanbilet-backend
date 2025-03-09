using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusTicketAPI.Data;
using YourNamespace.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BusTicketAPI.Controllers
{
    [Route("api/biletler")]
    [ApiController]
    public class BiletController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BiletController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Bilet>> SatınAl([FromBody] Bilet bilet)
        {
            if (bilet == null || bilet.SeferId <= 0 || bilet.UserId <= 0 || bilet.KoltukNumarasi <= 0)
                return BadRequest("Eksik veya hatalı veri!");


            bool koltukDolu = await _context.Biletler
                .AnyAsync(b => b.SeferId == bilet.SeferId && b.KoltukNumarasi == bilet.KoltukNumarasi);

            if (koltukDolu)
                return BadRequest("Bu koltuk zaten satın alınmış!");

            var sefer = await _context.Seferler.FirstOrDefaultAsync(s => s.Id == bilet.SeferId);
            if (sefer == null)
                return BadRequest("Geçersiz sefer!");

            bilet.Fiyat = sefer.Fiyat;
            bilet.SatinAlmaTarihi = DateTime.UtcNow;

            _context.Biletler.Add(bilet);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Bilet başarıyla satın alındı!", bilet });
        }


        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetBiletler(int userId)
        {
            var biletler = await _context.Biletler
                .Where(b => b.UserId == userId)
                .Include(b => b.Sefer)
                    .ThenInclude(s => s.KalkisSehir)
                .Include(b => b.Sefer)
                    .ThenInclude(s => s.VarisSehir)
                .Include(b => b.Sefer)
                    .ThenInclude(s => s.Firma)
                .Include(b => b.Sefer)
                    .ThenInclude(s => s.Otobus)
                .Select(b => new
                {
                    b.SeferId,
                    b.KoltukNumarasi,
                    Fiyat = b.Sefer.Fiyat,
                    SatinAlmaTarihi = b.SatinAlmaTarihi,
                    SeferBilgisi = new
                    {
                        KalkisSehir = b.Sefer.KalkisSehir != null ? b.Sefer.KalkisSehir.SehirAdi : "**HATA: Kalkış Şehri Yok!**",
                        VarisSehir = b.Sefer.VarisSehir != null ? b.Sefer.VarisSehir.SehirAdi : "**HATA: Varış Şehri Yok!**",
                        Tarih = b.Sefer.Tarih != null ? b.Sefer.Tarih.ToString("yyyy-MM-dd") : "**HATA: Tarih Yok!**",
                        Saat = b.Sefer.Saat != null ? b.Sefer.Saat.ToString(@"hh\:mm") : "**HATA: Saat Yok!**",
                        FirmaAdi = b.Sefer.Firma != null ? b.Sefer.Firma.FirmaAdi : "**HATA: Firma Yok!**",
                        OtobusPlaka = b.Sefer.Otobus != null ? b.Sefer.Otobus.Plaka : "**HATA: Otobüs Yok!**"
                    }
                })
                .ToListAsync();

            return Ok(biletler);
        }


    }
}
