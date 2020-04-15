using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class SiteUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Year { get; set; }
    }
}