using ImportCostPro.Core.Dtos;

namespace ImportCostPro.Core.ViewModels.OrdenGasto
{
    public class OrdenGastoIndexViewModel
    {
        public int OrdenImportacionId { get; set; }
        public string NumeroOrden { get; set; } = string.Empty;
        public string EstadoOrden { get; set; } = string.Empty;
        public bool OrdenPermiteModificaciones { get; set; }

        public List<OrdenGastoDto> Gastos { get; set; } = new();
    }
}