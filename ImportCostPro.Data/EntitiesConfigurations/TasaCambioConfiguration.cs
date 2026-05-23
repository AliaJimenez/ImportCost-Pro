using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ImportCostPro.Data.Entities;

namespace ImportCostPro.Data.EntitiesConfigurations
{
    public class TasaCambioConfiguration : IEntityTypeConfiguration<TasaCambio>
    {
        public void Configure(EntityTypeBuilder<TasaCambio> builder)
        {
            builder.HasKey(t => t.Id);
            
            // Moneda origen (FK)
            builder.Property(t => t.MonedaOrigenId)
                .IsRequired();
            
            builder.HasOne(t => t.MonedaOrigen)
                .WithMany(m => m.TasasCambioOrigen)
                .HasForeignKey(t => t.MonedaOrigenId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Moneda destino (FK)
            builder.Property(t => t.MonedaDestinoId)
                .IsRequired();
            
            builder.HasOne(t => t.MonedaDestino)
                .WithMany(m => m.TasasCambioDestino)
                .HasForeignKey(t => t.MonedaDestinoId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Tasa decimal(18,6)
            builder.Property(t => t.Tasa)
                .HasPrecision(18, 6)
                .IsRequired();
            
            // Fecha vigencia
            builder.Property(t => t.FechaVigencia)
                .IsRequired();
            
            // Activo por defecto
            builder.Property(t => t.Activo)
                .HasDefaultValue(true);
        }
    }
}
