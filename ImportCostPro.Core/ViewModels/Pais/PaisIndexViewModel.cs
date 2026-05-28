using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportCostPro.Core.ViewModels.Pais
{
    public class PaisIndexViewModel
    {
        public int ? Id { get; set; }
        public required string Nombre { get; set; }
        public required string CodigoISO { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
