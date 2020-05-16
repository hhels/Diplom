using Diplom.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace Diplom.Server.Models
{
    public class SiteUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Year { get; set; }

        public RussType Russ { get; set; }
}
}