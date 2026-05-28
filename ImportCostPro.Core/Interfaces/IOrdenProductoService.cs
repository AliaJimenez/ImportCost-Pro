using ImportCostPro.Core.Dtos;

namespace ImportCostPro.Core.Interfaces
{
    public interface IOrdenProductoService
    {
        Task<List<OrdenProductoDto>> ObtenerPorOrdenAsync(int ordenId);
        Task<OrdenProductoDto?> ObtenerPorIdAsync(int id);
        Task<(bool exito, string mensaje)> AgregarAsync(OrdenProductoDto dto);
        Task<(bool exito, string mensaje)> EditarAsync(OrdenProductoDto dto);
        Task<(bool exito, string mensaje)> EliminarAsync(int id);

        // Para el resumen FOB que se muestra en el detalle de la orden
        Task<ResumenFOBDto> ObtenerResumenFOBAsync(int ordenId);
    }
}