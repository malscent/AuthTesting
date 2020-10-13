using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace AuthTesting
{
    public class ValidTokenGenerator : IValidTokenGenerator
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public ValidTokenGenerator()
        {

        }

        public async Task<string> GetBearerTokenAsync(string AuthorityUrl, string ClientId, string ClientSecret, List<string> Scopes)
        {
            DiscoveryDocumentResponse disco = await _httpClient.GetDiscoveryDocumentAsync(AuthorityUrl);
            if (disco.IsError)
            {
                Console.WriteLine("Invalid Authority URL");
            }
            ClientCredentialsTokenRequest credReq = new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Scope = string.Join(" ", Scopes)
            };

            TokenResponse tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(credReq);
            if (tokenResponse.IsError)
            {
                Console.WriteLine("Unable to request token from authority.");
            }
            return tokenResponse.AccessToken;
        }

        public async Task<string> GetPasswordTokenAsync(string AuthorityUrl, string UserName, string Password, string ClientId, string ClientSecret, List<string> Scopes)
        {
            DiscoveryDocumentResponse disco = await _httpClient.GetDiscoveryDocumentAsync(AuthorityUrl);
            if (disco.IsError)
            {
                Console.WriteLine("Invalid Authority URL");
            }

            using (var sha1 = new SHA1Managed())
            {
                Console.WriteLine("-- Password Token Request -- ");
                Console.WriteLine($"Address: {disco.TokenEndpoint}");
                Console.WriteLine($"ClientId: {ClientId}");
                Console.WriteLine($"ClientSecret Hash: {Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(ClientSecret)))}");
                Console.WriteLine($"Username: {UserName}");
                Console.WriteLine($"Password Hash: {Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(Password)))}");
                Console.WriteLine($"Scopes: {string.Join(" ", Scopes)}");
            }

            PasswordTokenRequest passwordRequest = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Scope = string.Join(" ", Scopes),
                UserName = UserName,
                Password = Password
            };

            TokenResponse tokenResponse = await _httpClient.RequestPasswordTokenAsync(passwordRequest);
            if (tokenResponse.IsError)
            {
                Console.WriteLine("Unable to request token from authority.");
                Console.WriteLine(tokenResponse.ErrorDescription);
            }
            return tokenResponse.AccessToken;
        }
    }
}