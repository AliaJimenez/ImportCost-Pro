using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.ViewModels.Importador
{
    public class ImportadorFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El RNC es obligatorio")]
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres")]
        public required string Rnc { get; set; }

        [Required(ErrorMessage = "El país es obligatorio")]
        public required int PaisId { get; set; }

        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string? Contacto { get; set; }

        [StringLength(300, ErrorMessage = "Máximo 300 caracteres")]
        public string? Direccion { get; set; }

        [EmailAddress(ErrorMessage = "Email no válido")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Teléfono no válido")]
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres")]
        public string? Telefono { get; set; }

        public bool Activo { get; set; } = true;

        public List<SelectListItem> Paises { get; set; } = [];
    }
}