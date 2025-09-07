using IOMate.Application.Security;
using IOMate.Application.UseCases.ClaimGroups.CheckUserClaim;
using IOMate.Domain.Entities;
using MediatR;
using System.Text.Json;

namespace IOMate.Api.Extensions
{
    public class CurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public CurrentUserContext(IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
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

        public async Task<bool> HasClaimAsync(string resource, string action, CancellationToken cancellationToken = default)
        {
            var user = User;
            if (user == null)
                return false;

            var query = new CheckUserClaimQuery(user.Id, resource, action);
            return await _mediator.Send(query, cancellationToken);
        }
    }
}
