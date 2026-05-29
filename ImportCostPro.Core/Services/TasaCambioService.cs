using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Data.Contexts;
using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class TasaCambioService : ITasaCambioService
    {
        private readonly ImportCostDbContext _context;

        public TasaCambioService(ImportCostDbContext context)
        {
            _context = context;
        }

        public async Task<List<TasaCambioDto>> ObtenerTodasAsync()
        {
            return await _context.TasasCambio
                .Include(t => t.MonedaOrigen)
                .Include(t => t.MonedaDestino)
                .OrderByDescending(t => t.FechaVigencia)
                .Select(t => new TasaCambioDto
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
                })
                .ToListAsync();
        }

        public async Task<TasaCambioDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _context.TasasCambio
                .Include(t => t.MonedaOrigen)
                .Include(t => t.MonedaDestino)
                .FirstOrDefaultAsync(t => t.Id == id);
                
            if (entidad == null) return null;

            return new TasaCambioDto
            {
                Id = entidad.Id,
                MonedaOrigenId = entidad.MonedaOrigenId,
                NombreMonedaOrigen = entidad.MonedaOrigen.Nombre,
                MonedaDestinoId = entidad.MonedaDestinoId,
                NombreMonedaDestino = entidad.MonedaDestino.Nombre,
                Tasa = entidad.Tasa,
                FechaVigencia = entidad.FechaVigencia,
                Activo = entidad.Activo,
                FechaCreacion = entidad.FechaCreacion
            };
        }

        public async Task<(bool exito, string mensaje)> CrearAsync(TasaCambioDto dto)
        {
            if (dto.MonedaOrigenId == dto.MonedaDestinoId)
                return (false, "La moneda de origen y la de destino no pueden ser la misma.");

            // Corrección: Valida duplicados SOLO si la tasa está activa
            var duplicado = await _context.TasasCambio.AnyAsync(t => 
                t.Activo && 
                t.MonedaOrigenId == dto.MonedaOrigenId && 
                t.MonedaDestinoId == dto.MonedaDestinoId && 
                t.FechaVigencia.Date == dto.FechaVigencia.Date);

            if (duplicado)
                return (false, "Ya existe una tasa de cambio ACTIVA para estas monedas en la fecha seleccionada.");

            var entidad = new TasaCambio
            {
                MonedaOrigenId = dto.MonedaOrigenId,
                MonedaDestinoId = dto.MonedaDestinoId,
                Tasa = dto.Tasa,
                FechaVigencia = dto.FechaVigencia,
                Activo = dto.Activo,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };

            _context.TasasCambio.Add(entidad);
            await _context.SaveChangesAsync();

            return (true, "Tasa de cambio creada exitosamente.");
        }

        public async Task<(bool exito, string mensaje)> EditarAsync(TasaCambioDto dto)
        {
            var entidad = await _context.TasasCambio.FindAsync(dto.Id);
            if (entidad == null) return (false, "Tasa de cambio no encontrada.");

            if (dto.MonedaOrigenId == dto.MonedaDestinoId)
                return (false, "La moneda de origen y la de destino no pueden ser la misma.");

            // Corrección: Validación de uso en un cálculo oficial existente
            var usada = await _context.CalculosLandedCost
                .Include(c => c.OrdenImportacion)
                    .ThenInclude(o => o.Gastos)
                .AnyAsync(c => 
                    (c.OrdenImportacion.MonedaId == entidad.MonedaOrigenId || 
                     c.OrdenImportacion.Gastos.Any(g => g.MonedaId == entidad.MonedaOrigenId)) &&
                    c.FechaCalculo >= entidad.FechaVigencia);

            if (usada)
            {
                // Solo permitimos cambiar el estado. Si intenta cambiar algo más, bloqueamos.
                if (entidad.MonedaOrigenId != dto.MonedaOrigenId ||
                    entidad.MonedaDestinoId != dto.MonedaDestinoId ||
                    entidad.Tasa != dto.Tasa ||
                    entidad.FechaVigencia.Date != dto.FechaVigencia.Date)
                {
                    return (false, "No se pueden modificar los campos críticos (Monedas, Tasa o Fecha) porque esta tasa ya fue utilizada en un cálculo oficial de Landed Cost. Solo puede cambiar su estado (Activo/Inactivo).");
                }
            }

            entidad.MonedaOrigenId = dto.MonedaOrigenId;
            entidad.MonedaDestinoId = dto.MonedaDestinoId;
            entidad.Tasa = dto.Tasa;
            entidad.FechaVigencia = dto.FechaVigencia;
            entidad.Activo = dto.Activo;
            entidad.FechaModificacion = DateTime.Now;

            await _context.SaveChangesAsync();
            return (true, "Tasa de cambio actualizada exitosamente.");
        }

        public async Task<(bool exito, string mensaje)> EliminarAsync(int id)
        {
            var entidad = await _context.TasasCambio.FindAsync(id);
            if (entidad == null) return (false, "Tasa de cambio no encontrada.");

            // Corrección: Validación de uso para impedir eliminación
            var usada = await _context.CalculosLandedCost
                .Include(c => c.OrdenImportacion)
                    .ThenInclude(o => o.Gastos)
                .AnyAsync(c => 
                    (c.OrdenImportacion.MonedaId == entidad.MonedaOrigenId || 
                    c.OrdenImportacion.Gastos.Any(g => g.MonedaId == entidad.MonedaOrigenId)) &&
                    c.FechaCalculo >= entidad.FechaVigencia);

            if (usada) 
                return (false, "No se puede eliminar: esta tasa ya se usó en un cálculo oficial de Landed Cost. Por favor, edítela y cambie su estado a inactivo.");

            _context.TasasCambio.Remove(entidad);
            await _context.SaveChangesAsync();

            return (true, "Tasa de cambio eliminada exitosamente.");
        }
    }
}