using ImportCostPro.Core.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImportCostPro.Core.Interfaces
{
    public interface IImportadorService
    {
        Task<List<ImportadorDto>> GetAllAsync();
        Task<List<ImportadorDto>> GetActivosAsync();
        Task<ImportadorDto?> GetByIdAsync(int id);
        Task<ImportadorDto> CreateAsync(ImportadorDto importadorDto);
        Task<ImportadorDto> UpdateAsync(ImportadorDto importadorDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByRncAsync(string rnc, int? excludeId = null);
        Task<bool> HasOrdersAsync(int importadorId);
        Task<bool> CanDeleteAsync(int importadorId);
        Task<string> GetDeleteErrorMessageAsync(int importadorId);
    }
}