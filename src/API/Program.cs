using API.Middleware;
using Application.Services;
using Domain.Interfaces;
using Domain.Interfaces.Products;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Products;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<ISlugService, SlugService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

builder.Services.AddCors();

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
