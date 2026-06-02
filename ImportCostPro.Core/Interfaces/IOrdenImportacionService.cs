using ImportCostPro.Core.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImportCostPro.Core.Interfaces
{
    public interface IOrdenImportacionService
    {
        Task<List<OrdenImportacionDto>> GetAllAsync();
        Task<List<OrdenImportacionDto>> GetActivasAsync();
        Task<OrdenImportacionDto?> GetByIdAsync(int id);
        Task<OrdenImportacionDto> CreateAsync(OrdenImportacionDto ordenDto);
        Task<OrdenImportacionDto> UpdateAsync(OrdenImportacionDto ordenDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> CanEditAsync(int ordenId);
        Task<bool> CanDeleteAsync(int ordenId);
        Task<bool> CloseOrderAsync(int ordenId);
        Task<bool> ExistsByNumeroOrdenAsync(string numeroOrden, int? excludeId = null);
        Task<string> GetDeleteErrorMessageAsync(int ordenId);
    }
}