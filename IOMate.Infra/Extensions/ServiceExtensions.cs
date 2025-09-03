using IOMate.Application.UseCases.Events.GetEvents;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using IOMate.Infra.Context;
using IOMate.Infra.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IOMate.Infra.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureInfra(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Postgres");

            services.AddDbContext<AppDbContext>(opt =>
                opt.UseNpgsql(connectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<IRequestHandler<GetEventsRequestDto, List<GetEventResponseDto>>, GetEventsHandler<User>>();
        }
    }
}
