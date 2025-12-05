namespace web.Models
{
    public class MojiDogodkiViewModel
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = null!;
        public DateTime DatumCas { get; set; }
        public string? Opis { get; set; }
        public int SteviloPrijav { get; set; }
    }
}