using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthTesting
{
    public interface IValidTokenGenerator
    {
        Task<string> GetBearerTokenAsync(string AuthorityUrl, string ClientId, string ClientSecret, List<string> Scopes);
        Task<string> GetPasswordTokenAsync(string AuthorityUrl, string UserName, string Password, string ClientId, string ClientSecret, List<string> Scopes);
    }
}