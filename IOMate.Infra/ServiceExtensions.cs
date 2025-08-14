using IOMate.Domain.Interfaces;
using IOMate.Infra.Context;
using IOMate.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IOMate.Infra
{
    public static class ServiceExtensions
    {
        public static void ConfigureInfra(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Postgres");

            services.AddDbContext<AppDbContext>(opt =>
                opt.UseNpgsql(connectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
