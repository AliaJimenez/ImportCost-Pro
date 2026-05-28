using ImportCostPro.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportCostPro.Core.Interfaces
{
    public interface  IProveedorService
    {
        
        Task<IEnumerable<ProveedorDto>> GetAllAsync();
        Task<IEnumerable<ProveedorDto>> GetActivosAsync();
        Task<ProveedorDto> GetByIdAsync(int id);
        Task<ProveedorDto> CreateAsync(ProveedorDto proveedorDto);
        Task<ProveedorDto> UpdateAsync(ProveedorDto proveedorDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> HasOrdersAsync(int proveedorId);
        Task<bool> CanDeleteAsync(int proveedorId);
        Task<bool> CanEditPaisMonedaAsync(int proveedorId);
        Task<string> GetDeleteErrorMessageAsync(int proveedorId);
    }
}
