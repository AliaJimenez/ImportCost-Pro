using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportCostPro.Core.ViewModels.Proveedor
{
    internal class ProveedorFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El RNC es obligatorio")]
        [StringLength(20, ErrorMessage = "Maximo 20 caracteres")]
        public string Rnc { get; set; }

        [StringLength(300, ErrorMessage = "Maximo 300 caracteres")]
        public string Direccion { get; set; }

        [StringLength(100, ErrorMessage = "Maximo 100 caracteres")]
        public string Contacto { get; set; }

        [EmailAddress(ErrorMessage = "Email no válido")]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Teléfono no válido")]
        [StringLength(20)]
        public string Telefono { get; set; }

        public bool Activo { get; set; } = true;
    }
}
