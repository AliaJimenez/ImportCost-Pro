using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImportCostPro.Data.EntitiesConfigurations
{
    public class TasaCambioConfiguration : IEntityTypeConfiguration<TasaCambio>
    {
        public void Configure(EntityTypeBuilder<TasaCambio> builder)
        {
            #region Basic Configuration
            builder.ToTable("TasasCambio");
            builder.HasKey(t => t.Id);
            #endregion

            #region Property Configuration
            builder.Property(t => t.Tasa)
                .IsRequired()
                .HasColumnType("decimal(18,6)"); 

            builder.Property(t => t.FechaVigencia)
                .IsRequired();

            builder.Property(t => t.Activo)
                .HasDefaultValue(true);

            builder.Property(t => t.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(t => t.FechaModificacion)
                .HasDefaultValueSql("GETDATE()");
            #endregion

            #region Relationship Configuration
            builder.HasOne(t => t.MonedaOrigen)
                .WithMany(m => m.TasasCambioOrigen)
                .HasForeignKey(t => t.MonedaOrigenId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.MonedaDestino)
                .WithMany(m => m.TasasCambioDestino)
                .HasForeignKey(t => t.MonedaDestinoId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}