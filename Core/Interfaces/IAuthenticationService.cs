using Core.Enitities;
using Core.Enitities.Identity;

namespace Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<AppUser>> ValidateLoginByPassAsync(string email, string password, bool isPersistent = false);
        Task<Result<string>> GetUserRoleAsync(AppUser user);
    }
}
