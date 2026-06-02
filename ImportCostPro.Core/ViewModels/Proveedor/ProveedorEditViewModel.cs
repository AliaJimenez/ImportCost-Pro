using ImportCostPro.Core.ViewModels.Proveedor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImportCostPro.Core.ViewModels.Proveedor
{
    public class ProveedorEditViewModel : ProveedorFormViewModel
    {
        public bool TieneOrdenes { get; set; }
        public string? NombrePais { get; set; }
        public string? NombreMoneda { get; set; }
    }
}