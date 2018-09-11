using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using IdentityServer4.Test;

using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;

namespace IDS4
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("Cloud911Api", "Test Api Resource")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                //Authenticate an Application 
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "Cloud911Api","Test Api Resource" } //labels of the resources Identity Server is protecting
                },


                //Authenticate a User with Username & Password
                new Client
                {
                    ClientId = "android",

                    // no interactive user, use the clientid/secret for authentication
                    // used to authenticate applications
                    AllowedGrantTypes = GrantTypes.Implicit,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "Cloud911Api","Test Api Resource" } //labels of the resources Identity Server is protecting
                },



                //Authenticate MVC App with OpenID Connect implicit flow client 
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                   // where to redirect to after login
                  RedirectUris = { "http://localhost:5002/signin-oidc" },

                   // where to redirect to after logout
                  PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                   AllowedScopes = new List<string>
                   {
                     IdentityServerConstants.StandardScopes.OpenId,
                     IdentityServerConstants.StandardScopes.Profile
                   }
                }
           };

        }



        //InMemory Test Users for Development, persist users in production
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




        //Add Support for OpenId Scopes
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                //The SubjectId
               new IdentityResources.OpenId(),

               //(first name, last name etc..)
               new IdentityResources.Profile(),
           };
        }

    }


}
