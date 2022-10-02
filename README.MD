# Image Gallery App - Changed TestUsers
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
- Changed IDP `TestUsers.cs`:
    ```csharp
    public static List<TestUser> Users = new List<TestUser>
    {
        new TestUser{
            SubjectId = "b6f7a7b1-f403-4434-8dac-8495ddd1f36e",
            Username = "Frank",
            Password = "password",
            Claims = new List<Claim>{
                new Claim("given_name", "Frank"),
                new Claim("family_name", "Underwood")
            }
        },
        new TestUser{
            SubjectId = "b99748a7-ee9c-4e05-8c71-784dda2d2e87",
            Username = "Claire",
            Password = "password",
            Claims = new List<Claim>{
                new Claim("given_name", "Claire"),
                new Claim("family_name", "Underwood")
            }
        },
    };
    ```
- Add Testusers to `IDP Services`:
    ```csharp
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        ...
        .AddInMemoryClients(Config.Clients)
        .AddTestUsers(TestUsers.Users);
        ...
    }
    ```
- Add Profile to `Config.IdentityResources`:
    ```csharp
    public static IEnumerable<IdentityResource> IdentityResources =>
    new IdentityResource[]
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    };
    ```
