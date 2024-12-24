using Duende.IdentityServer.Models;

namespace WebApiAuthentication.DemoIDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        ];

    public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope("claimsapi")
        ];

    public static IEnumerable<ApiResource> ApiResources =>
    [
        new ApiResource("claimsapi")
        {
            Scopes = { "claimsapi" }
        }
    ];

    public static IEnumerable<Client> Clients =>
        [
            // client credentials flow client
            new Client
            {
                ClientId = "democlientclientcredentials",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "claimsapi" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "democlientcodepkce",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                    
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:7267/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:7267/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:7267/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "claimsapi" }
            },
        ];
}
