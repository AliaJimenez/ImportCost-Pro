using ImportCostPro.Core.Dtos;

namespace ImportCostPro.Core.Interfaces
{
    public interface IOrdenGastoService
    {
        Task<List<OrdenGastoDto>> ObtenerPorOrdenAsync(int ordenId);
        Task<OrdenGastoDto?> ObtenerPorIdAsync(int id);
        Task<(bool exito, string mensaje)> RegistrarAsync(OrdenGastoDto dto);
        Task<(bool exito, string mensaje)> EditarAsync(OrdenGastoDto dto);
        Task<(bool exito, string mensaje)> EliminarAsync(int id);

        // Para llenar el select de órdenes disponibles
        Task<List<OrdenImportacionDto>> ObtenerOrdenesAbiertasAsync();
    }
}
