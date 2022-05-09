using Core.Entities.Identity;
using Core.Helpers;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        Task<TokenFormat> CreateToken(AppUser user);
    }
}
