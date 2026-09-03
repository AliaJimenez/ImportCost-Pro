using System;

namespace ImportCostPro.Core.Dtos
{
    public class OrdenImportacionDto
    {
        public int Id { get; set; }
        public required string NumeroOrden { get; set; }
        public required int ImportadorId { get; set; }
        public string? NombreImportador { get; set; }

        public required int ProveedorId { get; set; }
        public string? NombreProveedor { get; set; }

        public required int PaisOrigenId { get; set; }
        public string? NombrePais { get; set; }

        public required int MonedaId { get; set; }
        public string? NombreMoneda { get; set; }

        public required DateTime FechaOrden { get; set; }
        public required string ModalidadTransporte { get; set; }

        public required string Estado { get; set; }

        public decimal? CostoFOB { get; set; }
        public decimal? CIF { get; set; }
        public decimal? Arancel { get; set; }
        public decimal? ImpuestoSelectivo { get; set; }
        public decimal? TasaAduanal { get; set; }
        public decimal? ITBIS { get; set; }
        public decimal? PrecioSugerido { get; set; }

        public required bool Activo { get; set; }
        public required DateTime FechaCreacion { get; set; }
        public required DateTime FechaModificacion { get; set; }
        public DateTime? FechaCierre { get; set; }
    }
}