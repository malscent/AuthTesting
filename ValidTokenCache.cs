using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AuthTesting
{
    public class ValidTokenCache : IValidTokenGenerator
    {
        private class TokenCache
        {
            public string Token { get; set; }
            public DateTime CreatedDate { get; set; }
        }
        private readonly IValidTokenGenerator _generator = new ValidTokenGenerator();
        private Dictionary<string, TokenCache> _cache = new Dictionary<string, TokenCache>();
        public async Task<string> GetBearerTokenAsync(string AuthorityUrl, string ClientId, string ClientSecret, List<string> Scopes)
        {
            if (_cache.ContainsKey(ClientId))
            {
                var token = _cache[ClientId];
                if (token.CreatedDate > DateTime.Now.AddHours(-1))
                {
                    return token.Token;
                }
                else
                {
                    var newToken =  await _generator.GetBearerTokenAsync(AuthorityUrl, ClientId, ClientSecret, Scopes);
                    var newTokenCache = new TokenCache
                    {
                        Token = newToken,
                        CreatedDate = DateTime.Now
                    };
                    _cache[ClientId] = newTokenCache;
                }
            }
            else
            {
                var newToken =  await _generator.GetBearerTokenAsync(AuthorityUrl, ClientId, ClientSecret, Scopes);
                var newTokenCache = new TokenCache
                {
                    Token = newToken,
                    CreatedDate = DateTime.Now
                };
                _cache[ClientId] = newTokenCache;
            }
            return _cache[ClientId].Token;
        }

        public Task<string> GetPasswordTokenAsync(string AuthorityUrl, string UserName, string Password, string ClientId, string ClientSecret,
            List<string> Scopes)
        {
            return _generator.GetPasswordTokenAsync(AuthorityUrl, UserName, Password, ClientId, ClientSecret, Scopes);
        }
    }
}