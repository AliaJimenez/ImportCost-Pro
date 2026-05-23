using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ImportCostPro.Data.Contexts;
using ImportCostPro.Data.Entities;
using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;

namespace ImportCostPro.Core.Services
{
    public class TasaCambioService : ITasaCambioService
    {
        private readonly ImportCostDbContext _context;

        public TasaCambioService(ImportCostDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TasaCambioDto>> GetAllAsync()
        {
            var tasas = await _context.TasasCambio
                .Include(t => t.MonedaOrigen)
                .Include(t => t.MonedaDestino)
                .OrderByDescending(t => t.FechaVigencia)
                .ToListAsync();
            
            return MapToDto(tasas);
        }

        public async Task<IEnumerable<TasaCambioDto>> GetActivasAsync()
        {
            var tasas = await _context.TasasCambio
                .Where(t => t.Activo)
                .Include(t => t.MonedaOrigen)
                .Include(t => t.MonedaDestino)
                .OrderByDescending(t => t.FechaVigencia)
                .ToListAsync();
            
            return MapToDto(tasas);
        }

        public async Task<TasaCambioDto> GetByIdAsync(int id)
        {
            var tasa = await _context.TasasCambio
                .Include(t => t.MonedaOrigen)
                .Include(t => t.MonedaDestino)
                .FirstOrDefaultAsync(t => t.Id == id);
            
            return tasa == null ? null : MapToDto(tasa);
        }

        public async Task<TasaCambioDto> CreateAsync(TasaCambioDto tasaDto)
        {
            var tasa = new TasaCambio
            {
                MonedaOrigenId = tasaDto.MonedaOrigenId,
                MonedaDestinoId = tasaDto.MonedaDestinoId,
                Tasa = tasaDto.Tasa,
                FechaVigencia = tasaDto.FechaVigencia,
                Activo = tasaDto.Activo,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };
            
            _context.TasasCambio.Add(tasa);
            await _context.SaveChangesAsync();
            
            tasa = await _context.TasasCambio
                .Include(t => t.MonedaOrigen)
                .Include(t => t.MonedaDestino)
                .FirstAsync(t => t.Id == tasa.Id);
            
            return MapToDto(tasa);
        }

        public async Task<TasaCambioDto> UpdateAsync(TasaCambioDto tasaDto)
        {
            var tasa = await _context.TasasCambio.FindAsync(tasaDto.Id);
            if (tasa == null)
                throw new Exception("Tasa de cambio no encontrada");
            
            tasa.MonedaOrigenId = tasaDto.MonedaOrigenId;
            tasa.MonedaDestinoId = tasaDto.MonedaDestinoId;
            tasa.Tasa = tasaDto.Tasa;
            tasa.FechaVigencia = tasaDto.FechaVigencia;
            tasa.Activo = tasaDto.Activo;
            tasa.FechaModificacion = DateTime.Now;
            
            _context.TasasCambio.Update(tasa);
            await _context.SaveChangesAsync();
            
            tasa = await _context.TasasCambio
                .Include(t => t.MonedaOrigen)
                .Include(t => t.MonedaDestino)
                .FirstAsync(t => t.Id == tasa.Id);
            
            return MapToDto(tasa);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tasa = await _context.TasasCambio.FindAsync(id);
            if (tasa == null)
                return false;
            
            _context.TasasCambio.Remove(tasa);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<decimal> GetTasaVigenteAsync(int monedaOrigenId, int monedaDestinoId, DateTime fecha)
        {
            var tasa = await _context.TasasCambio
                .Where(t => t.MonedaOrigenId == monedaOrigenId
                    && t.MonedaDestinoId == monedaDestinoId
                    && t.FechaVigencia <= fecha
                    && t.Activo)
                .OrderByDescending(t => t.FechaVigencia)
                .FirstOrDefaultAsync();
            
            if (tasa == null)
                throw new Exception($"No hay tasa vigente para {monedaOrigenId} a {monedaDestinoId} en {fecha:dd/MM/yyyy}");
            
            return tasa.Tasa;
        }

        public async Task<TasaCambioDto> GetTasaVigenteDtoAsync(int monedaOrigenId, int monedaDestinoId, DateTime fecha)
        {
            var tasa = await _context.TasasCambio
                .Where(t => t.MonedaOrigenId == monedaOrigenId
                    && t.MonedaDestinoId == monedaDestinoId
                    && t.FechaVigencia <= fecha
                    && t.Activo)
                .Include(t => t.MonedaOrigen)
                .Include(t => t.MonedaDestino)
                .OrderByDescending(t => t.FechaVigencia)
                .FirstOrDefaultAsync();
            
            return tasa == null ? null : MapToDto(tasa);
        }

        public async Task<bool> CanDeleteAsync(int tasaId)
        {
            // Por ahora retorna true (para futuro: verificar si está en órdenes)
            return true;
        }

        public async Task<string> GetDeleteErrorMessageAsync(int tasaId)
        {
            return "No se puede eliminar esta tasa de cambio.";
        }

        private IEnumerable<TasaCambioDto> MapToDto(IEnumerable<TasaCambio> tasas)
        {
            return tasas.Select(t => new TasaCambioDto
            {
                Id = t.Id,
                MonedaOrigenId = t.MonedaOrigenId,
                NombreMonedaOrigen = $"{t.MonedaOrigen.Nombre} ({t.MonedaOrigen.CodigoISO})",
                MonedaDestinoId = t.MonedaDestinoId,
                NombreMonedaDestino = $"{t.MonedaDestino.Nombre} ({t.MonedaDestino.CodigoISO})",
                Tasa = t.Tasa,
                FechaVigencia = t.FechaVigencia,
                Activo = t.Activo,
                FechaCreacion = t.FechaCreacion
            });
        }

        private TasaCambioDto MapToDto(TasaCambio tasa)
        {
            return new TasaCambioDto
            {
                Id = tasa.Id,
                MonedaOrigenId = tasa.MonedaOrigenId,
                NombreMonedaOrigen = $"{tasa.MonedaOrigen.Nombre} ({tasa.MonedaOrigen.CodigoISO})",
                MonedaDestinoId = tasa.MonedaDestinoId,
                NombreMonedaDestino = $"{tasa.MonedaDestino.Nombre} ({tasa.MonedaDestino.CodigoISO})",
                Tasa = tasa.Tasa,
                FechaVigencia = tasa.FechaVigencia,
                Activo = tasa.Activo,
                FechaCreacion = tasa.FechaCreacion
            };
        }
    }
}
