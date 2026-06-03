using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ImportCostPro.Data.Entities;

namespace ImportCostPro.Data.EntityConfigurations
{
    public class OrdenProductoConfiguration : IEntityTypeConfiguration<OrdenProducto>
    {
        public void Configure(EntityTypeBuilder<OrdenProducto> builder)
        {
            #region Basic configuration
            builder.HasKey(op => op.Id);
            builder.ToTable("OrdenProductos");
            #endregion

            #region Property configurations
            builder.Property(op => op.Cantidad).IsRequired().HasColumnType("decimal(10,4)");
            builder.Property(op => op.PrecioUnitarioFOB).IsRequired().HasColumnType("decimal(18,4)");
            builder.Property(op => op.MargenGananciaDeseado).IsRequired().HasColumnType("decimal(5,2)");
            builder.Property(op => op.FOBTotal).IsRequired().HasColumnType("decimal(18,4)");
            builder.Property(op => op.PesoTotal).IsRequired().HasColumnType("decimal(10,4)");
            builder.Property(op => op.VolumenTotal).HasColumnType("decimal(18,4)");
            #endregion

            #region Relationships
            builder.HasOne(op => op.OrdenImportacion)
                .WithMany(o => o.Productos)
                .HasForeignKey(op => op.OrdenImportacionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(op => op.Producto)
                .WithMany(p => p.OrdenProductos)
                .HasForeignKey(op => op.ProductoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}