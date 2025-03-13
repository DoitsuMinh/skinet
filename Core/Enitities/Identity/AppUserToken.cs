
using Microsoft.AspNetCore.Identity;

namespace Core.Enitities.Identity
{
    public class AppUserToken: IdentityUserToken<string>
    {
        public DateTime ExpireDateUTC { get; set; }
    }
}
