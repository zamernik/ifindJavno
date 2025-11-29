using System;

namespace web.Models
{
    public class Dogodek
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public DateTime DatumCas { get; set; }

        public string OrganizatorId { get; set; }

        public int KategorijaId { get; set; }
        public Kategorija Kategorija { get; set; }

        public Lokacija Lokacija { get; set; }
    }
}
