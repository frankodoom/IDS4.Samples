using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IDS4.Client
{
    class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            //////discover all the endpoints using metadata of identity server
            ////var disco = await DiscoveryClient.GetAsync("https://heimdall-auth.azurewebsites.net");
            //var disco = await DiscoveryClient.GetAsync("http://localhost:61011");
            ////var discoveryClient = new DiscoveryClient("Policy");
            ////discoveryClient.Policy = new DiscoveryPolicy { RequireHttps = false };
            //if (disco.IsError)
            //{
            //    Console.WriteLine(disco.Error);
            //    return;
            //}

            ////Grab a bearer token
            //var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");

            ////var tokenResponse = await tokenClient.RequestClientCredentialsAsync("Cloud911Api");
            //var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("f.odoom@accedegh.net", "Secure@124", "Cloud911Api");
            //Console.WriteLine(tokenResponse.AccessToken);

            //if (tokenResponse.IsError)
            //{
            //    Console.WriteLine(tokenResponse.Error);
            //    Console.ReadLine();
            //    return;
            //}

            //Console.WriteLine(tokenResponse.Json);
            //Console.WriteLine("\n\n");

           //Consume our Customer API
            //var client = new HttpClient();
            //client.SetBearerToken(tokenResponse.AccessToken);

            //var customerInfo = new StringContent(
            //    JsonConvert.SerializeObject(
            //            new { Id = 10, FirstName = "Manish", LastName = "Narayan" }),
            //            Encoding.UTF8, "application/json");

            //var createCustomerResponse = await client.PostAsync("http://localhost:59337/api/customers"
            //                                                , customerInfo);

            //if (!createCustomerResponse.IsSuccessStatusCode)
            //{
            //    Console.WriteLine(createCustomerResponse.StatusCode);
            //}

            //var getCustomerResponse = await client.GetAsync("http://localhost:59337/api/customers");
            //if (!getCustomerResponse.IsSuccessStatusCode)
            //{
            //    Console.WriteLine(getCustomerResponse.StatusCode);
            //}
            //else
            //{
            //    var content = await getCustomerResponse.Content.ReadAsStringAsync();
            //    Console.WriteLine(JArray.Parse(content));
            //}

            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:61011");

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest { Address = disco.TokenEndpoint, GrantType = IdentityModel.OidcConstants.GrantTypes.Password, ClientId = "client", ClientSecret = "secret", UserName = "f.odoom@accdegh.net", Password = "Secure@124", Parameters = { { "scope", "Cloud911Api" } } });
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            var token = tokenResponse.AccessToken;
            var iUserTOken = tokenResponse.IdentityToken;
            Console.WriteLine(token);
            Console.WriteLine(".........................................................................................................................................................");
            Console.WriteLine(iUserTOken);

            Console.ReadLine();

        }
    }
}
