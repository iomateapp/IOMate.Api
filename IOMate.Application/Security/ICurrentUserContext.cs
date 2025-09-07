using IOMate.Domain.Entities;

namespace IOMate.Application.Security
{
    public interface ICurrentUserContext
    {
        User? User { get; }
        Task<bool> HasClaimAsync(string resource, string action, CancellationToken cancellationToken = default);
    }
}