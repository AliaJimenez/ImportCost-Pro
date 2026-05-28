using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.ViewModels
{
    public class CategoriaArancelariaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código arancelario es requerido.")]
        [MaxLength(20, ErrorMessage = "El código no puede tener más de 20 caracteres.")]
        [Display(Name = "Código arancelario")]
        public string ? CodigoArancelario { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [MaxLength(150, ErrorMessage = "El nombre no puede tener más de 150 caracteres.")]
        [Display(Name = "Nombre o descripción")]
        public string?  Nombre { get; set; }

        [Required(ErrorMessage = "El porcentaje de arancel es requerido.")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100.")]
        [Display(Name = "Porcentaje de arancel (%)")]
        public decimal PorcentajeArancel { get; set; }

        [Required(ErrorMessage = "Debe indicar si aplica ITBIS.")]
        [Display(Name = "¿Aplica ITBIS?")]
        public bool AplicaItbis { get; set; }

        [Required(ErrorMessage = "Debe indicar si aplica impuesto selectivo.")]
        [Display(Name = "¿Aplica impuesto selectivo?")]
        public bool AplicaImpuestoSelectivo { get; set; }

        [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100.")]
        [Display(Name = "Porcentaje de impuesto selectivo (%)")]
        public decimal PorcentajeImpuestoSelectivo { get; set; }

        [Required(ErrorMessage = "El estado es requerido.")]
        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        //para mostrar en la vista de detalles si la categoría arancelaria tiene productos asociados
        public bool TieneProductosAsociados { get; set; }
    }
}

