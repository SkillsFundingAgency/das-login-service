using System.Collections.Generic;
using IdentityServer4.Models;

namespace SFA.DAS.LoginService.Web
{
    public static class Config
    {
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "api1" }
                
                },
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    
                    RedirectUris = { "https://localhost:6016/signin-oidc" },
                    
                    PostLogoutRedirectUris = { "https://localhost:6016/signout-callback-oidc" }, 

                    RequireConsent = false,

                    AllowedScopes = { "openid" }
                },
            }; 
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            }; 
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API 1")
            }; 
        }
    }
}