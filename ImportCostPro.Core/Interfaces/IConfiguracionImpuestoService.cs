using ImportCostPro.Core.Dtos;

namespace ImportCostPro.Core.Interfaces
{
    public interface IConfiguracionImpuestoService
    {
        Task<ConfiguracionImpuestoDto> ObtenerConfiguracionAsync();
        Task<(bool exito, string mensaje)> ActualizarConfiguracionAsync(ConfiguracionImpuestoDto dto);
    }
}