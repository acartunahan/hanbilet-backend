using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusTicketAPI.Data;
using YourNamespace.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusTicketAPI.Controllers
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

        // 1️⃣ Tüm Seferleri Getir (Filtreleme Destekli)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetSeferler()
        {
            var seferler = await _context.Seferler
                .Include(s => s.KalkisSehir)
                .Include(s => s.VarisSehir)
                .Select(s => new
                {
                    s.Id,
                    KalkisSehir = s.KalkisSehir.SehirAdi,
                    VarisSehir = s.VarisSehir.SehirAdi,
                    Tarih = s.Tarih.ToString("yyyy-MM-dd"),
                    Saat = s.Saat.ToString(@"hh\:mm"), // ⏰ Saat formatı düzgün döndüğünden emin ol
                    s.Fiyat,
                    FirmaAdi = s.Firma.FirmaAdi,
                    OtobusPlaka = s.Otobus.Plaka
                })
                .ToListAsync();

            return Ok(seferler);
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
                return BadRequest("Saat alanı boş olamaz."); // ⏰ Saatin boş olup olmadığını kontrol et

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
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sefer == null)
                return NotFound();

            return Ok(new
            {
                sefer.Id,
                Tarih = sefer.Tarih.ToString("yyyy-MM-dd"),
                Saat = sefer.Tarih.ToString("HH:mm"),
                sefer.Fiyat,
                KalkisSehir = sefer.KalkisSehir!.SehirAdi,
                VarisSehir = sefer.VarisSehir!.SehirAdi,
                FirmaAdi = sefer.Firma!.FirmaAdi,
                OtobusPlaka = sefer.Otobus!.Plaka
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
