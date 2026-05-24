using ImportCostPro.Core.Dtos;

namespace ImportCostPro.Core.Interfaces
{
    public interface IMonedaService
    {
        Task<List<MonedaDto>> ObtenerTodasAsync();
        Task<List<MonedaDto>> ObtenerActivasAsync();
        Task<MonedaDto?> ObtenerPorIdAsync(int id);
        Task<(bool exito, string mensaje)> CrearAsync(MonedaDto dto);
        Task<(bool exito, string mensaje)> EditarAsync(MonedaDto dto);
        Task<(bool exito, string mensaje)> EliminarAsync(int id);
    }
}