using AutoMapper;
using ImageGallery.API.Entities;
using ImageGallery.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace ImageGallery.API;

public static class HostingExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
         .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:5001";
                options.Audience = "imagegalleryapi";

                // it's recommended to check the type header to avoid "JWT confusion" attacks
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    RoleClaimType = "role"
                };

                options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // register the DbContext on the container, getting the connection string from
        // appSettings (note: use this during development; in a production environment,
        // it's better to store the connection string in an environment variable)
        builder.Services.AddDbContext<GalleryContext>(options =>
        {
            options.UseSqlServer(
                builder.Configuration["ConnectionStrings:ImageGallery"]);
        });

        // register the repository
        builder.Services.AddScoped<IGalleryRepository, GalleryRepository>();

        // register AutoMapper-related services
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}
