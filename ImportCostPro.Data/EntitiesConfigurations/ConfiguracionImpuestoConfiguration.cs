using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImportCostPro.Data.EntitiesConfigurations
{
    public class ConfiguracionImpuestoConfiguration : IEntityTypeConfiguration<ConfiguracionImpuesto>
    {
        public void Configure(EntityTypeBuilder<ConfiguracionImpuesto> builder)
        {
            #region Basic Configuration
            builder.ToTable("ConfiguracionesImpuestos");
            builder.HasKey(c => c.Id);
            #endregion

            #region Property Configuration
            builder.Property(c => c.PorcentajeITBIS)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            builder.Property(c => c.PorcentajeTasaServicioAduanal)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            builder.Property(c => c.FechaModificacion)
                .HasDefaultValueSql("GETDATE()");
            #endregion

            #region Relationship Configuration
            // Esta entidad no tiene relaciones foráneas según la estructura actual
            #endregion
        }
    }
}