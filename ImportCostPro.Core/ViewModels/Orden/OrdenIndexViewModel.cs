using System;

namespace ImportCostPro.Core.ViewModels.Orden
{
    public class OrdenIndexViewModel
    {
        public int Id { get; set; }
        public required string NumeroOrden { get; set; }
        public string? NombreImportador { get; set; }
        public string? NombreProveedor { get; set; }
        public string? NombrePais { get; set; }
        public string? NombreMoneda { get; set; }
        public required string Estado { get; set; }
        public decimal? PrecioSugerido { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}