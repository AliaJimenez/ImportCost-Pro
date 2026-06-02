using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Data.Entities
{
    public class Proveedor
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }

        public required int PaisOrigenId { get; set; }
        public Pais? PaisOrigen { get; set; }

        public required int MonedaPrincipalId { get; set; }
        public Moneda? MonedaPrincipal { get; set; }

        public string? Contacto { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }

        public required bool Activo { get; set; } = true;

        public required DateTime FechaCreacion { get; set; } = DateTime.Now;
        public required DateTime FechaModificacion { get; set; } = DateTime.Now;

        // Relación con órdenes de importación corregida (usando la clase correcta y C# 12)
        public ICollection<OrdenImportacion> OrdenesImportacion { get; set; } = [];
    }
}