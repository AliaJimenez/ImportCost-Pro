using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Data.Contexts
{
    public class ImportCostDbContext : DbContext
    {
        public ImportCostDbContext(DbContextOptions<ImportCostDbContext> options) 
            : base(options)
        {
        }

        // Aquí Waldin, Yailyn y tú irán agregando sus DbSets (DbSet<Pais>, DbSet<Moneda>, etc.)

        public DbSet<CategoriaArancelaria> CategoriasArancelaria { get; set; }
        public DbSet<Producto> Productos { get; set; }
    }
}