using System;
using ImportCostPro.Core.Dtos;


namespace ImportCostPro.Core.Interfaces
{
  public interface IImportadorService
    {
        //waldin
        Task<IEnumerable<ImportadorDto>> GetAllAsync();
        Task<IEnumerable<ImportadorDto>> GetActivosAsync();
        Task<ImportadorDto> GetByIdAsync(int id);
        Task<ImportadorDto> CreateAsync(ImportadorDto importadorDto);
        Task<ImportadorDto> UpdateAsync(ImportadorDto importadorDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByRncAsync(string rnc, int? excludeId = null);
        Task<bool> CanDeleteAsync(int importadorId);
        Task<string> GetDeleteErrorMessageAsync(int importadorId);
    }
}
