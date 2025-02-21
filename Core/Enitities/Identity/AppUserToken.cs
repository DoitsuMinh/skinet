
using Microsoft.AspNetCore.Identity;

namespace Core.Enitities.Identity
{
    public class AppUserToken: IdentityUserToken<string> 
    {
        public DateTime ExpiredDate { get; set; }
    }
}
