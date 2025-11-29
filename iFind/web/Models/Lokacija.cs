using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public class Lokacija
    {
        public int Id { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public string? Naslov { get; set; }

        public int DogodekId { get; set; }
        public Dogodek Dogodek { get; set; } = null!;
    }
}