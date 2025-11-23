using Microsoft.AspNetCore.Identity;

namespace web.Models
{   
    //Razred IdentityUser se nahaj v extensionu ki sem ga dodal torej Microdoft.AspNetCore.Identity
    public class ApplicationUser : IdentityUser
    {
        public string Ime { get; set; }
        public string Priimerk { get; set;}
    }
}