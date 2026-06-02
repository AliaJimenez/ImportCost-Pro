using System;

namespace ImportCostPro.Core.ViewModels.Orden
{
    public class OrdenEditViewModel : OrdenFormViewModel
    {
        public string? Estado { get; set; }
        public decimal? CostoFOB { get; set; }
        public decimal? CIF { get; set; }
        public decimal? Arancel { get; set; }
        public decimal? ITBIS { get; set; }
        public decimal? PrecioSugerido { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaCierre { get; set; }
    }
}