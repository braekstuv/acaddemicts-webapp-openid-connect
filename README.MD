# Image Gallery App - Logout from application
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
❌ We cannot logout from the application (We're still logged in to the IDP) <br/>
❌ Our profile information isn't available <br/>
✅ PKCE is enabled<br/>
❌ We are able to see images of all the users <br/>
❌ We aren't able to upload images, as our identity isn't known <br/>
✅ We're running over https <br/>
