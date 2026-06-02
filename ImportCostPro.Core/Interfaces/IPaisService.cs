using ImportCostPro.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportCostPro.Core.Interfaces
{
    public interface IPaisService
    {
        //waldin
        Task<IEnumerable<PaisDto>> GetAllAsync();
        Task<IEnumerable<PaisDto>> GetActivosAsync();
        Task<PaisDto?> GetByIdAsync(int id);
        Task<PaisDto> CreateAsync(PaisDto paisDto);
        Task<PaisDto> UpdateAsync(PaisDto paisDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByCodigoISOAsync(string codigoISO, int? excludeId = null);
        Task<bool> CanDeleteAsync(int paisId);
        Task<string> GetDeleteErrorMessageAsync(int paisId);
    }

}

