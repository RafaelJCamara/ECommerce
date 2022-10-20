using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                   new Client
                   {
                       ClientId = "backendClient",
                       ClientName = "Backend Microservice App",
                       AllowedGrantTypes = GrantTypes.ClientCredentials,
                       //proof key protection should be false to not allow further proofs of auth
                       RequirePkce = false,
                       AllowRememberConsent = false,
                       //RedirectUris = new List<string>()
                       //{
                       //    "https://localhost:5002/signin-oidc"
                       //},
                       //PostLogoutRedirectUris = new List<string>()
                       //{
                       //    "https://localhost:5002/signout-callback-oidc"
                       //},
                       ClientSecrets = new List<Secret>
                       {
                           new Secret("secret".Sha256())
                       },
                       AllowedScopes = new List<string>
                       {
                           IdentityServerConstants.StandardScopes.OpenId,
                           IdentityServerConstants.StandardScopes.Profile,
                           IdentityServerConstants.StandardScopes.Address,
                           IdentityServerConstants.StandardScopes.Email,
                           "basketAPI",
                           "discountAPI",
                           "orderingAPI",
                           "roles"
                       },
                       //AllowAccessTokensViaBrowser = true
                   }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
           new ApiScope[]
           {
               new ApiScope("basketAPI", "Basket API"),
               new ApiScope("discountAPI", "Discount API"),
               new ApiScope("orderingAPI", "Ordering API")
           };

        public static IEnumerable<IdentityResource> IdentityResources =>
          new IdentityResource[]
          {
              new IdentityResources.OpenId(),
              new IdentityResources.Profile(),
              new IdentityResources.Address(),
              new IdentityResources.Email(),
              new IdentityResource(
                "roles",
                "Your role(s)",
                new List<string>() { "role" })
          };

        public static List<TestUser> TestUsers =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "username",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.GivenName, "rafael"),
                        new Claim(JwtClaimTypes.FamilyName, "camara")
                    }
                }
            };
    }
}
