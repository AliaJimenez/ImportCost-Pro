using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.ViewModels
{
    public class TasaCambioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La moneda de origen es obligatoria.")]
        [Display(Name = "Moneda Origen")]
        public int MonedaOrigenId { get; set; }

        [Required(ErrorMessage = "La moneda de destino es obligatoria.")]
        [Display(Name = "Moneda Destino")]
        public int MonedaDestinoId { get; set; }

        [Required(ErrorMessage = "La tasa de conversión es obligatoria.")]
        [Range(0.000001, double.MaxValue, ErrorMessage = "La tasa debe ser mayor a 0.")]
        [Display(Name = "Tasa de Conversión")]
        public decimal Tasa { get; set; }

        [Required(ErrorMessage = "La fecha de vigencia es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Vigencia")]
        public DateTime FechaVigencia { get; set; } = DateTime.Now;

        [Display(Name = "¿Está activa?")]
        public bool Activo { get; set; } = true;
        
        // Propiedades auxiliares para mostrar los nombres en las tablas
        public string? NombreMonedaOrigen { get; set; }
        public string? NombreMonedaDestino { get; set; }
    }
}