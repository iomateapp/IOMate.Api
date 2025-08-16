using IOMate.Api.Extensions;
using IOMate.Application.Extensions;
using IOMate.Infra.Context;
using IOMate.Infra.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureInfra(builder.Configuration);
builder.Services.ConfigureApplication();
builder.Services.ConfigureCorsPolicy();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureJwt(builder.Configuration);

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
app.UseMiddleware<ExceptionMiddleware>();
app.Run();

static void CreateDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dataContext.Database.EnsureCreated();
}
