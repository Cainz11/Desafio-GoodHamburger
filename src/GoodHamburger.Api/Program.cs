using GoodHamburger.Api.ExceptionHandling;
using GoodHamburger.Application;
using GoodHamburger.Infrastructure;
using GoodHamburger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter());
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new() { Title = "Good Hamburger API", Version = "v1" });
    });
}

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddApplication();

var connectionString = builder.Configuration.GetConnectionString("Default")
                       ?? "Server=localhost,1433;Database=GoodHamburger;User Id=sa;Password=GoodHamburger@2026;Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=True;Connection Timeout=60";
builder.Services.AddInfrastructure(connectionString);

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();
