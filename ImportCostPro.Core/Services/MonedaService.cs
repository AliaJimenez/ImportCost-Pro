using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Data.Contexts;
using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class MonedaService : IMonedaService
    {
        private readonly ImportCostDbContext _context;

        public MonedaService(ImportCostDbContext context)
        {
            _context = context;
        }

        public async Task<List<MonedaDto>> ObtenerTodasAsync()
        {
            return await _context.Monedas
                .OrderBy(m => m.Nombre)
                .Select(m => new MonedaDto
                {
                    Id = m.Id,
                    CodigoISO = m.CodigoISO,
                    Nombre = m.Nombre,
                    Simbolo = m.Simbolo,
                    EsMonedaLocal = m.EsMonedaLocal,
                    Activo = m.Activo
                })
                .ToListAsync();
        }

        public async Task<List<MonedaDto>> ObtenerActivasAsync()
        {
            return await _context.Monedas
                .Where(m => m.Activo)
                .OrderBy(m => m.Nombre)
                .Select(m => new MonedaDto
                {
                    Id = m.Id,
                    CodigoISO = m.CodigoISO,
                    Nombre = m.Nombre,
                    Simbolo = m.Simbolo,
                    EsMonedaLocal = m.EsMonedaLocal,
                    Activo = m.Activo
                })
                .ToListAsync();
        }

        public async Task<MonedaDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _context.Monedas.FirstOrDefaultAsync(m => m.Id == id);
            if (entidad == null) return null;

            return new MonedaDto
            {
                Id = entidad.Id,
                CodigoISO = entidad.CodigoISO,
                Nombre = entidad.Nombre,
                Simbolo = entidad.Simbolo,
                EsMonedaLocal = entidad.EsMonedaLocal,
                Activo = entidad.Activo
            };
        }

        public async Task<(bool exito, string mensaje)> CrearAsync(MonedaDto dto)
        {
            var isoExiste = await _context.Monedas
                .AnyAsync(m => m.CodigoISO.ToUpper() == dto.CodigoISO.Trim().ToUpper());

            if (isoExiste)
                return (false, "Ya existe una moneda con este código ISO.");

            if (dto.EsMonedaLocal)
            {
                var otraLocal = await _context.Monedas
                    .AnyAsync(m => m.EsMonedaLocal);
                
                if (otraLocal)
                    return (false, "Ya existe una moneda marcada como local. Solo puede haber una en el sistema, independientemente de su estado.");
            }

            var entidad = new Moneda
            {
                CodigoISO = dto.CodigoISO.Trim().ToUpper(),
                Nombre = dto.Nombre.Trim(),
                Simbolo = dto.Simbolo.Trim(),
                EsMonedaLocal = dto.EsMonedaLocal,
                Activo = dto.Activo,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };

            _context.Monedas.Add(entidad);
            await _context.SaveChangesAsync();

            return (true, "Moneda creada correctamente.");
        }

        public async Task<(bool exito, string mensaje)> EditarAsync(MonedaDto dto)
        {
            var entidad = await _context.Monedas.FirstOrDefaultAsync(m => m.Id == dto.Id);
            if (entidad == null)
                return (false, "Moneda no encontrada.");

            if (entidad.CodigoISO.ToUpper() != dto.CodigoISO.Trim().ToUpper())
            {
                var enUso = await _context.TasasCambio.AnyAsync(t => t.MonedaOrigenId == dto.Id || t.MonedaDestinoId == dto.Id) ||
                            await _context.Proveedores.AnyAsync(p => p.MonedaPrincipalId == dto.Id) ||
                            await _context.OrdenesImportacion.AnyAsync(o => o.MonedaId == dto.Id) ||
                            await _context.Set<OrdenGasto>().AnyAsync(g => g.MonedaId == dto.Id); 
                
                if (enUso)
                    return (false, "No se puede modificar el Código ISO porque esta moneda ya está siendo utilizada en tasas, proveedores, órdenes o gastos.");

                var isoExiste = await _context.Monedas
                    .AnyAsync(m => m.CodigoISO.ToUpper() == dto.CodigoISO.Trim().ToUpper() && m.Id != dto.Id);

                if (isoExiste)
                    return (false, "Ya existe otra moneda con este código ISO.");
            }

            if (dto.EsMonedaLocal)
            {
                var otraLocal = await _context.Monedas
                    .AnyAsync(m => m.EsMonedaLocal && m.Id != dto.Id);
                
                if (otraLocal)
                    return (false, "Ya existe otra moneda local. Solo puede haber una en el sistema.");
            }

            entidad.CodigoISO = dto.CodigoISO.Trim().ToUpper();
            entidad.Nombre = dto.Nombre.Trim();
            entidad.Simbolo = dto.Simbolo.Trim();
            entidad.EsMonedaLocal = dto.EsMonedaLocal;
            entidad.Activo = dto.Activo;
            entidad.FechaModificacion = DateTime.Now;

            await _context.SaveChangesAsync();
            return (true, "Moneda actualizada correctamente.");
        }

        public async Task<(bool exito, string mensaje)> EliminarAsync(int id)
        {
            var entidad = await _context.Monedas.FirstOrDefaultAsync(m => m.Id == id);
            if (entidad == null)
                return (false, "Moneda no encontrada.");

            if (entidad.EsMonedaLocal)
                return (false, "No se puede eliminar la moneda local del sistema. Desmárquela como local primero editándola.");

            var usadaEnTasas = await _context.TasasCambio.AnyAsync(t => t.MonedaOrigenId == id || t.MonedaDestinoId == id);
            if (usadaEnTasas) return (false, "No se puede eliminar esta moneda porque está siendo utilizada en Tasas de Cambio.");

            var usadaEnProveedores = await _context.Proveedores.AnyAsync(p => p.MonedaPrincipalId == id);
            if (usadaEnProveedores) return (false, "No se puede eliminar esta moneda porque está asignada a uno o más Proveedores.");

            var usadaEnOrdenes = await _context.OrdenesImportacion.AnyAsync(o => o.MonedaId == id);
            if (usadaEnOrdenes) return (false, "No se puede eliminar esta moneda porque está asociada a Órdenes de Importación.");

            var usadaEnGastos = await _context.Set<OrdenGasto>().AnyAsync(g => g.MonedaId == id);
            if (usadaEnGastos) return (false, "No se puede eliminar esta moneda porque está siendo utilizada en Gastos de Importación.");

            _context.Monedas.Remove(entidad);
            await _context.SaveChangesAsync();

            return (true, "Moneda eliminada correctamente.");
        }
    }
}