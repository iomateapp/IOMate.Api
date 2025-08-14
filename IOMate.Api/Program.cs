using IOMate.Infra;
using IOMate.Application;
using IOMate.Infra.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureInfra(builder.Configuration);
builder.Services.ConfigureApplication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

CreateDatabase(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

static void CreateDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dataContext.Database.EnsureCreated();
}
