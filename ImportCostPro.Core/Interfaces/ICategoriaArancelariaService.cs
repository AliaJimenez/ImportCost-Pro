using ImportCostPro.Core.Dtos;

namespace ImportCostPro.Core.Interfaces
{
    public interface ICategoriaArancelariaService
    {
        Task<List<CategoriaArancelariaDto>> ObtenerTodasAsync();
        Task<CategoriaArancelariaDto?> ObtenerPorIdAsync(int id);
        Task<(bool exito, string mensaje)> CrearAsync(CategoriaArancelariaDto dto);
        Task<(bool exito, string mensaje)> EditarAsync(CategoriaArancelariaDto dto);
        Task<(bool exito, string mensaje)> EliminarAsync(int id);
        Task<List<CategoriaArancelariaDto>> ObtenerCategoriasActivasAsync();
    }
}
