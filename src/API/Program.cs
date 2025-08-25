using API.Middleware;
using Application.Services;
using Domain.Interfaces;
using Domain.Interfaces.Products;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Products;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new Exception("Cannot get default connection string");

    opt.UseSqlServer(connectionString);
});
builder.Services.AddSingleton<ISlugService, SlugService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddCors();
builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
{
    var connectionString = builder.Configuration.GetConnectionString("Redis")
        ?? throw new Exception("Cannot get redis connection string");
    var configuration = ConfigurationOptions.Parse(connectionString, true);
    return ConnectionMultiplexer.Connect(configuration);
}
);
builder.Services.AddSingleton<ICartService, CartService>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    var slugService = services.GetRequiredService<ISlugService>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context, slugService);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    throw;
}

app.Run();
