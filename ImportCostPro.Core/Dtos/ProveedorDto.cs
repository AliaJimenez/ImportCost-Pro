using System;

namespace ImportCostPro.Core.Dtos
{
    public class ProveedorDto
    {
        public int Id { get; set; }
       
        public required string Nombre { get; set; }
        public required int PaisOrigenId { get; set; }
        public required int MonedaPrincipalId { get; set; }
        public required bool Activo { get; set; }
        public required DateTime FechaCreacion { get; set; }
        public required DateTime FechaModificacion { get; set; }

        public string? NombrePais { get; set; }
        public string? NombreMoneda { get; set; }
        public string? Contacto { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        
        public bool TieneOrdenes { get; set; }
    }
}