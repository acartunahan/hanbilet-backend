using Microsoft.AspNetCore.Mvc;
using BusTicketAPI.Data;
using BusTicketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BusTicketAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Kullanıcı Kayıt (Register)
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Bu e-posta adresi zaten kullanılıyor." });
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Kullanıcı başarıyla kaydedildi." });
        }

        // Kullanıcı Giriş (Login)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginModel.Email && u.Password == loginModel.Password);
            if (existingUser == null)
            {
                return Unauthorized(new { message = "Geçersiz e-posta veya şifre." });
            }

            return Ok(new { message = "Giriş başarılı.", userId = existingUser.Id });
        }

    }
}
