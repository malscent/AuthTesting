using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AuthTesting
{
    class Program
    {
        
        public const string OAUTH_BASE_URL = "https://oauth.dev.valididcloud.com";
        public const string CLIENT_ID = "93c285a9-6ef6-4e16-afb7-1d3989f6becc";
        public const string CLIENT_SECRET = "47bed627-19f4-4194-afa0-2c223df09df8";
        public static List<string> Scopes = new List<string>
        {
            "https://cap.dev.valididcloud.com/", "https://cap.dev.valididcloud.com/organization",
            "https://cap.dev.valididcloud.com/reflection", "https://cap.dev.valididcloud.com/system", 
            "https://cap.dev.valididcloud.com/user"
        };
        static async Task Main(string[] args)
        {
            IValidTokenGenerator generator = new ValidTokenCache();

            var token = await generator.GetBearerTokenAsync(OAUTH_BASE_URL, CLIENT_ID, CLIENT_SECRET, Scopes);
            var client = new HttpClient();
            Console.WriteLine(token);
            var url = "https://cap.dev.valididcloud.com/accounts";
            var second_url = "https://cap.dev.valididcloud.com/users/no-user-groups";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(url);
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            var response_two = await client.GetAsync(second_url);
            Console.WriteLine(await response_two.Content.ReadAsStringAsync());

        }
    }
}