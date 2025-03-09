using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusTicketAPI.Data;
using YourNamespace.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusTicketAPI.Controllers
{
    [Route("api/otobusler")]
    [ApiController]
    public class OtobusController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OtobusController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetOtobusler()
        {
            var otobusler = await _context.Otobusler
                .Select(o => new
                {
                    o.Id,
                    o.Plaka,
                    o.KoltukSayisi,
                    o.OtobusModel,
                    o.FirmaId
                })
                .ToListAsync();

            return Ok(otobusler);
        }


        // 2️⃣ Yeni otobüs ekle
        [HttpPost]
        public async Task<ActionResult<Otobus>> PostOtobus(Otobus otobus)
        {
            _context.Otobusler.Add(otobus);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOtobusler), new { id = otobus.Id }, otobus);
        }

        [HttpPost("toplu-ekle")]
        public async Task<IActionResult> PostOtobusler(List<Otobus> otobusler)
        {
            if (otobusler == null || otobusler.Count == 0)
                return BadRequest("Eklemek için en az bir otobüs belirtmelisiniz.");

            _context.Otobusler.AddRange(otobusler);
            await _context.SaveChangesAsync();

            return Ok(otobusler);
        }
        [HttpGet("by-firma/{firmaId}")]
        public async Task<ActionResult<IEnumerable<Otobus>>> GetOtobuslerByFirma(int firmaId)
        {
            var otobusler = await _context.Otobusler
                                          .Where(o => o.FirmaId == firmaId)
                                          .ToListAsync();
            return Ok(otobusler);
        }

        // 3️⃣ Belirli bir otobüsü getir
        [HttpGet("{id}")]
        public async Task<ActionResult<Otobus>> GetOtobus(int id)
        {
            var otobus = await _context.Otobusler.FindAsync(id);
            if (otobus == null)
                return NotFound();
            return otobus;
        }

        // 4️⃣ Otobüsü güncelle
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOtobus(int id, Otobus otobus)
        {
            if (id != otobus.Id)
                return BadRequest();

            _context.Entry(otobus).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 5️⃣ Otobüsü sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOtobus(int id)
        {
            var otobus = await _context.Otobusler.FindAsync(id);
            if (otobus == null)
                return NotFound();

            _context.Otobusler.Remove(otobus);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
