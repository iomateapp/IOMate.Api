using IOMate.Api.Extensions;
using IOMate.Application.Extensions;
using IOMate.Application.Security;
using IOMate.Application.UseCases.ClaimGroups.AddClaimToGroup;
using IOMate.Application.UseCases.ClaimGroups.AssignToUser;
using IOMate.Application.UseCases.ClaimGroups.CreateClaimGroup;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using IOMate.Domain.Shared;
using IOMate.Infra.Context;
using IOMate.Infra.Extensions;
using MediatR;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureInfra(builder.Configuration);
builder.Services.ConfigureApplication();
builder.Services.ConfigureCorsPolicy();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.ConfigureAuthorization();

builder.Services.AddLocalization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();

var app = builder.Build();

await CreateDatabaseAsync(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

var supportedCultures = new[] { "pt-BR", "en-US" };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pt-BR"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
});
app.UseMiddleware<ExceptionMiddleware>();

app.Run();

static async Task CreateDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

    dataContext.Database.EnsureCreated();

    // Seed admin user
    User? adminUser = null;
    if (!dataContext.Users.Any(u => u.Email == "admin@iomate.com"))
    {
        var passwordHasher = scope.ServiceProvider.GetService<IPasswordHasher>();
        adminUser = new User
        {
            FirstName = "Admin",
            LastName = "IOMate",
            Email = "admin@iomate.com",
            Password = passwordHasher?.HashPassword("Admin@123")
        };
        dataContext.Users.Add(adminUser);
        await dataContext.SaveChangesAsync();
    }
    else
    {
        adminUser = dataContext.Users.FirstOrDefault(u => u.Email == "admin@iomate.com");
    }

    if (adminUser != null && !dataContext.ClaimGroups.Any(cg => cg.Name == "Admin"))
    {
        var createGroupCommand = new CreateClaimGroupCommand("Admin", "Group with full administrative permissions");
        var groupResult = await mediator.Send(createGroupCommand);

        if (groupResult != null)
        {
            var claimCommands = new List<AddClaimToGroupCommand>();

            foreach (var resource in ApplicationClaims.ResourcesAndActions)
            {
                foreach (var action in resource.Value)
                {
                    claimCommands.Add(new AddClaimToGroupCommand(groupResult.Id, resource.Key, action));
                }
            }

            foreach (var command in claimCommands)
            {
                await mediator.Send(command);
            }

            var assignCommand = new AssignClaimGroupToUserCommand(groupResult.Id, adminUser.Id);
            await mediator.Send(assignCommand);
        }
    }
}
