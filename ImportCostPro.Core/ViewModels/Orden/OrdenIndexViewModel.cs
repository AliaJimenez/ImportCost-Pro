using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportCostPro.Core.ViewModels.Orden
{
    public class OrdenIndexViewModel
    {
        public int Id { get; set; }
        public string NumeroOrden { get; set; }
        public string NombreImportador { get; set; }
        public string NombreProveedor { get; set; }
        public string NombrePais { get; set; }
        public string NombreMoneda { get; set; }
        public string Estado { get; set; }
        public decimal? PrecioSugerido { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
