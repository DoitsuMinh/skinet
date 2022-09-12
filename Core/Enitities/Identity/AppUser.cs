using Microsoft.AspNetCore.Identity;

namespace Core.Enitities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
    }
}