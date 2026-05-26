using System;

namespace ImportCostPro.Core.Dtos
{
    public class ImportadorDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Rnc { get; set; }
        public string? Direccion { get; set; }
        public string? Contacto { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }

        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}