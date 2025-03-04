using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusTicketAPI.Data;
using YourNamespace.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusTicketAPI.Controllers
{
    [Route("api/firmalar")]
    [ApiController]
    public class FirmaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FirmaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔥 1️⃣ Tüm firmaları getir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Firma>>> GetFirmalar()
        {
            return await _context.Firmalar.ToListAsync();
        }

        // 🔥 2️⃣ Tek bir firma ekle
        [HttpPost]
        public async Task<ActionResult<Firma>> PostFirma(Firma firma)
        {
            _context.Firmalar.Add(firma);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFirmalar), new { id = firma.Id }, firma);
        }

        // 🔥 3️⃣ Toplu firma ekleme
        [HttpPost("toplu-ekle")]
        public async Task<IActionResult> PostFirmalar(List<Firma> firmalar)
        {
            if (firmalar == null || firmalar.Count == 0)
                return BadRequest("Eklemek için en az bir firma belirtmelisiniz.");

            _context.Firmalar.AddRange(firmalar);
            await _context.SaveChangesAsync();
            return Ok(firmalar);
        }
    }
}
