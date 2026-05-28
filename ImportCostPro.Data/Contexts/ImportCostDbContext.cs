using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ImportCostPro.Data.Contexts
{
    public class ImportCostDbContext : DbContext
    {
        public ImportCostDbContext(DbContextOptions<ImportCostDbContext> options) 
            : base(options)
        {
        }

        // Aqui iremos agregando sus DbSets (DbSet<Pais>, DbSet<Moneda>, etc.)

        public DbSet<CategoriaArancelaria> CategoriasArancelarias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Moneda> Monedas { get; set; }
        public DbSet<TasaCambio> TasasCambio { get; set; }
        public DbSet<ConfiguracionImpuesto> ConfiguracionesImpuestos { get; set; }
        //public DbSet<ConfiguracionImpuesto> ConfiguracionesImpuesto { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Importador> Importadores { get; set; }
        public DbSet <Proveedor> Proveedores { get; set; }
        public DbSet<OrdenProducto> OrdenProductos { get; set; }
        public DbSet<OrdenGasto> OrdenGastos { get; set; }

        public DbSet<OrdenImportacion> OrdenesImportacion { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //principio liskov

            //trae todas las configuraciones de entidades 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
           


        }
    }
}
