using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using IdentityServer4.Test;

using System.Linq;
using System.Threading.Tasks;

namespace IDS4.Client.MVC
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("Cloud911Api")
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
                    AllowedScopes = { "Cloud911Api"} //labels of the resources Identity Server is protecting
                },

                new Client
                {
                    ClientId = "mvc",
                    ClientName ="MVC Demo",
                    AllowedGrantTypes = GrantTypes.Implicit,
                   // RedirectUris = {"http://"}
                    AllowedScopes = { "Cloud911Api"} //labels of the resources Identity Server is protecting
                }
            };
        }

        internal static IEnumerable<IdentityResource> GetIdentityResources()
        {
            throw new NotImplementedException();
        }

        internal static IEnumerable<ApiResource> GetApiResources()
        {
            throw new NotImplementedException();
        }
    }
}
