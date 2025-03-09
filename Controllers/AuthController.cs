using Microsoft.AspNetCore.Mvc;
using BusTicketAPI.Data;
using BusTicketAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        // 📌 Kullanıcı Kayıt (Register)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Bu e-posta adresi zaten kullanılıyor." });
            }

            // Şifreyi hash'le
            user.Password = HashPassword(user.Password);
            user.ConfirmPassword = user.Password;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Kullanıcı başarıyla kaydedildi." });
        }

        // 📌 Kullanıcı Giriş (Login)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Validasyon hatalarını döndür
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

            if (user == null)
            {
                return Unauthorized(new { message = "E-posta veya şifre hatalı!" });
            }

            // Kullanıcının gönderdiği şifreyi tekrar hash'le ve karşılaştır
            string hashedPassword = HashPassword(loginRequest.Password);

            if (user.Password != hashedPassword)
            {
                return Unauthorized(new { message = "E-posta veya şifre hatalı!" });
            }

            // ✅ Kullanıcının `cinsiyet` bilgisini ekledik
            return Ok(new
            {
                message = "Giriş başarılı!",
                userId = user.Id,
                name = user.Name,
                role = user.Role,
                cinsiyet = user.Cinsiyet // ✅ Cinsiyet bilgisi artık API yanıtında!
            });
        }

        // 📌 Şifreyi Hashleme Metodu (Güvenlik için)
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
