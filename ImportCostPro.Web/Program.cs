using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.Services;
using ImportCostPro.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar la cadena de conexión y el DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ImportCostDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Inyección de Dependencias (Servicios de Aplicación)
// Aquí iremos añadiendo los servicios según avancen
builder.Services.AddScoped<ICategoriaArancelariaService, CategoriaArancelariaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();