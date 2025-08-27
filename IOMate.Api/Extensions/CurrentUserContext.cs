using IOMate.Domain.Entities;
using System.Text.Json;

namespace IOMate.Api.Extensions
{
    public static class CurrentUserContext
    {
        private static IHttpContextAccessor? _httpContextAccessor;

        public static void Configure(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor;
        }

        public static User? User
        {
            get
            {
                var principal = _httpContextAccessor?.HttpContext?.User;
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
