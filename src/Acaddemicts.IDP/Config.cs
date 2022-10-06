using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Acaddemicts.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Address(),
            new IdentityResource(
                "roles",
                "Your role(s)",
                new[] { "role" })
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            { 
                new ApiScope("imagegalleryapi")
            };

    public static IEnumerable<ApiResource> ApiResources =>
    new ApiResource[]
        {
                new ApiResource(
                    "imagegalleryapi", 
                    "Image Gallery Api",
                    new[] { "role" })
                {
                    Scopes = { "imagegalleryapi" }
                }
        };

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
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "imagegalleryapi"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RequireConsent = true,
                }
            };
}