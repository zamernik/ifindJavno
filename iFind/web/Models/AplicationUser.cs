using Microsoft.AspNetCore.Identity;
using System;

namespace web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string Ime { get; set; }
        public required string Priimek { get; set; }
        public required string Spol { get; set; }
    }
}
