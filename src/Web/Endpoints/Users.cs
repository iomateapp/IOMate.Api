using IOMate.Api.Application.Users.Queries.GetUsers;
using IOMate.Api.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace IOMate.Api.Web.Endpoints;

public class Users : EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapIdentityApi<ApplicationUser>();

        groupBuilder.MapGet("/", (UserManager<ApplicationUser> userManager) =>
        {
            var users = userManager.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .ToList();

            return Results.Ok(users);
        })
        .RequireAuthorization();

        groupBuilder.MapPut("/{id}", async (string id, UpdateUserDto dto, UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return Results.NotFound(new { message = "Usuário não encontrado." });

            // Atualiza os campos
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.UserName = dto.UserName;
            user.Email = dto.Email;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Results.BadRequest(result.Errors);

            return Results.Ok(new
            {
                message = "Usuário atualizado com sucesso.",
                user = new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.FirstName,
                    user.LastName
                }
            });
        })
.RequireAuthorization();
    }
}
