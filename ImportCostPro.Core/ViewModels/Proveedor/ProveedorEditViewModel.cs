using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportCostPro.Core.ViewModels.Proveedor
{
    
        public class ProveedorEditViewModel
        {
            public int Id { get; set; }

            [Required]
            [StringLength(150)]
            public required string Nombre { get; set; }
            [Required]
            public int PaisOrigenId { get; set; }
            [Required]
            public int MonedaPrincipalId { get; set; }
            [EmailAddress]
            public required string Email { get; set; }
            [Phone]
            public required string Telefono { get; set; }
            [StringLength(300)]
            public required string Direccion { get; set; }

            public bool Activo { get; set; } = true;
            public bool TieneOrdenes { get; set; }
            public  required string NombrePais { get; set; }
            public required string NombreMoneda { get; set; }

            // Listas p
           // public List<SelectListItem> Paises { get; set; } = new(); //pendiente waldin
            //public List<SelectListItem> Monedas { get; set; } = new(); // monedas aliandy 
        }
    
}
