using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;


namespace ImportCostPro.Data.EntitiesConfigurations
{
    public class ProveedorEntityConfiguration : IEntityTypeConfiguration<Proveedor>
    {
        public void Configure(EntityTypeBuilder<Proveedor> builder)
        {
            //fluent API 
            #region Basic Configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Proveedores");
            #endregion

            #region Property Configuration
            builder.Property(pr => pr.Nombre).IsRequired().HasMaxLength(150);
            builder.Property(pr => pr.PaisOrigenId).IsRequired();
            builder.Property(pr => pr.MonedaPrincipalId).IsRequired();
            builder.Property(pr => pr.Contacto).HasMaxLength(100);
            builder.Property(pr => pr.Email).HasMaxLength(100);
            builder.Property(pr => pr.Telefono).HasMaxLength(20);
            builder.Property(pr => pr.Direccion).HasMaxLength(300);
            builder.Property(pr => pr.Activo).IsRequired().HasDefaultValue(true);
            builder.Property(pr => pr.FechaCreacion).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(pr => pr.FechaModificacion).IsRequired().HasDefaultValueSql("GETDATE()");
            #endregion

            #region Index Configuration
            builder.HasIndex(pr => pr.Nombre).IsUnique();
            #endregion

            #region Relationship Configuration
            builder.HasOne(pr => pr.PaisOrigen)
                   .WithMany(p => p.Proveedores)
                   .HasForeignKey(pr => pr.PaisOrigenId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pr => pr.MonedaPrincipal)
                   .WithMany()
                   .HasForeignKey(pr => pr.MonedaPrincipalId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
