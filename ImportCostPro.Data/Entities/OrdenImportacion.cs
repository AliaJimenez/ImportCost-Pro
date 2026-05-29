using System;
using System.Collections.Generic;

namespace ImportCostPro.Data.Entities
{
    public class OrdenImportacion
    {
        public int Id { get; set; }
        public string NumeroOrden { get; set; }

        // Foreign Keys
        public int ImportadorId { get; set; }
        public int ProveedorId { get; set; }
        public int PaisOrigenId { get; set; }
        public int MonedaId { get; set; }

        // Navigation Properties
        public Importador Importador { get; set; }
        public Proveedor Proveedor { get; set; }
        public Pais PaisOrigen { get; set; }
        public Moneda Moneda { get; set; }

        // Estados: Abierta, Calculada, Cerrada, Cancelada
        public string Estado { get; set; } = "Abierta";

        // Fechas
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
        public DateTime? FechaCierre { get; set; }

        // Datos de cálculo
        public decimal? CostoFOB { get; set; }
        public decimal? CIF { get; set; }
        public decimal? Arancel { get; set; }
        public decimal? ImpuestoSelectivo { get; set; }
        public decimal? TasaAduanal { get; set; }
        public decimal? ITBIS { get; set; }
        public decimal? PrecioSugerido { get; set; }

        // Activo
        public bool Activo { get; set; } = true;

        // Colecciones
        public ICollection<OrdenProducto> Productos { get; set; } = new List<OrdenProducto>();
        public ICollection<OrdenGasto> Gastos { get; set; } = new List<OrdenGasto>();
        }
}