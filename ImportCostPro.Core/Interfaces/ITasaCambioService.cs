using ImportCostPro.Core.Dtos;

namespace ImportCostPro.Core.Interfaces
{
    public interface ITasaCambioService
    {
        Task<List<TasaCambioDto>> ObtenerTodasAsync();
        Task<TasaCambioDto?> ObtenerPorIdAsync(int id);
        Task<(bool exito, string mensaje)> CrearAsync(TasaCambioDto dto);
        Task<(bool exito, string mensaje)> EditarAsync(TasaCambioDto dto);
        Task<(bool exito, string mensaje)> EliminarAsync(int id);
    }
}