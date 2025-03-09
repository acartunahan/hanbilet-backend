using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusTicketAPI.Data;
using YourNamespace.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusTicketAPI.Controllers
{
    [Route("api/sehirler")]
    [ApiController]
    public class SehirController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SehirController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sehir>>> GetSehirler()
        {
            return await _context.Sehirler.ToListAsync();
        }


        [HttpPost("toplu-ekle")]
        public async Task<IActionResult> PostSehirler(List<Sehir> sehirler)
        {
            if (sehirler == null || sehirler.Count == 0)
                return BadRequest("Eklemek için en az bir şehir belirtmelisiniz.");

            _context.Sehirler.AddRange(sehirler);
            await _context.SaveChangesAsync();

            return Ok(sehirler);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Sehir>> GetSehir(int id)
        {
            var sehir = await _context.Sehirler.FindAsync(id);
            if (sehir == null)
                return NotFound();
            return sehir;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutSehir(int id, Sehir sehir)
        {
            if (id != sehir.Id)
                return BadRequest();

            _context.Entry(sehir).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSehir(int id)
        {
            var sehir = await _context.Sehirler.FindAsync(id);
            if (sehir == null)
                return NotFound();

            _context.Sehirler.Remove(sehir);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
