#nullable disable
using System;
using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public enum Spol
    {
        M,
        Z
    }

    public class Uporabnik
    {
        public int Id { get; set; }

        [Required]
        public string Ime { get; set; }

        [Required]
        public string Priimek { get; set; }

        public Spol? Spol { get; set; }

        [Required]
        [EmailAddress]
        public string Mail { get; set; }

        [Required]
        public string Geslo { get; set; } //verjetno mora biti hashirano

        public DateTime DatumRegistracije { get; set; } = DateTime.Now;

        //vloge
        public bool JeAdministrator { get; set; } = false;
        public bool JeOrganizator { get; set; } = false;

        //povezave
        public virtual ICollection<Dogodek> OrganiziraniDogodki { get; set; }
        //public virtual ICollection<Udelezba> Udelezbe { get; set; } začasno
    }
}
