using Microsoft.AspNetCore.Http;
using RealTimeSupportChat.Application.Contracts.Identity;
using System.Security.Claims;

namespace RealTimeSupportChat.Identity.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => 
            _httpContextAccessor.HttpContext?
            .User?
            .FindFirst("uid")?
            .Value;


        public string Role =>
            _httpContextAccessor.HttpContext
            .User?
            .FindFirst(ClaimTypes.Role)
            .Value;

    }
}
