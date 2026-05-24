using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.ViewModels
{
    public class ConfiguracionImpuestoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ITBIS es obligatorio.")]
        [Range(0, 100, ErrorMessage = "El valor del ITBIS debe estar entre 0 y 100.")]
        [Display(Name = "Tasa de ITBIS (%)")]
        public decimal PorcentajeITBIS { get; set; }

        [Required(ErrorMessage = "La Tasa de Servicio Aduanal es obligatoria.")]
        [Range(0, 100, ErrorMessage = "El valor de la Tasa Aduanal debe estar entre 0 y 100.")]
        [Display(Name = "Tasa de Servicio Aduanal (%)")]
        public decimal PorcentajeTasaServicioAduanal { get; set; }
    }
}