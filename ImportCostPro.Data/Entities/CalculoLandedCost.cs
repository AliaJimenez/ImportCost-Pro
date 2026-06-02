using System;
using System.Collections.Generic;

namespace ImportCostPro.Data.Entities
{
    public class CalculoLandedCost
    {
        public int Id { get; set; }
        public int OrdenImportacionId { get; set; }
        public OrdenImportacion ? OrdenImportacion { get; set; }
        
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

        // Instantánea Fiscal (Congelando la configuración del momento)
        public decimal PorcentajeTasaServicioUsado { get; set; }
        public decimal PorcentajeItbisUsado { get; set; }
        public DateTime FechaCalculo { get; set; } = DateTime.Now;

        // Relación con el detalle
        public ICollection<CalculoLandedCostDetalle> Detalles { get; set; } = new List<CalculoLandedCostDetalle>();
    }
}