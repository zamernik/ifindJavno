using Microsoft.AspNetCore.Identity;
using System;

namespace web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string Ime { get; set; }
        public required string Priimek { get; set; }
        public required string Spol { get; set; }

        //1:* povezava z dogodki
        public ICollection<Dogodek> OrganiziraniDogodki { get; set; } = new List<Dogodek>();

        //1:* za udelezbo
        public ICollection<Udelezba> Udelezbe { get; set; } = new List<Udelezba>();
    }
}
