using System.Threading.Tasks;
using ImportCostPro.Core.Dtos;

namespace ImportCostPro.Core.Interfaces
{
    public interface IConfiguracionImpuestoService
    {
        Task<ConfiguracionImpuestoDto> GetAsync();
        Task<ConfiguracionImpuestoDto> UpdateAsync(ConfiguracionImpuestoDto configDto);
    }
}