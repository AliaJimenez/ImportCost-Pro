using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImportCostPro.Data.EntitiesConfigurations
{
    public class ImportadorEntityConfiguration : IEntityTypeConfiguration<Importador>
    {
        public void Configure(EntityTypeBuilder<Importador> builder)
        {
            //fluent API 
            #region Basic Configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Importadores");
            #endregion

            #region Property Configuration
            builder.Property(i => i.Nombre).IsRequired().HasMaxLength(150);
            builder.Property(i => i.Rnc).IsRequired().HasMaxLength(20);
            builder.Property(i => i.Direccion).HasMaxLength(300);
            builder.Property(i => i.Contacto).HasMaxLength(100);
            builder.Property(i => i.Email).HasMaxLength(100);
            builder.Property(i => i.Telefono).HasMaxLength(20);
            builder.Property(i => i.Activo).IsRequired().HasDefaultValue(true);
            builder.Property(i => i.FechaCreacion).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(i => i.FechaModificacion).IsRequired().HasDefaultValueSql("GETDATE()");
            #endregion

            #region Index Configuration
            builder.HasIndex(i => i.Rnc).IsUnique();
            #endregion
        }
    }
}
