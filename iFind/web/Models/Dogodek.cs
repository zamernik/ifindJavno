using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  
using web.Models;                           

namespace web.Models
{
    public class Dogodek
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv je obvezen")]
        public string Naziv { get; set; } = null!;

        public string? Opis { get; set; }

        [Required(ErrorMessage = "Datum in čas sta obvezna")]
        public DateTime DatumCas { get; set; }

        // OrganizatorId ni required – ga nastavimo v controllerju
        public string OrganizatorId { get; set; } = null!;
        public ApplicationUser Organizator { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Izberite kategorijo")]
        public int KategorijaId { get; set; }
        
        public Kategorija Kategorija { get; set; } = null!;

        public Lokacija? Lokacija { get; set; }

        public ICollection<Udelezba> Udelezbe { get; set; } = new List<Udelezba>();
    }
}