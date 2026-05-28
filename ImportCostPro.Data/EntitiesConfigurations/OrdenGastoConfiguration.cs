using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ImportCostPro.Data.Entities;

namespace ImportCostPro.Data.EntityConfigurations
{
    public class OrdenGastoConfiguration : IEntityTypeConfiguration<OrdenGasto>
    {
        public void Configure(EntityTypeBuilder<OrdenGasto> builder)
        {
            #region Basic configuration
            builder.HasKey(og => og.Id);
            builder.ToTable("OrdenGastos");
            #endregion

            #region Property configurations
            builder.Property(og => og.TipoGasto).IsRequired().HasMaxLength(50);
            builder.Property(og => og.Monto).IsRequired().HasColumnType("decimal(18,4)");
            builder.Property(og => og.MetodoDistribucion).IsRequired().HasMaxLength(50);
            builder.Property(og => og.FechaGasto).IsRequired();
            builder.Property(og => og.MontoEnMonedaLocal).IsRequired().HasColumnType("decimal(18,4)");
            #endregion

            #region Relationships
            builder.HasOne(og => og.OrdenImportacion)
                .WithMany(o => o.Gastos)
                .HasForeignKey(og => og.OrdenImportacionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(og => og.Moneda)
                .WithMany()
                .HasForeignKey(og => og.MonedaId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}