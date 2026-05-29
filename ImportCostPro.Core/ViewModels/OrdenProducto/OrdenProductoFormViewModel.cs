using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Core.ViewModels.OrdenProducto
{
    public class OrdenProductoFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La orden es requerida.")]
        [Display(Name = "Orden de importación")]
        public int OrdenImportacionId { get; set; }

        [Required(ErrorMessage = "El producto es requerido.")]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0.")]
        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario FOB es requerido.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
        [Display(Name = "Precio unitario FOB")]
        public decimal PrecioUnitarioFOB { get; set; }

        [Required(ErrorMessage = "El margen de ganancia es requerido.")]
        [Range(0, 99.99, ErrorMessage = "El margen debe ser mayor o igual a 0 y menor que 100.")]
        [Display(Name = "Margen de ganancia deseado (%)")]
        public decimal MargenGananciaDeseado { get; set; }

        // Para mostrar info de la orden en la vista
        public string NumeroOrden { get; set; } = string.Empty;
        public string MonedaOrden { get; set; } = string.Empty;
        public string SimboloMoneda { get; set; } = string.Empty;

        public SelectList? ProductosDisponibles { get; set; }
    }
}
