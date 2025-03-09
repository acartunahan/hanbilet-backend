using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusTicketAPI.Data;
using BusTicketAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourNamespace.Models;

namespace BusTicketAPI.Controllers
{
    [Route("api/koltuklar")]
    [ApiController]
    public class KoltukController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KoltukController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📌 1️⃣ Belirli bir sefere ait koltukları getir
        [HttpGet("{seferId}")]
        public async Task<ActionResult<IEnumerable<Koltuk>>> GetKoltuklar(int seferId)
        {
            var koltuklar = await _context.Koltuklar
                .Where(k => k.SeferId == seferId)
                .Include(k => k.User) // Kullanıcı bilgisi dahil edildi
                .ToListAsync();

            // Eğer bu sefere ait koltuklar veritabanında yoksa, 40 koltuk oluştur.
            if (!koltuklar.Any())
            {
                for (int i = 1; i <= 40; i++)
                {
                    _context.Koltuklar.Add(new Koltuk
                    {
                        SeferId = seferId,
                        KoltukNumarasi = i,
                        Dolu = false, // Başlangıçta boş
                        UserId = null // Boş koltuklarda UserId null olmalı
                    });
                }
                await _context.SaveChangesAsync();

                // Yeni eklenen koltukları çek
                koltuklar = await _context.Koltuklar
                    .Where(k => k.SeferId == seferId)
                    .Include(k => k.User)
                    .ToListAsync();
            }

            // Kullanıcı bilgisi ile birlikte dönüyoruz.
            var response = koltuklar.Select(k => new
            {
                k.Id,
                k.SeferId,
                k.KoltukNumarasi,
                k.Dolu,
                k.UserId,
                Cinsiyet = k.User != null ? k.User.Cinsiyet : null // Kullanıcı varsa cinsiyetini getir
            });

            return Ok(response);
        }

        // 📌 2️⃣ Koltuk satın alma
        [HttpPost("satin-al")]
        public async Task<IActionResult> SatinAl([FromBody] Koltuk koltuk)
        {
            // Koltuk mevcut mu?
            var mevcutKoltuk = await _context.Koltuklar
                .FirstOrDefaultAsync(k => k.SeferId == koltuk.SeferId && k.KoltukNumarasi == koltuk.KoltukNumarasi);

            if (mevcutKoltuk == null)
                return NotFound(new { message = "Koltuk bulunamadı!" });

            if (mevcutKoltuk.Dolu)
                return BadRequest(new { message = "Bu koltuk zaten satın alınmış!" });

            // Kullanıcıyı getir
            var user = await _context.Users.FindAsync(koltuk.UserId);
            if (user == null)
                return BadRequest(new { message = "Kullanıcı bulunamadı!" });

            // 📌 Koltuğu "Dolu" yap ve Kullanıcı ID'sini ata
            mevcutKoltuk.Dolu = true;
            mevcutKoltuk.UserId = user.Id; // Kullanıcı ID'si eklendi

            // 📌 Sefer bilgisini al
            var sefer = await _context.Seferler.FirstOrDefaultAsync(s => s.Id == koltuk.SeferId);
            if (sefer == null)
                return BadRequest(new { message = "Geçersiz sefer!" });

            // 📌 Kullanıcının bilet kaydını oluştur
            var yeniBilet = new Bilet
            {
                SeferId = koltuk.SeferId,
                UserId = user.Id,
                KoltukNumarasi = koltuk.KoltukNumarasi,
                SatinAlmaTarihi = DateTime.UtcNow,
                Fiyat = sefer.Fiyat // **Bilet fiyatı sefer modelinden alınıyor**
            };

            _context.Biletler.Add(yeniBilet);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Bilet başarıyla satın alındı!",
                biletId = yeniBilet.Id,
                koltuk = new
                {
                    mevcutKoltuk.Id,
                    mevcutKoltuk.SeferId,
                    mevcutKoltuk.KoltukNumarasi,
                    mevcutKoltuk.Dolu,
                    mevcutKoltuk.UserId,
                    Cinsiyet = user.Cinsiyet // Kullanıcının cinsiyet bilgisi
                }
            });
        }
    }
}
