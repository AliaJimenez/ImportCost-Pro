using ImportCostPro.Core.Dtos;

namespace ImportCostPro.Core.ViewModels.OrdenProducto
{
    public class OrdenProductoIndexViewModel
    {
        public int OrdenImportacionId { get; set; }
        public string NumeroOrden { get; set; } = string.Empty;
        public string EstadoOrden { get; set; } = string.Empty;
        public bool OrdenPermiteModificaciones { get; set; }
        public List<OrdenProductoDto> Productos { get; set; } = new();

        public ResumenFOBDto Resumen { get; set; } = new();
    }
}
