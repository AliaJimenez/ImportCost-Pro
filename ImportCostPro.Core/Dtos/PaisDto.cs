using System;

namespace ImportCostPro.Core.Dtos
{
    public class PaisDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string CodigoISO { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}