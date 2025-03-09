using System;
using System.ComponentModel.DataAnnotations;

namespace BusTicketAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Telefon numarası 10 haneli olmalıdır.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Cinsiyet alanı zorunludur.")]
        public string Cinsiyet { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrar zorunludur.")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; } = "User";


    }
}
