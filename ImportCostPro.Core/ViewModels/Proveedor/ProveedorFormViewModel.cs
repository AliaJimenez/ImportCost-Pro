using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportCostPro.Core.ViewModels.Proveedor
{
    public class ProveedorFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
        public string ? Nombre { get; set; }

        [Required(ErrorMessage = "El país es obligatorio")]
        public int PaisOrigenId { get; set; }

        [Required(ErrorMessage = "La moneda es obligatoria")]
        public int MonedaPrincipalId { get; set; }

        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string ? Contacto { get; set; }

        [EmailAddress(ErrorMessage = "Email no válido")]
        [StringLength(100)]
        public string ? Email { get; set; }

        [Phone(ErrorMessage = "Teléfono no válido")]
        [StringLength(20)]
        public string ? Telefono { get; set; }

        [StringLength(300, ErrorMessage = "Máximo 300 caracteres")]
        public string ? Direccion { get; set; }

        public bool Activo { get; set; } = true;

    
        public List<SelectListItem> Paises { get; set; } = new();
        public List<SelectListItem> Monedas { get; set; } = new();
    }
}
