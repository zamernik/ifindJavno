using web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace web.Data
{
    public class iFindContext : IdentityDbContext<ApplicationUser>
    {
        public iFindContext(DbContextOptions<iFindContext> options) : base(options) { }

        public DbSet<Dogodek> Dogodek { get; set; } = null!;
        public DbSet<Kategorija> Kategorija { get; set; } = null!;
        public DbSet<Lokacija> Lokacija { get; set; } = null!;
        public DbSet<Udelezba> Udelezba { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Imena tabel v slovenščini
            modelBuilder.Entity<Dogodek>().ToTable("Dogodki");
            modelBuilder.Entity<Kategorija>().ToTable("Kategorije");
            modelBuilder.Entity<Lokacija>().ToTable("Lokacije");
            modelBuilder.Entity<Udelezba>().ToTable("Udelezbe");

            // 1:1 Dogodek:Lokacija
            modelBuilder.Entity<Lokacija>()
                .HasOne(l => l.Dogodek)
                .WithOne(d => d.Lokacija)
                .HasForeignKey<Lokacija>(l => l.DogodekId)
                .OnDelete(DeleteBehavior.Cascade);

            // Udelezba – composite key + prava relacija na ApplicationUser
            modelBuilder.Entity<Udelezba>()
                .HasKey(u => new { u.UporabnikId, u.DogodekId });

            modelBuilder.Entity<Udelezba>()
                .HasOne(u => u.Uporabnik)
                .WithMany(u => u.Udelezbe)
                .HasForeignKey(u => u.UporabnikId)
                .OnDelete(DeleteBehavior.NoAction);  //OBVEZNO noaction

            modelBuilder.Entity<Udelezba>()
                .HasOne(u => u.Dogodek)
                .WithMany(d => d.Udelezbe)
                .HasForeignKey(u => u.DogodekId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}