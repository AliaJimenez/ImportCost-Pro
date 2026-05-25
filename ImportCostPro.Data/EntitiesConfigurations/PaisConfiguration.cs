using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ImportCostPro.Data.EntitiesConfigurations
{
    public class PaisEntityConfiguration : IEntityTypeConfiguration<Pais>
    {
        public void Configure(EntityTypeBuilder<Pais> builder)
        {
            //fluent API 
            #region Basic Configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Paises");
            #endregion

            #region Property Configuration
            builder.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(p => p.CodigoISO).IsRequired().HasMaxLength(3);
            builder.Property(p => p.Activo).IsRequired().HasDefaultValue(true);
            builder.Property(p => p.FechaCreacion).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.FechaModificacion).IsRequired().HasDefaultValueSql("GETDATE()");
            #endregion

            #region Index Configuration
            builder.HasIndex(p => p.Nombre).IsUnique();
            builder.HasIndex(p => p.CodigoISO).IsUnique();
            #endregion

            #region Relationship Configuration
            builder.HasMany<Proveedor>(p => p.Proveedores)
                   .WithOne(pr => pr.PaisOrigen)
                   .HasForeignKey(pr => pr.PaisOrigenId)
                   .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany<Producto>(p => p.Productos)
            //       .WithOne(pr => pr.PaisOrigen)
            //       .HasForeignKey(pr => pr.PaisOrigenId)
            //       .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
