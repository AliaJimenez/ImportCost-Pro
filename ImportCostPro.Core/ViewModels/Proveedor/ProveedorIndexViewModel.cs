using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportCostPro.Core.ViewModels.Proveedor
{
    public class ProveedorIndexViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombrePais { get; set; }
        public string NombreMoneda { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; }
        public bool TieneOrdenes { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
