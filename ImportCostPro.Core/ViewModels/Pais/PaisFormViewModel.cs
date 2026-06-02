using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ImportCostPro.Core.ViewModels.Pais
{
    public class PaisFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El código ISO es obligatorio")]
        [StringLength(3, MinimumLength = 2, ErrorMessage = "Entre 2-3 caracteres")]
        public  required string CodigoISO { get; set; }

        public bool Activo { get; set; } = true;
    }
}
