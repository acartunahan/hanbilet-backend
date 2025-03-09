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

            // Filtreleme işlemleri
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

            // İlişkili tablolardan verileri dahil et
            var seferler = await query
                .Include(s => s.KalkisSehir)  // Kalkış şehri
                .Include(s => s.VarisSehir)   // Varış şehri
                .Include(s => s.Firma)        // Firma
                .Include(s => s.Otobus)       // Otobüs
                .ToListAsync();

            var seferlerDto = seferler.Select(s => new
            {
                s.Id,
                KalkisSehirAdi = s.KalkisSehir?.SehirAdi,  // Kalkış şehri adı
                VarisSehirAdi = s.VarisSehir?.SehirAdi,    // Varış şehri adı
                s.Tarih,
                // Saatin formatlanması
                Saat = s.Saat.ToString(@"hh\:mm"),          // Saat formatı
                s.Fiyat,
                FirmaAdi = s.Firma?.FirmaAdi,             // Firma adı
                OtobusPlaka = s.Otobus?.Plaka             // Otobüs plakası
            }).ToList();

            return Ok(seferlerDto);
        }





        // 2️⃣ Yeni Sefer Ekle
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

        // 3️⃣ Belirli Bir Seferi Getir
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
                KalkisSehir = sefer.KalkisSehir.SehirAdi,   // Kalkış şehri
                VarisSehir = sefer.VarisSehir.SehirAdi,     // Varış şehri
                Tarih = sefer.Tarih.ToString("yyyy-MM-dd"), // Tarih formatı
                Saat = sefer.Saat.ToString(@"hh\:mm"),      // Saat formatı
                sefer.Fiyat,
                FirmaAdi = sefer.Firma.FirmaAdi,            // Firma adı
                OtobusPlaka = sefer.Otobus.Plaka            // Otobüs plakası
            });
        }

        // 4️⃣ Sefer Güncelle
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

        // 5️⃣ Sefer Sil
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
