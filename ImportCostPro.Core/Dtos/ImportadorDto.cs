using System;

namespace ImportCostPro.Core.Dtos
{
    public class ImportadorDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Rnc { get; set; }
        public required int PaisId { get; set; }
        public string? NombrePais { get; set; } // Para mostrar en las tablas
        public string? Direccion { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public required bool Activo { get; set; }
        public bool TieneOrdenes { get; set; }
        public required DateTime FechaCreacion { get; set; }
        public required DateTime FechaModificacion { get; set; }
    }
}