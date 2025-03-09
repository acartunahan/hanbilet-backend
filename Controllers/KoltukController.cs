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

        [HttpGet("{seferId}")]
        public async Task<ActionResult<IEnumerable<Koltuk>>> GetKoltuklar(int seferId)
        {
            var koltuklar = await _context.Koltuklar
                .Where(k => k.SeferId == seferId)
                .Include(k => k.User)
                .ToListAsync();

            if (!koltuklar.Any())
            {
                for (int i = 1; i <= 40; i++)
                {
                    _context.Koltuklar.Add(new Koltuk
                    {
                        SeferId = seferId,
                        KoltukNumarasi = i,
                        Dolu = false,
                        UserId = null
                    });
                }
                await _context.SaveChangesAsync();

                koltuklar = await _context.Koltuklar
                    .Where(k => k.SeferId == seferId)
                    .Include(k => k.User)
                    .ToListAsync();
            }

            var response = koltuklar.Select(k => new
            {
                k.Id,
                k.SeferId,
                k.KoltukNumarasi,
                k.Dolu,
                k.UserId,
                Cinsiyet = k.User != null ? k.User.Cinsiyet : null
            });

            return Ok(response);
        }

        [HttpPost("satin-al")]
        public async Task<IActionResult> SatinAl([FromBody] Koltuk koltuk)
        {

            var mevcutKoltuk = await _context.Koltuklar
                .FirstOrDefaultAsync(k => k.SeferId == koltuk.SeferId && k.KoltukNumarasi == koltuk.KoltukNumarasi);

            if (mevcutKoltuk == null)
                return NotFound(new { message = "Koltuk bulunamadı!" });

            if (mevcutKoltuk.Dolu)
                return BadRequest(new { message = "Bu koltuk zaten satın alınmış!" });


            var user = await _context.Users.FindAsync(koltuk.UserId);
            if (user == null)
                return BadRequest(new { message = "Kullanıcı bulunamadı!" });

            mevcutKoltuk.Dolu = true;
            mevcutKoltuk.UserId = user.Id;


            var sefer = await _context.Seferler.FirstOrDefaultAsync(s => s.Id == koltuk.SeferId);
            if (sefer == null)
                return BadRequest(new { message = "Geçersiz sefer!" });


            var yeniBilet = new Bilet
            {
                SeferId = koltuk.SeferId,
                UserId = user.Id,
                KoltukNumarasi = koltuk.KoltukNumarasi,
                SatinAlmaTarihi = DateTime.UtcNow,
                Fiyat = sefer.Fiyat
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
                    Cinsiyet = user.Cinsiyet
                }
            });
        }
    }
}
