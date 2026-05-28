using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportCostPro.Core.Dtos
{
    public class OrdenImportacionDto
    {
        public int Id { get; set; }
        public string ? NumeroOrden { get; set; }
        public int ImportadorId { get; set; }
        public string ? NombreImportador { get; set; }

        public int ProveedorId { get; set; }
        public string ? NombreProveedor { get; set; }

        public int PaisOrigenId { get; set; }
        public string ? NombrePais { get; set; }

        public int MonedaId { get; set; }
        public string ? NombreMoneda { get; set; }

        public string ? Estado { get; set; }  // Abierta, Calculada, Cerrada, Cancelada

        public decimal? CostoFOB { get; set; }
        public decimal? CIF { get; set; }
        public decimal? Arancel { get; set; }
        public decimal? ImpuestoSelectivo { get; set; }
        public decimal? TasaAduanal { get; set; }
        public decimal? ITBIS { get; set; }
        public decimal? PrecioSugerido { get; set; }

        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public DateTime? FechaCierre { get; set; }

    }
}
