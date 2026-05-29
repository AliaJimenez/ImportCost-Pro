using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Core.ViewModels.OrdenGasto
{
    public class OrdenGastoFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La orden es requerida.")]
        [Display(Name = "Orden de importación")]
        public int OrdenImportacionId { get; set; }

        [Required(ErrorMessage = "El tipo de gasto es requerido.")]
        [Display(Name = "Tipo de gasto")]
        public string TipoGasto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El monto es requerido.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "El monto debe ser mayor que 0.")]
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La moneda es requerida.")]
        [Display(Name = "Moneda")]
        public int MonedaId { get; set; }

        [Required(ErrorMessage = "El método de distribución es requerido.")]
        [Display(Name = "Método de distribución")]
        public string MetodoDistribucion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha del gasto es requerida.")]
        [Display(Name = "Fecha del gasto")]
        public DateTime FechaGasto { get; set; } = DateTime.Today;

        public string NumeroOrden { get; set; } = string.Empty;

        public SelectList? MonedasDisponibles { get; set; }
        public SelectList? TiposGastoDisponibles { get; set; }
        public SelectList? MetodosDistribucionDisponibles { get; set; }
    }
}
