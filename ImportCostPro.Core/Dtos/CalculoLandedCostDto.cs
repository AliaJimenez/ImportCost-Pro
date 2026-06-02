using System;
using System.Collections.Generic;

namespace ImportCostPro.Core.Dtos
{
    public class CalculoLandedCostDto
    {
        public int OrdenImportacionId { get; set; }
        public string NumeroOrden { get; set; } = string.Empty;
        public DateTime FechaCalculo { get; set; }
        
        // Totales consolidados en Moneda Local
        public decimal FobTotalLocal { get; set; }
        public decimal FleteTotalLocal { get; set; }
        public decimal SeguroTotalLocal { get; set; }
        public decimal GastosLocalesTotal { get; set; }
        public decimal CifTotalLocal { get; set; }
        
        // Totales de Impuestos
        public decimal TotalArancel { get; set; }
        public decimal TotalIsc { get; set; }
        public decimal TotalTasaServicio { get; set; }
        public decimal TotalItbis { get; set; }
        public decimal CostoTotalImportacion { get; set; }
        
        // Guardar configuración del momento
        public decimal PorcentajeTasaServicioUsado { get; set; }
        public decimal PorcentajeItbisUsado { get; set; }

        public List<CalculoLandedCostDetalleDto> Detalles { get; set; } = new List<CalculoLandedCostDetalleDto>();
    }
}