using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Acaddemicts.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            { };

    public static IEnumerable<Client> Clients =>
        new Client[]
            {
                new()
                {
                    ClientName = "ImageGallery",
                    ClientId = "imagegalleryclient",
                    // Avoid setting this to true, we'd rather get the claims from the userinfo endpoint 
                    //AlwaysIncludeUserClaimsInIdToken = true, 
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true, // set to true by default
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44389/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:44389/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RequireConsent = true
                }
            };
}