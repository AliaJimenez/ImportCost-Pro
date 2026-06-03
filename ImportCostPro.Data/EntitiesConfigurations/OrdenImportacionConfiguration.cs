using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImportCostPro.Data.EntityConfigurations
    {
        public class OrdenImportacionConfiguration : IEntityTypeConfiguration<OrdenImportacion>
        {
            public void Configure(EntityTypeBuilder<OrdenImportacion> builder)
            {
                #region Basic Configuration
                builder.HasKey(x => x.Id);
                builder.ToTable("OrdenesImportacion");
                #endregion

                #region Property Configuration
                builder.Property(o => o.NumeroOrden)
                    .IsRequired()
                    .HasMaxLength(50);

                builder.Property(o => o.ImportadorId)
                    .IsRequired();

                builder.Property(o => o.ProveedorId)
                    .IsRequired();

                builder.Property(o => o.PaisOrigenId)
                    .IsRequired();

                builder.Property(o => o.MonedaId)
                    .IsRequired();

                builder.Property(o => o.Estado)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("Abierta");

                builder.Property(o => o.CostoFOB)
                    .HasColumnType("decimal(18,2)");

                builder.Property(o => o.CIF)
                    .HasColumnType("decimal(18,2)");

                builder.Property(o => o.Arancel)
                    .HasColumnType("decimal(18,2)");

                builder.Property(o => o.ImpuestoSelectivo)
                    .HasColumnType("decimal(18,2)");

                builder.Property(o => o.TasaAduanal)
                    .HasColumnType("decimal(18,2)");

                builder.Property(o => o.ITBIS)
                    .HasColumnType("decimal(18,2)");

                builder.Property(o => o.PrecioSugerido)
                    .HasColumnType("decimal(18,2)");

                builder.Property(o => o.Activo)
                    .IsRequired()
                    .HasDefaultValue(true);

                builder.Property(o => o.FechaCreacion)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");

                builder.Property(o => o.FechaModificacion)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");
                #endregion

                #region Index Configuration
                builder.HasIndex(o => o.NumeroOrden)
                    .IsUnique();
                #endregion

                #region Relationship Configuration
                builder.HasOne(o => o.Importador)
                    .WithMany()
                    .HasForeignKey(o => o.ImportadorId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Proveedor)
                  .WithMany(p => p.OrdenesImportacion)
                  .HasForeignKey(o => o.ProveedorId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.PaisOrigen)
                    .WithMany()
                    .HasForeignKey(o => o.PaisOrigenId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(o => o.Moneda)
                    .WithMany()
                    .HasForeignKey(o => o.MonedaId)
                    .OnDelete(DeleteBehavior.Restrict);
                #endregion
            }
        }
    
}
