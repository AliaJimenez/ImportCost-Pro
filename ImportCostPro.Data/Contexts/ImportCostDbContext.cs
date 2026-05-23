using ImportCostPro.Data.Entities;
using ImportCostPro.Data.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Data.Contexts
{
    public class ImportCostDbContext : DbContext
    {
        public ImportCostDbContext(DbContextOptions<ImportCostDbContext> options) 
            : base(options)
        {
        }

        // Iremos agregando los DbSets (DbSet<Pais>, DbSet<Moneda>, etc.)

        //public DbSet<CategoriaArancelaria> CategoriasArancelaria { get; set; }
        //public DbSet<Producto> Productos { get; set; }
        public DbSet<Moneda> Monedas { get; set; }
        public DbSet<TasaCambio> TasasCambio { get; set; }
        public DbSet<ConfiguracionImpuesto> ConfiguracionesImpuesto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new MonedaConfiguration());
            modelBuilder.ApplyConfiguration(new TasaCambioConfiguration());
            modelBuilder.ApplyConfiguration(new ConfiguracionImpuestoConfiguration());
        }

        public DbSet<CategoriaArancelaria> CategoriasArancelaria { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pais> Paises { get; set; } 
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet <Importador> importadores { get; set; }    
      

    }
}
