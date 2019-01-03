using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IDS4.MCV.Identity
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
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    //labels of the resources Identity Server is protecting
                    AllowedScopes = { "Cloud911Api","Cloud911 Test Api " }
                },
                new Client
                {
                    ClientId = "mvc",
                    ClientName ="MVC Demo",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                   //RedirectUris = {"http://"}
                   //labels of the resources Identity Server is protecting
                   AllowedScopes = {"cloud911"}
                }
            };
        }


        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource> {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource {
                Name = "role",
                UserClaims = new List<string> {"role"},
                
                
            }
        };
        }


            //InMemory User test
            //TODO remove in production
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

