using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ImportCostPro.Data.Entities;

namespace ImportCostPro.Data.EntitiesConfigurations
{
    public class ConfiguracionImpuestoConfiguration : IEntityTypeConfiguration<ConfiguracionImpuesto>
    {
        public void Configure(EntityTypeBuilder<ConfiguracionImpuesto> builder)
        {
            builder.HasKey(c => c.Id);
            
            // ITBIS decimal(5,2) - entre 0 y 100
            builder.Property(c => c.PorcentajeITBIS)
                .HasPrecision(5, 2)
                .IsRequired();
            
            // Tasa Servicio Aduanal decimal(5,2) - entre 0 y 100
            builder.Property(c => c.PorcentajeTasaServicioAduanal)
                .HasPrecision(5, 2)
                .IsRequired();
        }
    }
}
