# Image Gallery App - Added OIDC to ImageGallery.Client
This application is the base from which we will start constructing and consuming our identity provider.

## How to run?
- Open the .sln file with Visual Studio
- Set the `ImageGallery` connectionstring in the appsettings (preferably by user secrets)
- Run this command in the `src` folder:
    ```
    dotnet ef database update --project ImageGallery.API
    ```
- Set both `ImageGallery.API` and `ImageGallery.Client` as startup projects

## The application in the current state
✅ We need to login to the application <br/>
❌ We cannot logout from the application <br/>
❌ Our profile information isn't available <br/>
❌ PKCE isn't enabled<br/>
❌ We are able to see images of all the users <br/>
❌ We aren't able to upload images, as our identity isn't known <br/>
✅ We're running over https <br/>

## Changes
- Added to `ImageGallery Client Services`:
    ```csharp
        builder.Services.AddAuthentication(options =>
        {
            // the default scheme should be unique for every app on the same domain,
            // so taht cookies don't interfere with eachother
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.Authority = "https://localhost:5001";
            options.ClientId = "imagegalleryclient";
            options.ResponseType = "code";
            options.UsePkce = false;
            //options.CallbackPath = new PathString("...");
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.SaveTokens = true;
            options.ClientSecret = "secret";
        });
    ```
- Added to `Program.cs`:
  ```
  ...
  app.UseRouting();

  app.UseAuthentication();
  app.UseAuthorization();
  ...
  ```
- Decorated `GalleryController` with `[Authorize]` attribute
- Added to `GalleryController`:
  ```csharp
  public async Task WriteOutIdentityInformation()
  {
      // get the saved identity token
      var identityToken = await HttpContext
          .GetTokenAsync(OpenIdConnectParameterNames.IdToken);

      // write it out
      Debug.WriteLine($"Identity token: {identityToken}");

     //write out the user claims
     foreach (var claim in User.Claims)
     {
         Debug.WriteLine($"Claim Type: {claim.Type} - Claim value: {claim.Value}");
     }
  }
  ```
- Added to `GalleryController.Index`:
  ```csharp
  public async Task<IActionResult> Index()
  {
    await WriteOutIdentityInformation();
    ...
  ```
