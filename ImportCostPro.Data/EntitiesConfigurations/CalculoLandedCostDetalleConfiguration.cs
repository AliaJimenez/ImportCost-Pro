using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImportCostPro.Data.EntitiesConfigurations
{
    public class CalculoLandedCostDetalleConfiguration : IEntityTypeConfiguration<CalculoLandedCostDetalle>
    {
        public void Configure(EntityTypeBuilder<CalculoLandedCostDetalle> builder)
        {
            #region Basic Configuration
            builder.HasKey(d => d.Id);
            #endregion

            #region Property Configuration
            builder.Property(d => d.Cantidad).HasColumnType("decimal(18,2)");
            builder.Property(d => d.FobOriginalUnitario).HasColumnType("decimal(18,2)");
            builder.Property(d => d.FobLocalTotal).HasColumnType("decimal(18,2)");
            builder.Property(d => d.FleteAsignado).HasColumnType("decimal(18,2)");
            builder.Property(d => d.SeguroAsignado).HasColumnType("decimal(18,2)");
            builder.Property(d => d.GastosLocalesAsignados).HasColumnType("decimal(18,2)");
            builder.Property(d => d.ValorCif).HasColumnType("decimal(18,2)");
            builder.Property(d => d.MontoArancel).HasColumnType("decimal(18,2)");
            builder.Property(d => d.MontoIsc).HasColumnType("decimal(18,2)");
            builder.Property(d => d.MontoTasaServicio).HasColumnType("decimal(18,2)");
            builder.Property(d => d.MontoItbis).HasColumnType("decimal(18,2)");
            builder.Property(d => d.CostoTotalImportado).HasColumnType("decimal(18,2)");
            builder.Property(d => d.CostoUnitarioImportado).HasColumnType("decimal(18,2)");
            builder.Property(d => d.MargenDeseadoAplicado).HasColumnType("decimal(5,2)");
            builder.Property(d => d.PrecioVentaSugerido).HasColumnType("decimal(18,2)");
            #endregion

            #region Relationship Configuration
            builder.HasOne(d => d.Producto)
                    .WithMany()
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}