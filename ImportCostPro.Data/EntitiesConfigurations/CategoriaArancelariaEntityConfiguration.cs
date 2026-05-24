using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ImportCostPro.Data.Entities;

namespace ImportCostPro.Data.EntityConfigurations
{
    public class CategoriaArancelariaEntityConfiguration: IEntityTypeConfiguration<CategoriaArancelaria>
    {
        public void Configure(EntityTypeBuilder<CategoriaArancelaria> builder)
        {
            //fluent API 
            #region Basic Configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("CategoriasArancelarias");
            #endregion

            #region Property Configuration
            builder.Property(c => c.CodigoArancelario).IsRequired().HasMaxLength(20);
            builder.Property(c => c.Nombre).IsRequired().HasMaxLength(150);
            builder.Property(c => c.PorcentajeArancel).IsRequired().HasColumnType("decimal(5,2)");
            builder.Property(c => c.PorcentajeImpuestoSelectivo).HasColumnType("decimal(5,2)").HasDefaultValue(0);
            builder.Property(c => c.Activo).IsRequired().HasDefaultValue(true);
            #endregion

            #region Relationship Configuration
            builder.HasMany<Producto>(c => c.Productos)
                .WithOne(p => p.CategoriaArancelaria)
                .HasForeignKey(p => p.CategoriaArancelariaId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
