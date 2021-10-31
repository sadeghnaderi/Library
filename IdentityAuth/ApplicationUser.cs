using Library.Models;
using Microsoft.AspNetCore.Identity;

namespace Library.IdentityAuth
{
    public class ApplicationUser : IdentityUser
    {
         public string Name { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}
