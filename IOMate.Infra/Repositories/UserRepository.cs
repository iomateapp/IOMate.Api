using IOMate.Application.Security;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using IOMate.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace IOMate.Infra.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context, ICurrentUserContext currentUserContext)
            : base(context, currentUserContext) { }

        public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken)
        {
            return await Context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
    }
}
