using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.ViewModels.Orden
{
    public class OrdenFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de orden es obligatorio")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public required string NumeroOrden { get; set; }

        [Required(ErrorMessage = "El importador es obligatorio")]
        public required int ImportadorId { get; set; }

        [Required(ErrorMessage = "El proveedor es obligatorio")]
        public required int ProveedorId { get; set; }

        [Required(ErrorMessage = "El país es obligatorio")]
        public required int PaisOrigenId { get; set; }

        [Required(ErrorMessage = "La moneda es obligatoria")]
        public required int MonedaId { get; set; }

        [Required(ErrorMessage = "La fecha de la orden es obligatoria")]
        [DataType(DataType.Date)]
        public required DateTime FechaOrden { get; set; }

        [Required(ErrorMessage = "La modalidad de transporte es obligatoria")]
        public required string ModalidadTransporte { get; set; }

        public bool Activo { get; set; } = true;

        public List<SelectListItem> Importadores { get; set; } = [];
        public List<SelectListItem> Proveedores { get; set; } = [];
        public List<SelectListItem> Paises { get; set; } = [];
        public List<SelectListItem> Monedas { get; set; } = [];
        public List<SelectListItem> Modalidades { get; set; } = [];
    }
}