using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.Services;
using ImportCostPro.Data.Contexts;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// 1. Configurar la cadena de conexión y el DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ImportCostDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Inyección de Dependencias (Servicios de Aplicación)
// Aquí iremos añadiendo los servicios según avancen
// Asegúrate de tener esto en tu Program.cs
builder.Services.AddScoped<IMonedaService, MonedaService>();
builder.Services.AddScoped<ITasaCambioService, TasaCambioService>();
builder.Services.AddScoped<IConfiguracionImpuestoService, ConfiguracionImpuestoService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ICategoriaArancelariaService, CategoriaArancelariaService>(); 
builder.Services.AddScoped<IPaisService, PaisService>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();
builder.Services.AddScoped<IImportadorService, ImportadorService>();
builder.Services.AddScoped<IOrdenProductoService, OrdenProductoService>();
builder.Services.AddScoped<IOrdenGastoService, OrdenGastoService>();
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