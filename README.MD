# Image Gallery App - Added IDP UI
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
❌ We don't need to log in, so the application doesn't know our identity <br/>
❌ We are able to see images of all the users <br/>
❌ We aren't able to upload images, as our identity isn't known <br/>
✅ The only positive point is that we're running over https <br/>

## Changes
- Added IDP UI:
    ```
    cd src/Acaddemicts.IDP
    dotnet new isui
    ```
- Changed IDP `HostingExtensions.cs`:
    ```csharp
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services.AddIdentityServer(options =>
            {
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients);

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();

        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
    ```
