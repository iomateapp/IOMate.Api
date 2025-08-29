using IOMate.Application.Security;
using IOMate.Domain.Entities;
using System.Text.Json;

namespace IOMate.Api.Extensions
{
    public class CurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public User? User
        {
            get
            {
                var principal = _httpContextAccessor.HttpContext?.User;
                if (principal == null)
                    return null;

                var userClaim = principal.FindFirst("user")?.Value;
                if (string.IsNullOrWhiteSpace(userClaim))
                    return null;

                try
                {
                    return JsonSerializer.Deserialize<User>(userClaim);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
