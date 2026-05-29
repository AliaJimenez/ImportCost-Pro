using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

    namespace ImportCostPro.Core.ViewModels.Orden
    {
        public class OrdenEditViewModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "El número de orden es obligatorio")]
            [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
            public string NumeroOrden { get; set; }

            [Required(ErrorMessage = "El importador es obligatorio")]
            public int ImportadorId { get; set; }

            [Required(ErrorMessage = "El proveedor es obligatorio")]
            public int ProveedorId { get; set; }

            [Required(ErrorMessage = "El país es obligatorio")]
            public int PaisOrigenId { get; set; }

            [Required(ErrorMessage = "La moneda es obligatoria")]
            public int MonedaId { get; set; }

            public string Estado { get; set; }  // Abierta, Calculada, Cerrada, Cancelada
            public bool Activo { get; set; } = true;

            // Datos calculados (solo lectura)
            public decimal? CostoFOB { get; set; }
            public decimal? CIF { get; set; }
            public decimal? Arancel { get; set; }
            public decimal? ITBIS { get; set; }
            public decimal? PrecioSugerido { get; set; }

            public DateTime FechaCreacion { get; set; }
            public DateTime? FechaCierre { get; set; }

            // Selectores
            public List<SelectListItem> Importadores { get; set; } = new();
            public List<SelectListItem> Proveedores { get; set; } = new();
            public List<SelectListItem> Paises { get; set; } = new();
            public List<SelectListItem> Monedas { get; set; } = new();
        }
    }

