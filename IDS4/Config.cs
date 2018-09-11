using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using IdentityServer4.Test;

using System.Linq;
using System.Threading.Tasks;

namespace IDS4
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("Cloud911Api", "Customer Api for BankOfDotNet")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "Cloud911Api","Test Api Resourse" } //labels of the resources Identity Server is protecting
                },

                new Client
                {
                    ClientId = "mvc",
                    ClientName ="MVC Demo",
                    AllowedGrantTypes = GrantTypes.Implicit,
                   // RedirectUris = {"http://"}
                    AllowedScopes = { "Cloud911Api" } //labels of the resources Identity Server is protecting
                }
            };
        }


        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
    {
        new TestUser
        {
            SubjectId = "1",
            Username = "alice",
            Password = "password"
        },
        new TestUser
        {
            SubjectId = "2",
            Username = "bob",
            Password = "password",
            
        }
    };
        }
    }
}
