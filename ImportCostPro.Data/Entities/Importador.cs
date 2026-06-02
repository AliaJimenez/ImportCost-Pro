using System;
using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Data.Entities
{
    public class Importador
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Rnc { get; set; }
        public required int PaisId { get; set; }
        public Pais? Pais { get; set; }
        public string? Contacto { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }

        public required bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaModificacion { get; set; } = DateTime.Now;

        // Próxima navegación a órdenes (aún no implementada)
        // public ICollection<OrdenImportacion> Ordenes { get; set; } = [];
    }
}