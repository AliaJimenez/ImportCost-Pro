using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Data
{
    public class ImportCostDbContext : DbContext
    {
        public ImportCostDbContext(DbContextOptions<ImportCostDbContext> options) 
            : base(options)
        {
        }

        // Aquí Waldin, Yailyn y tú irán agregando sus DbSets (DbSet<Pais>, DbSet<Moneda>, etc.)
    }
}