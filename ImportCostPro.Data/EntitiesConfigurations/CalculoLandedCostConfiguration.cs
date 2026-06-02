using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImportCostPro.Data.EntitiesConfigurations
{
    public class CalculoLandedCostConfiguration : IEntityTypeConfiguration<CalculoLandedCost>
    {
        public void Configure(EntityTypeBuilder<CalculoLandedCost> builder)
        {
            #region Basic Configuration
            builder.HasKey(c => c.Id);
            #endregion

            #region Property Configuration
            builder.Property(c => c.FobTotalLocal).HasColumnType("decimal(18,2)");
            builder.Property(c => c.FleteTotalLocal).HasColumnType("decimal(18,2)");
            builder.Property(c => c.SeguroTotalLocal).HasColumnType("decimal(18,2)");
            builder.Property(c => c.GastosLocalesTotal).HasColumnType("decimal(18,2)");
            builder.Property(c => c.CifTotalLocal).HasColumnType("decimal(18,2)");
            builder.Property(c => c.TotalArancel).HasColumnType("decimal(18,2)");
            builder.Property(c => c.TotalIsc).HasColumnType("decimal(18,2)");
            builder.Property(c => c.TotalTasaServicio).HasColumnType("decimal(18,2)");
            builder.Property(c => c.TotalItbis).HasColumnType("decimal(18,2)");
            builder.Property(c => c.CostoTotalImportacion).HasColumnType("decimal(18,2)");
            builder.Property(c => c.PorcentajeTasaServicioUsado).HasColumnType("decimal(5,2)");
            builder.Property(c => c.PorcentajeItbisUsado).HasColumnType("decimal(5,2)");
            #endregion

            #region Relationship Configuration
            builder.HasMany(c => c.Detalles)
                .WithOne(d => d.CalculoLandedCost)
                .HasForeignKey(d => d.CalculoLandedCostId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}