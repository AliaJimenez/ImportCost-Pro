using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ImportCostPro.Data.Entities;

namespace ImportCostPro.Data.EntitiesConfigurations
{
    public class MonedaConfiguration : IEntityTypeConfiguration<Moneda>
    {
        public void Configure(EntityTypeBuilder<Moneda> builder)
        {
            builder.HasKey(m => m.Id);
            
            // Código ISO único y requerido
            builder.Property(m => m.CodigoISO)
                .IsRequired()
                .HasMaxLength(3);
            
            builder.HasIndex(m => m.CodigoISO)
                .IsUnique();
            
            // Nombre requerido
            builder.Property(m => m.Nombre)
                .IsRequired()
                .HasMaxLength(100);
            
            // Símbolo
            builder.Property(m => m.Simbolo)
                .HasMaxLength(5);
            
            // EsMonedaLocal por defecto false
            builder.Property(m => m.EsMonedaLocal)
                .HasDefaultValue(false);
            
            // Activo por defecto
            builder.Property(m => m.Activo)
                .HasDefaultValue(true);
            
            // Fechas
            builder.Property(m => m.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
