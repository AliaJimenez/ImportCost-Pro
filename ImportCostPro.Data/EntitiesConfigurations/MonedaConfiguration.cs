using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImportCostPro.Data.EntitiesConfigurations
{
    public class MonedaConfiguration : IEntityTypeConfiguration<Moneda>
    {
        public void Configure(EntityTypeBuilder<Moneda> builder)
        {
            #region Basic Configuration
            builder.ToTable("Monedas");
            builder.HasKey(m => m.Id);
            #endregion

            #region Property Configuration
            builder.Property(m => m.CodigoISO)
                .IsRequired()
                .HasMaxLength(3)
                .IsFixedLength(); // El ISO siempre tiene exactamente 3 caracteres

            builder.Property(m => m.Nombre)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(m => m.Simbolo)
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(m => m.EsMonedaLocal)
                .HasDefaultValue(false);

            builder.Property(m => m.Activo)
                .HasDefaultValue(true);

            builder.Property(m => m.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(m => m.FechaModificacion)
                .HasDefaultValueSql("GETDATE()");
            #endregion

            #region Index Configuration
            builder.HasIndex(m => m.CodigoISO)
                .IsUnique();
            #endregion

            #region Relationship Configuration
            // Relación 1 a Muchos: Una Moneda puede ser el Origen de muchas Tasas de Cambio
            builder.HasMany(m => m.TasasCambioOrigen)
                .WithOne(t => t.MonedaOrigen)
                .HasForeignKey(t => t.MonedaOrigenId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación 1 a Muchos: Una Moneda puede ser el Destino de muchas Tasas de Cambio
            builder.HasMany(m => m.TasasCambioDestino)
                .WithOne(t => t.MonedaDestino)
                .HasForeignKey(t => t.MonedaDestinoId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}