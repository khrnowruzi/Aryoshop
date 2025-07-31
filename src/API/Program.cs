using Application.Interfaces;
using Application.Services;
using Core.Interfaces;
using Core.Interfaces.Products;
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

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

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
