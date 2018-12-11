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
            //discover all the endpoints using metadata of identity server
            //var disco = await DiscoveryClient.GetAsync("https://heimdall-auth.azurewebsites.net");
            var disco = await DiscoveryClient.GetAsync("https://heimdall-auth.azurewebsites.net");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            //Grab a bearer token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("Cloud911Api");

           // var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("Cloud911Api", "password", "bob");


            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

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

            Console.ReadLine();

        }
    }
}
