using Microsoft.AspNetCore.Identity;

namespace CodePuliseAPI.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateToken(IdentityUser user,List<string > roles);
    }
}
