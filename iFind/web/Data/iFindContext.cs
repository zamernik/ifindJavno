using web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//tukaj definiram katere tabele bom uporabljal

namespace web.Data
{

    //Spremiti sem moral bivše dedovanje iz DbContext v IdentityDbContext<ApplicationUser>
    public class iFindContext : IdentityDbContext<ApplicationUser>
    {
        public iFindContext(DbContextOptions<iFindContext> options) : base(options)
        {
            
        }
        public DbSet<Uporabnik> Uporabnik {get; set;}
        public DbSet<Dogodek> Dogodek {get; set;}
        public DbSet<Kategorija> Kategorija {get; set;}
        public DbSet<Lokacija> Lokacija {get; set;}
        public DbSet<Udelezba> Udelezba {get; set;}
         

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Uporabnik>().ToTable("Uporabniki"); //ker drugači privzeto po angleško- npr. Uporabniks
            modelBuilder.Entity<Dogodek>().ToTable("Dogodki");
            modelBuilder.Entity<Kategorija>().ToTable("Kategorije");
            modelBuilder.Entity<Lokacija>().ToTable("Lokacije");
            modelBuilder.Entity<Udelezba>().ToTable("Udelezbe");

            //da bo ob kreaciji baze naredilo sestavljen PK pri tabeli Udeležba
            modelBuilder.Entity<Udelezba>()
                .HasKey(u => new { u.UporabnikId, u.DogodekId });



            // zaradi problema s CASCADE pri database update
            modelBuilder.Entity<Udelezba>()
                .HasOne(u => u.Uporabnik)
                .WithMany(u => u.Udelezbe)
                .HasForeignKey(u => u.UporabnikId)
                .OnDelete(DeleteBehavior.NoAction);   // brez cascade tukaj

            modelBuilder.Entity<Udelezba>()
                .HasOne(u => u.Dogodek)
                .WithMany(d => d.Udelezbe)
                .HasForeignKey(u => u.DogodekId)
                .OnDelete(DeleteBehavior.Cascade);   // ostane cascade (briše udeležbe ob brisanju dogodka)

        }
    }
}