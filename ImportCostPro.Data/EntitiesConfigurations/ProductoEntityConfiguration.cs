using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ImportCostPro.Data.Entities;

namespace ImportCostPro.Data.EntitiesConfigurations
{
    public class ProductoEntityConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            #region Basic Configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Productos");
            #endregion

            #region Property Configuration
            builder.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
            builder.Property(p => p.CodigoReferencia).IsRequired().HasMaxLength(50);
            builder.Property(p => p.PesoUnitario).IsRequired().HasColumnType("decimal(10,4)");
            builder.Property(p => p.Largo).IsRequired().HasColumnType("decimal(10,4)");
            builder.Property(p => p.Ancho).IsRequired().HasColumnType("decimal(10,4)");
            builder.Property(p => p.Alto).IsRequired().HasColumnType("decimal(10,4)");
            builder.Property(p => p.UnidadMedida).IsRequired().HasMaxLength(20);
            builder.Property(p => p.Descripcion).HasMaxLength(250);
            builder.Property(p => p.Activo).IsRequired().HasDefaultValue(true);
            #endregion

            #region Relationship Configuration
            // builder.HasOne<Pais>(p => p.PaisOrigen)
            //     .WithMany()
            //     .HasForeignKey(p => p.PaisOrigenId)
            //     .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }

    }
}
