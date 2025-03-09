using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;
using BusTicketAPI.Data;

namespace YourNamespace.Controllers
{
    [Route("api/seferler")]
    [ApiController]
    public class SeferController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SeferController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sefer>>> GetSeferler(
            [FromQuery] int? kalkisSehirId,
            [FromQuery] int? varisSehirId,
            [FromQuery] DateTime? tarih)
        {
            var query = _context.Seferler.AsQueryable();


            if (kalkisSehirId.HasValue)
                query = query.Where(s => s.KalkisSehirId == kalkisSehirId.Value);

            if (varisSehirId.HasValue)
                query = query.Where(s => s.VarisSehirId == varisSehirId.Value);

            if (tarih.HasValue)
            {
                DateTime startOfDay = tarih.Value.Date;
                DateTime endOfDay = tarih.Value.Date.AddDays(1);

                query = query.Where(s => s.Tarih >= startOfDay && s.Tarih < endOfDay);
            }


            var seferler = await query
                .Include(s => s.KalkisSehir)
                .Include(s => s.VarisSehir)
                .Include(s => s.Firma)
                .Include(s => s.Otobus)
                .ToListAsync();

            var seferlerDto = seferler.Select(s => new
            {
                s.Id,
                KalkisSehirAdi = s.KalkisSehir?.SehirAdi,
                VarisSehirAdi = s.VarisSehir?.SehirAdi,
                s.Tarih,
                // Saatin formatlanması
                Saat = s.Saat.ToString(@"hh\:mm"),
                s.Fiyat,
                FirmaAdi = s.Firma?.FirmaAdi,
                OtobusPlaka = s.Otobus?.Plaka
            }).ToList();

            return Ok(seferlerDto);
        }





        [HttpPost]
        public async Task<ActionResult<Sefer>> PostSefer([FromBody] Sefer sefer)
        {
            if (sefer == null)
                return BadRequest("Gönderilen veri boş olamaz.");

            if (sefer.KalkisSehirId <= 0 || sefer.VarisSehirId <= 0 || sefer.FirmaId <= 0 || sefer.OtobusId <= 0)
                return BadRequest("Geçersiz ID değerleri. Lütfen tüm alanları doğru seçin.");

            if (sefer.Tarih == default)
                return BadRequest("Tarih alanı boş olamaz.");

            if (sefer.Saat == default)
                return BadRequest("Saat alanı boş olamaz.");

            _context.Seferler.Add(sefer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSeferler), new { id = sefer.Id }, sefer);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetSefer(int id)
        {
            var sefer = await _context.Seferler
                .Include(s => s.KalkisSehir)
                .Include(s => s.VarisSehir)
                .Include(s => s.Firma)
                .Include(s => s.Otobus)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sefer == null)
                return NotFound();

            return Ok(new
            {
                sefer.Id,
                KalkisSehir = sefer.KalkisSehir.SehirAdi,
                VarisSehir = sefer.VarisSehir.SehirAdi,
                Tarih = sefer.Tarih.ToString("yyyy-MM-dd"),
                Saat = sefer.Saat.ToString(@"hh\:mm"),
                sefer.Fiyat,
                FirmaAdi = sefer.Firma.FirmaAdi,
                OtobusPlaka = sefer.Otobus.Plaka
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSefer(int id, Sefer sefer)
        {
            if (id != sefer.Id)
                return BadRequest();

            _context.Entry(sefer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Seferler.Any(s => s.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSefer(int id)
        {
            var sefer = await _context.Seferler.FindAsync(id);
            if (sefer == null)
                return NotFound();

            _context.Seferler.Remove(sefer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
