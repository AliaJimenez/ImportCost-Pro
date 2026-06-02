using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Data.Contexts;
using ImportCostPro.Data.Entities;
using System;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class PaisService : IPaisService
    {
        private readonly ImportCostDbContext _context;

        public PaisService(ImportCostDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaisDto>> GetAllAsync()
        {
            var paises = await _context.Paises
                .OrderBy(p => p.Nombre)
                .ToListAsync();
            return MapToDto(paises);
        }

        public async Task<IEnumerable<PaisDto>> GetActivosAsync()
        {
            var paises = await _context.Paises
                .Where(p => p.Activo)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
            return MapToDto(paises);
        }

        public async Task<PaisDto> GetByIdAsync(int id)
        {
            var pais = await _context.Paises.FindAsync(id);
            return pais == null ? null : MapToDto(pais);
        }

        public async Task<PaisDto> CreateAsync(PaisDto paisDto)
        {
            if (await ExistsByCodigoISOAsync(paisDto.CodigoISO))
                throw new Exception("El código ISO ya existe");

            var pais = new Pais
            {
                Nombre = paisDto.Nombre,
                CodigoISO = paisDto.CodigoISO?.ToUpper()!,
                Activo = paisDto.Activo,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };

            _context.Paises.Add(pais);
            await _context.SaveChangesAsync();
            return MapToDto(pais);
        }

        public async Task<PaisDto> UpdateAsync(PaisDto paisDto)
        {
            var pais = await _context.Paises.FindAsync(paisDto.Id);
            if (pais == null)
                throw new Exception("País no encontrado");

            if (await ExistsByCodigoISOAsync(paisDto.CodigoISO, paisDto.Id))
                throw new Exception("El código ISO ya existe");

            pais.Nombre = paisDto.Nombre;
            pais.CodigoISO = paisDto.CodigoISO?.ToUpper()!;
            pais.Activo = paisDto.Activo;
            pais.FechaModificacion = DateTime.Now;

            _context.Paises.Update(pais);
            await _context.SaveChangesAsync();
            return MapToDto(pais);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pais = await _context.Paises.FindAsync(id);
            if (pais == null)
                return false;

            _context.Paises.Remove(pais);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByCodigoISOAsync(string codigoISO, int? excludeId = null)
        {
            var codigoUpperCase = codigoISO?.ToUpper();  

            var query = _context.Paises
                .Where(p => p.CodigoISO == codigoUpperCase); 

            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId);

            return await query.AnyAsync();
        }

        public async Task<bool> CanDeleteAsync(int paisId)
        {
            bool enProveedores = await _context.Proveedores
                .AnyAsync(p => p.PaisOrigenId == paisId);

            bool enProductos = await _context.Productos
                .AnyAsync(p => p.PaisOrigenId == paisId);

            return !enProveedores && !enProductos;
        }

        public async Task<string> GetDeleteErrorMessageAsync(int paisId)
        {
            if (await _context.Proveedores.AnyAsync(p => p.PaisOrigenId == paisId))
                return "Este país está asignado a proveedores.";

            if (await _context.Productos.AnyAsync(p => p.PaisOrigenId == paisId))
                return "Este país está asignado a productos.";

            return "No se puede eliminar este país.";
        }

        private IEnumerable<PaisDto> MapToDto(IEnumerable<Pais> paises)
        {
            return paises.Select(p => new PaisDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                CodigoISO = p.CodigoISO,
                Activo = p.Activo,
                FechaCreacion = p.FechaCreacion,
                FechaModificacion = p.FechaModificacion
            });
        }

        private PaisDto MapToDto(Pais pais)
        {
            return new PaisDto
            {
                Id = pais.Id,
                Nombre = pais.Nombre,
                CodigoISO = pais.CodigoISO,
                Activo = pais.Activo,
                FechaCreacion = pais.FechaCreacion,
                FechaModificacion = pais.FechaModificacion
            };
        }
    }
}
