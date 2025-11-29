using web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using web.Data;

namespace web.Data
{
    public class iFindContext : IdentityDbContext<ApplicationUser>
    {
        public iFindContext(DbContextOptions<iFindContext> options) : base(options)
        {
        }

        public DbSet<Uporabnik> Uporabnik { get; set; }
        public DbSet<Dogodek> Dogodek { get; set; }
        public DbSet<Kategorija> Kategorija { get; set; }
        public DbSet<Lokacija> Lokacija { get; set; }
        ///public DbSet<Udelezba> Udelezba { get; set; } začasno

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // tabela imena
            modelBuilder.Entity<Uporabnik>().ToTable("Uporabniki");
            modelBuilder.Entity<Dogodek>().ToTable("Dogodki");
            modelBuilder.Entity<Kategorija>().ToTable("Kategorije");
            modelBuilder.Entity<Lokacija>().ToTable("Lokacije");
            //modelBuilder.Entity<Udelezba>().ToTable("Udelezbe"); začasno

            // ===========================
            // UDELEZBA: Composite key
            // ===========================
            /*modelBuilder.Entity<Udelezba>()
                .HasKey(u => new { u.UporabnikId, u.DogodekId });začasno!*/ 

            // ===========================
            // RELACIJE UDELEZBA ↔ UPORABNIK (1:N)
            // ===========================
            /*
            modelBuilder.Entity<Udelezba>()
                .HasOne(u => u.Uporabnik)
                .WithMany(u => u.Udelezbe)
                .HasForeignKey(u => u.UporabnikId)
                .OnDelete(DeleteBehavior.NoAction); začasno*/ 

            // ===========================
            // RELACIJE UDELEZBA ↔ DOGODEK (1:N)
            // ===========================
            /*
            modelBuilder.Entity<Udelezba>()
                .HasOne(u => u.Dogodek)
                .WithMany(d => d.Udelezbe)
                .HasForeignKey(u => u.DogodekId)
                .OnDelete(DeleteBehavior.Cascade);začasno*/

            // ===========================
            // RELACIJA DOGODEK ↔ LOKACIJA (1:1)
            // ===========================
            modelBuilder.Entity<Lokacija>()
                .HasOne(l => l.Dogodek)
                .WithOne(d => d.Lokacija)
                .HasForeignKey<Lokacija>(l => l.DogodekId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
