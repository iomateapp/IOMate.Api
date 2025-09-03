using IOMate.Domain.Entities;

namespace IOMate.Application.Security
{
    public interface ICurrentUserContext
    {
        User? User { get; }
    }
}