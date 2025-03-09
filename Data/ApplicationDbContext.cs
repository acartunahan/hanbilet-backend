using Microsoft.EntityFrameworkCore;
using BusTicketAPI.Models;
using YourNamespace.Models;

namespace BusTicketAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Sehir> Sehirler { get; set; }  // 📌 Şehir tablosu eklendi
        public DbSet<Sefer> Seferler { get; set; }
        public DbSet<SeferDuraklari> SeferDuraklari { get; set; }
        public DbSet<Firma> Firmalar { get; set; }
        public DbSet<Otobus> Otobusler { get; set; }
        public DbSet<Bilet> Biletler { get; set; }

        public DbSet<Koltuk> Koltuklar { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Sefer>()
                .HasOne(s => s.KalkisSehir)
                .WithMany()
                .HasForeignKey(s => s.KalkisSehirId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sefer>()
                .HasOne(s => s.VarisSehir)
                .WithMany()
                .HasForeignKey(s => s.VarisSehirId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sefer>()
                .HasOne(s => s.Firma)
                .WithMany(f => f.Seferler)
                .HasForeignKey(s => s.FirmaId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Otobus>()
                .HasOne(o => o.Firma)
                .WithMany(f => f.Otobusler)
                .HasForeignKey(o => o.FirmaId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Sefer>()
                .HasOne(s => s.Otobus)
                .WithMany(o => o.Seferler)
                .HasForeignKey(s => s.OtobusId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }


    }
}
