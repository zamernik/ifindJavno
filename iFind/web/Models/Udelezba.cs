using System;
#nullable disable
namespace web.Models
{
    public class Udelezba
    {
        // Identity uporabnik → string Id
        public string UporabnikId { get; set; } = null!;
        public ApplicationUser Uporabnik { get; set; } = null!;

        public int DogodekId { get; set; }
        public Dogodek Dogodek { get; set; } = null!;

        public DateTime DatumPrijave { get; set; } = DateTime.UtcNow;
    }
}

