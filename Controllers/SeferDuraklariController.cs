using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusTicketAPI.Data;
using YourNamespace.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusTicketAPI.Controllers
{
    [Route("api/sefer-duraklari")]
    [ApiController]
    public class SeferDuraklariController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SeferDuraklariController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Belirli bir seferin tüm duraklarını getir
        [HttpGet("{seferId}")]
        public async Task<ActionResult<IEnumerable<SeferDuraklari>>> GetDuraklar(int seferId)
        {
            var duraklar = await _context.SeferDuraklari
                .Where(d => d.SeferId == seferId)
                .OrderBy(d => d.Sira)
                .ToListAsync();

            if (duraklar == null || duraklar.Count == 0)
                return NotFound("Bu sefere ait durak bulunamadı.");

            return duraklar;
        }

        // 2️⃣ Yeni bir durak ekle
        [HttpPost]
        public async Task<ActionResult<SeferDuraklari>> PostDurak(SeferDuraklari durak)
        {
            _context.SeferDuraklari.Add(durak);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDuraklar), new { seferId = durak.SeferId }, durak);
        }

        // 3️⃣ Belirli bir durağı getir
        [HttpGet("durak/{id}")]
        public async Task<ActionResult<SeferDuraklari>> GetDurak(int id)
        {
            var durak = await _context.SeferDuraklari.FindAsync(id);
            if (durak == null)
                return NotFound();
            return durak;
        }

        // 4️⃣ Durağı Güncelle
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDurak(int id, SeferDuraklari durak)
        {
            if (id != durak.Id)
                return BadRequest();

            _context.Entry(durak).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.SeferDuraklari.Any(d => d.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // 5️⃣ Durağı Sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDurak(int id)
        {
            var durak = await _context.SeferDuraklari.FindAsync(id);
            if (durak == null)
                return NotFound();

            _context.SeferDuraklari.Remove(durak);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
