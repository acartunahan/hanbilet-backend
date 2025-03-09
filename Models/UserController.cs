using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusTicketAPI.Data;
using BusTicketAPI.Models;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    // 📌 Kullanıcı Bilgilerini Getir
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound(new { message = "Kullanıcı bulunamadı." });

        return Ok(new
        {
            name = user.Name,
            surname = user.Surname,
            email = user.Email,
            phone = user.Phone,
            birthDate = user.BirthDate.ToString("yyyy-MM-dd"), // ✅ `Age` yerine doğum tarihi döndürülüyor
            role = user.Role // ✅ Kullanıcının rolü de döndürülüyor
        });
    }

    // 📌 Kullanıcı Kayıt Ol
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
        {
            return BadRequest(new { message = "Bu e-posta adresi zaten kullanılıyor." });
        }

        var today = DateTime.Today;
        var age = today.Year - user.BirthDate.Year;
        if (user.BirthDate.Date > today.AddYears(-age)) age--; // ✅ Yaşı doğum tarihinden hesapla

        if (age < 18)
        {
            return BadRequest(new { message = "18 yaşından küçükler kayıt olamaz." });
        }

        if (user.Password != user.ConfirmPassword)
        {
            return BadRequest(new { message = "Şifreler uyuşmuyor." });
        }

        user.Password = HashPassword(user.Password);
        user.ConfirmPassword = user.Password;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Kayıt başarılı! Giriş yapabilirsiniz.", role = user.Role });
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginUser)
    {
        if (string.IsNullOrEmpty(loginUser.Email) || string.IsNullOrEmpty(loginUser.Password))
        {
            return BadRequest(new { message = "E-posta ve şifre gereklidir!" });
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUser.Email);

        if (user == null)
        {
            return Unauthorized(new { message = "E-posta veya şifre hatalı!" });
        }

        // Kullanıcının gönderdiği şifreyi tekrar hash'le
        string hashedPassword = HashPassword(loginUser.Password);

        // Hash'lenmiş şifreyi veritabanındaki ile karşılaştır
        if (user.Password != hashedPassword)
        {
            return Unauthorized(new { message = "E-posta veya şifre hatalı!" });
        }

        return Ok(new { message = "Giriş başarılı!", userId = user.Id, name = user.Name, role = user.Role, cinsiyet = user.Cinsiyet });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound(new { message = "Kullanıcı bulunamadı." });
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Kullanıcı başarıyla silindi." });
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
