using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;

namespace IDS4.MCV.Identity
{
    public class Config
    {
        // scopes define the resources in your system

        public static IEnumerable<IdentityResource> GetIdentityResources()

        {

            return new List<IdentityResource>

            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }



        public static IEnumerable<ApiResource> GetApiResources()
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

                   ClientId = "mvc",
                   ClientName = "MVC Client",
                   AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                   RequireConsent = false,

                  ClientSecrets =
                  {
                        new Secret("secret".Sha256())
                  },

                  RedirectUris =           { "http://localhost:5002/signin-oidc" },
                  PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                  AllowedScopes =
                  {
                     IdentityServerConstants.StandardScopes.OpenId,
                     IdentityServerConstants.StandardScopes.Profile,
                     "Cloud911Api"
                  },
                  AllowOfflineAccess = true
                }
            };
        }
    }
}
