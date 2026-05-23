using Microsoft.EntityFrameworkCore;
using ImportCostPro.Data.Entities;
using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Data.Contexts;

namespace ImportCostPro.Core.Services
{
    public class MonedaService : IMonedaService
    {
        private readonly ImportCostDbContext _context;

        public MonedaService(ImportCostDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MonedaDto>> GetAllAsync()
        {
            var monedas = await _context.Monedas
                .OrderBy(m => m.Nombre)
                .ToListAsync();
            
            return MapToDto(monedas);
        }

        public async Task<IEnumerable<MonedaDto>> GetActivasAsync()
        {
            var monedas = await _context.Monedas
                .Where(m => m.Activo)
                .OrderBy(m => m.Nombre)
                .ToListAsync();
            
            return MapToDto(monedas);
        }

        public async Task<MonedaDto> GetByIdAsync(int id)
        {
            var moneda = await _context.Monedas.FindAsync(id);
            return moneda == null ? null : MapToDto(moneda);
        }

        public async Task<MonedaDto> CreateAsync(MonedaDto monedaDto)
        {
            // Validar que no exista el código ISO
            if (await ExistsByCodigoISOAsync(monedaDto.CodigoISO))
                throw new Exception("El código ISO ya existe");
            
            // Si es moneda local, verificar que no haya otra activa
            if (monedaDto.EsMonedaLocal)
            {
                var otraLocal = await _context.Monedas
                    .FirstOrDefaultAsync(m => m.EsMonedaLocal && m.Activo);
                
                if (otraLocal != null)
                    throw new Exception("Solo puede haber una moneda local activa");
            }
            
            var moneda = new Moneda
            {
                CodigoISO = monedaDto.CodigoISO,
                Nombre = monedaDto.Nombre,
                Simbolo = monedaDto.Simbolo,
                EsMonedaLocal = monedaDto.EsMonedaLocal,
                Activo = monedaDto.Activo,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };
            
            _context.Monedas.Add(moneda);
            await _context.SaveChangesAsync();
            
            return MapToDto(moneda);
        }

        public async Task<MonedaDto> UpdateAsync(MonedaDto monedaDto)
        {
            var moneda = await _context.Monedas.FindAsync(monedaDto.Id);
            if (moneda == null)
                throw new Exception("Moneda no encontrada");
            
            // Validar código ISO único
            if (await ExistsByCodigoISOAsync(monedaDto.CodigoISO, monedaDto.Id))
                throw new Exception("El código ISO ya existe");
            
            // Si es moneda local, validar
            if (monedaDto.EsMonedaLocal && !moneda.EsMonedaLocal)
            {
                var otraLocal = await _context.Monedas
                    .FirstOrDefaultAsync(m => m.EsMonedaLocal && m.Activo && m.Id != monedaDto.Id);
                
                if (otraLocal != null)
                    throw new Exception("Solo puede haber una moneda local activa");
            }
            
            moneda.CodigoISO = monedaDto.CodigoISO;
            moneda.Nombre = monedaDto.Nombre;
            moneda.Simbolo = monedaDto.Simbolo;
            moneda.EsMonedaLocal = monedaDto.EsMonedaLocal;
            moneda.Activo = monedaDto.Activo;
            moneda.FechaModificacion = DateTime.Now;
            
            _context.Monedas.Update(moneda);
            await _context.SaveChangesAsync();
            
            return MapToDto(moneda);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var moneda = await _context.Monedas.FindAsync(id);
            if (moneda == null)
                return false;
            
            _context.Monedas.Remove(moneda);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> ExistsByCodigoISOAsync(string codigoISO, int? excludeId = null)
        {
            var query = _context.Monedas
                .Where(m => m.CodigoISO == codigoISO);
            
            if (excludeId.HasValue)
                query = query.Where(m => m.Id != excludeId);
            
            return await query.AnyAsync();
        }

        public async Task<int?> GetMonedaLocalActivaAsync()
        {
            var monedaLocal = await _context.Monedas
                .FirstOrDefaultAsync(m => m.EsMonedaLocal && m.Activo);
            
            return monedaLocal?.Id;
        }

        public async Task<bool> IsMonedaLocalAsync(int monedaId)
        {
            var moneda = await _context.Monedas.FindAsync(monedaId);
            return moneda?.EsMonedaLocal ?? false;
        }

        public async Task<bool> CanDeleteAsync(int monedaId)
        {
            // Verificar si está en tasas de cambio
            bool enTasas = await _context.TasasCambio
                .AnyAsync(t => (t.MonedaOrigenId == monedaId || t.MonedaDestinoId == monedaId) && t.Activo);
            
            return !enTasas;
        }

        public async Task<string> GetDeleteErrorMessageAsync(int monedaId)
        {
            if (await _context.TasasCambio
                .AnyAsync(t => (t.MonedaOrigenId == monedaId || t.MonedaDestinoId == monedaId)))
            {
                return "Esta moneda está usada en tasas de cambio y no puede ser eliminada.";
            }
            
            return "No se puede eliminar esta moneda.";
        }

        private IEnumerable<MonedaDto> MapToDto(IEnumerable<Moneda> monedas)
        {
            return monedas.Select(m => new MonedaDto
            {
                Id = m.Id,
                CodigoISO = m.CodigoISO,
                Nombre = m.Nombre,
                Simbolo = m.Simbolo,
                EsMonedaLocal = m.EsMonedaLocal,
                Activo = m.Activo,
                FechaCreacion = m.FechaCreacion,
                FechaModificacion = m.FechaModificacion
            });
        }

        private MonedaDto MapToDto(Moneda moneda)
        {
            return new MonedaDto
            {
                Id = moneda.Id,
                CodigoISO = moneda.CodigoISO,
                Nombre = moneda.Nombre,
                Simbolo = moneda.Simbolo,
                EsMonedaLocal = moneda.EsMonedaLocal,
                Activo = moneda.Activo,
                FechaCreacion = moneda.FechaCreacion,
                FechaModificacion = moneda.FechaModificacion
            };
        }
    }
}