using System;
using System.Collections.Generic;

namespace ImportCostPro.Data.Entities
{
    public class OrdenImportacion
    {
        public int Id { get; set; }
        public required string NumeroOrden { get; set; }
        public required int ImportadorId { get; set; }
        public required int ProveedorId { get; set; }
        public required int PaisOrigenId { get; set; }
        public required int MonedaId { get; set; }
        public required DateTime FechaOrden { get; set; }
        public required string ModalidadTransporte { get; set; }

        public Importador? Importador { get; set; }
        public Proveedor? Proveedor { get; set; }
        public Pais? PaisOrigen { get; set; }
        public Moneda? Moneda { get; set; }

        public required string Estado { get; set; } = "Abierta";

        public required DateTime FechaCreacion { get; set; } = DateTime.Now;
        public required DateTime FechaModificacion { get; set; } = DateTime.Now;
        public DateTime? FechaCierre { get; set; }

        public decimal? CostoFOB { get; set; }
        public decimal? CIF { get; set; }
        public decimal? Arancel { get; set; }
        public decimal? ImpuestoSelectivo { get; set; }
        public decimal? TasaAduanal { get; set; }
        public decimal? ITBIS { get; set; }
        public decimal? PrecioSugerido { get; set; }

        public required bool Activo { get; set; } = true;

        public ICollection<OrdenProducto> Productos { get; set; } = [];
        public ICollection<OrdenGasto> Gastos { get; set; } = [];
    }
}