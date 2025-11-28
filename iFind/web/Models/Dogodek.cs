#nullable disable
using System;
using System.ComponentModel.DataAnnotations;
namespace web.Models
{
    public class Dogodek
    {
        public int Id { get; set; }

        [Required] public string Naziv { get; set; }
        public string Opis { get; set; }

        [Required] public DateTime DatumCas { get; set; }//datum+ura


        //FKji
        public int OrganizatorId { get; set; }
        public int KategorijaId { get; set; }
        public virtual Kategorija Kategorija { get; set; }

        public virtual Lokacija Lokacija { get; set; }


        //števec udeležbe
        public virtual ICollection<Udelezba> Udelezbe { get; set; }

        public int SteviloUdelezencev => Udelezbe?.Count ?? 0;
    }
}