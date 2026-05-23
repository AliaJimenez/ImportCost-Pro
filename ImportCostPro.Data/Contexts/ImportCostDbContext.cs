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

        // Aquí Waldin, Yailyn y tú irán agregando sus DbSets (DbSet<Pais>, DbSet<Moneda>, etc.)

        public DbSet<CategoriaArancelaria> CategoriasArancelarias { get; set; }
        public DbSet<Producto> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //principio liskov

            //trae todas las configuraciones de entidades 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
           
        }
    }
}