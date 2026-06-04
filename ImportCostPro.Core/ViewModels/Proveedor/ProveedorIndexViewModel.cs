using System;

namespace ImportCostPro.Core.ViewModels.Proveedor
{
    public class ProveedorIndexViewModel
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public string? NombrePais { get; set; }
        public string? NombreMoneda { get; set; }
        public string? Email { get; set; }
        public bool Activo { get; set; }
        public bool TieneOrdenes { get; set; }
        public string? Telefono { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}