# Image Gallery App - Implemented GetImages for logged in user
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
✅ We can logout from the application (including the idp) <br/>
✅ We're redirected back to the application after logout <br/>
✅ Our profile info is retrieved via the UserInfo endpoint <br/>
✅ PKCE is enabled <br/>
✅ Only Paying users are able to access The Order Frame page (by button or direct URI) <br/>
✅ Our API is secured <br/>
✅ Our Client Successfully passes the bearertoken to our API<br/>
✅ We are only able to see the images of the user that's logged in <br/>
❌ We can still get remove or edit individual images that do not belong to us <br/>
❌ We aren't able to upload images, as our identity isn't known <br/>
✅ We're running over https <br/>
