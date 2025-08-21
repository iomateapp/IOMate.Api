using IOMate.Api.Extensions;
using IOMate.Application.Extensions;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using IOMate.Infra.Context;
using IOMate.Infra.Extensions;
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

builder.Services.AddLocalization();

var app = builder.Build();

CreateDatabase(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
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

static void CreateDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dataContext.Database.EnsureCreated();

    // Seed admin user
    if (!dataContext.Users.Any(u => u.Email == "admin@iomate.com"))
    {
        var passwordHasher = scope.ServiceProvider.GetService<IPasswordHasher>();
        var admin = new User
        {
            FirstName = "Admin",
            LastName = "IOMate",
            Email = "admin@iomate.com",
            Password = passwordHasher?.HashPassword("Admin@123")
        };
        dataContext.Users.Add(admin);
        dataContext.SaveChanges();
    }
}
