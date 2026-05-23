using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImportCostPro.Core.Dtos;

namespace ImportCostPro.Core.Interfaces
{
    public interface ITasaCambioService
    {
        Task<IEnumerable<TasaCambioDto>> GetAllAsync();
        Task<IEnumerable<TasaCambioDto>> GetActivasAsync();
        Task<TasaCambioDto> GetByIdAsync(int id);
        Task<TasaCambioDto> CreateAsync(TasaCambioDto tasaDto);
        Task<TasaCambioDto> UpdateAsync(TasaCambioDto tasaDto);
        Task<bool> DeleteAsync(int id);
        
        // Métodos especiales
        Task<decimal> GetTasaVigenteAsync(int monedaOrigenId, int monedaDestinoId, DateTime fecha);
        Task<TasaCambioDto> GetTasaVigenteDtoAsync(int monedaOrigenId, int monedaDestinoId, DateTime fecha);
        Task<bool> CanDeleteAsync(int tasaId);
        Task<string> GetDeleteErrorMessageAsync(int tasaId);
    }
}