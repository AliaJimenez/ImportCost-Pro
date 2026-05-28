using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Core.ViewModels
{
    public class ProductoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es requerido.")]
        [MaxLength(150, ErrorMessage = "El nombre no puede tener más de 150 caracteres.")]
        [Display(Name = "Nombre del producto")]
        public string ? Nombre { get; set; }

        [Required(ErrorMessage = "El código o referencia es requerido.")]
        [MaxLength(50, ErrorMessage = "El código no puede tener más de 50 caracteres.")]
        [Display(Name = "Código o referencia")]
        public string ? CodigoReferencia { get; set; }

        [Required(ErrorMessage = "El país de origen es requerido.")]
        [Display(Name = "País de origen predeterminado")]
        public int PaisOrigenId { get; set; }

        [Required(ErrorMessage = "La categoría arancelaria es requerida.")]
        [Display(Name = "Categoría arancelaria")]
        public int CategoriaArancelariaId { get; set; }

        [Required(ErrorMessage = "El peso unitario es requerido.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "El peso debe ser mayor que 0.")]
        [Display(Name = "Peso unitario (kg)")]
        public decimal PesoUnitario { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "El largo debe ser mayor que 0.")]
        [Display(Name = "Largo (cm)")]
        public decimal? Largo { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "El ancho debe ser mayor que 0.")]
        [Display(Name = "Ancho (cm)")]
        public decimal? Ancho { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "El alto debe ser mayor que 0.")]
        [Display(Name = "Alto (cm)")]
        public decimal? Alto { get; set; }

        [Required(ErrorMessage = "La unidad de medida es requerida.")]
        [Display(Name = "Unidad de medida")]
        public string ? UnidadMedida { get; set; }

        [MaxLength(250, ErrorMessage = "La descripción no puede tener más de 250 caracteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El estado es requerido.")]
        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        // Para mostrar los selects en la vista
        public SelectList? PaisesDisponibles { get; set; }
        public SelectList? CategoriasDisponibles { get; set; }
        public SelectList? UnidadesMedidaDisponibles { get; set; }

        // Para controlar bloqueo en Edit
        public bool TieneOrdenesAsociadas { get; set; }
    }
}

