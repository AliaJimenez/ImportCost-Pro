using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImportCostPro.Core.Dtos;

namespace ImportCostPro.Core.Interfaces
{
    public interface IMonedaService
    {
        Task<IEnumerable<MonedaDto>> GetAllAsync();
        Task<IEnumerable<MonedaDto>> GetActivasAsync();
        Task<MonedaDto> GetByIdAsync(int id);
        Task<MonedaDto> CreateAsync(MonedaDto monedaDto);
        Task<MonedaDto> UpdateAsync(MonedaDto monedaDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByCodigoISOAsync(string codigoISO, int? excludeId = null);
        Task<int?> GetMonedaLocalActivaAsync();
        Task<bool> IsMonedaLocalAsync(int monedaId);
        Task<bool> CanDeleteAsync(int monedaId);
        Task<string> GetDeleteErrorMessageAsync(int monedaId);
    }
}