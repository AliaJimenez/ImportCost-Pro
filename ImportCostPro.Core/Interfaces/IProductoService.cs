using ImportCostPro.Core.Dtos;

namespace ImportCostPro.Core.Interfaces
{
    public interface IProductoService
    {
        Task<List<ProductoDto>> ObtenerTodosAsync();
        Task<List<ProductoDto>> ObtenerProductosActivosAsync();
        Task<ProductoDto?> ObtenerPorIdAsync(int id);
        Task<(bool exito, string mensaje)> CrearAsync(ProductoDto dto);
        Task<(bool exito, string mensaje)> EditarAsync(ProductoDto dto);
        Task<(bool exito, string mensaje)> EliminarAsync(int id);
        

    }
}
