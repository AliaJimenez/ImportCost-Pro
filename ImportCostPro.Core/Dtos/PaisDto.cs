using System;

namespace ImportCostPro.Core.Dtos
{
    public class PaisDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string CodigoISO { get; set; }
        public required bool Activo { get; set; }
        public  required DateTime FechaCreacion { get; set; }
        public  required DateTime FechaModificacion { get; set; }
    }
}