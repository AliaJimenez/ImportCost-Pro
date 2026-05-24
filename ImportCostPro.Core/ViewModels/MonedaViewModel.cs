using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.ViewModels
{
    public class MonedaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código ISO es obligatorio.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El código ISO debe tener exactamente 3 caracteres.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "El código ISO solo debe contener letras.")]
        [Display(Name = "Código ISO")]
        public string CodigoISO { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de la moneda es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El símbolo es obligatorio.")]
        [StringLength(5, ErrorMessage = "El símbolo no puede exceder los 5 caracteres.")]
        [Display(Name = "Símbolo (ej. $, €, RD$)")]
        public string Simbolo { get; set; } = string.Empty;

        [Display(Name = "¿Es la moneda local del sistema?")]
        public bool EsMonedaLocal { get; set; }

        [Display(Name = "¿Está activa?")]
        public bool Activo { get; set; } = true;
    }
}