using ImageGallery.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace ImageGallery.API.Authorization
{
    public class MustOwnImageHandler : AuthorizationHandler<MustOwnImageRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGalleryRepository _galleryRepository;

        public MustOwnImageHandler(
            IHttpContextAccessor httpContextAccessor,
            IGalleryRepository galleryRepository
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _galleryRepository = galleryRepository;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            MustOwnImageRequirement requirement)
        {
            var imageId = _httpContextAccessor.HttpContext.GetRouteValue("id").ToString();
            if(!Guid.TryParse(imageId, out Guid imageGuid))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var ownerId = context.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (!_galleryRepository.IsImageOwner(imageGuid, ownerId))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
