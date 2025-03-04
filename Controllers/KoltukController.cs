using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusTicketAPI.Data;
using YourNamespace.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                .ToListAsync();

            if (!koltuklar.Any())
            {
                // Eğer koltuklar yoksa, 40 tane oluştur
                for (int i = 1; i <= 40; i++)
                {
                    _context.Koltuklar.Add(new Koltuk
                    {
                        SeferId = seferId,
                        KoltukNumarasi = i,
                        Dolu = false
                    });
                }
                await _context.SaveChangesAsync();

                koltuklar = await _context.Koltuklar
                    .Where(k => k.SeferId == seferId)
                    .ToListAsync();
            }

            return Ok(koltuklar);
        }

        // 📌 2️⃣ Koltuk satın alma
        [HttpPost("satin-al")]
        public async Task<IActionResult> SatinAl([FromBody] Koltuk koltuk)
        {
            var mevcutKoltuk = await _context.Koltuklar
                .FirstOrDefaultAsync(k => k.SeferId == koltuk.SeferId && k.KoltukNumarasi == koltuk.KoltukNumarasi);

            if (mevcutKoltuk == null)
                return NotFound(new { message = "Koltuk bulunamadı!" });

            if (mevcutKoltuk.Dolu)
                return BadRequest(new { message = "Bu koltuk zaten satın alınmış!" });

            mevcutKoltuk.Dolu = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Bilet başarıyla satın alındı!" });
        }

    }
}
