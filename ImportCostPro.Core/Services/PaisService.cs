using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Data.Contexts;
using ImportCostPro.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{

    public class PaisService(ImportCostDbContext context) : IPaisService
    {
        public async Task<IEnumerable<PaisDto>> GetAllAsync()
        {
            var paises = await context.Paises
                .OrderBy(p => p.Nombre)
                .ToListAsync();
            return MapToDto(paises);
        }

        public async Task<IEnumerable<PaisDto>> GetActivosAsync()
        {
            var paises = await context.Paises
                .Where(p => p.Activo)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
            return MapToDto(paises);
        }

       
        public async Task<PaisDto?> GetByIdAsync(int id)
        {
            var pais = await context.Paises.FindAsync(id);
        
            return pais is null ? null : MapToDto(pais);
        }

        public async Task<PaisDto> CreateAsync(PaisDto paisDto)
        {
            if (await ExistsByCodigoISOAsync(paisDto.CodigoISO))
                throw new Exception("El código ISO ya existe");

            var pais = new Pais
            {
                Nombre = paisDto.Nombre,
                CodigoISO = paisDto.CodigoISO.ToUpper(),
                Activo = paisDto.Activo,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };

            context.Paises.Add(pais);
            await context.SaveChangesAsync();
            return MapToDto(pais);
        }

        public async Task<PaisDto> UpdateAsync(PaisDto paisDto)
        {
            var pais = await context.Paises.FindAsync(paisDto.Id)
                ?? throw new Exception("País no encontrado");

            if (await ExistsByCodigoISOAsync(paisDto.CodigoISO, paisDto.Id))
                throw new Exception("El código ISO ya existe");

            pais.Nombre = paisDto.Nombre;
            pais.CodigoISO = paisDto.CodigoISO.ToUpper();
            pais.Activo = paisDto.Activo;
            pais.FechaModificacion = DateTime.Now;

            context.Paises.Update(pais);
            await context.SaveChangesAsync();
            return MapToDto(pais);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pais = await context.Paises.FindAsync(id);
            if (pais is null)
                return false;

            context.Paises.Remove(pais);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByCodigoISOAsync(string codigoISO, int? excludeId = null)
        {
            var codigoUpperCase = codigoISO.ToUpper();

            var query = context.Paises
                .Where(p => p.CodigoISO == codigoUpperCase);

            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId);

            return await query.AnyAsync();
        }

        public async Task<bool> CanDeleteAsync(int paisId)
        {
            bool enProveedores = await context.Proveedores
                .AnyAsync(p => p.PaisOrigenId == paisId);

            bool enProductos = await context.Productos
                .AnyAsync(p => p.PaisOrigenId == paisId);

            return !enProveedores && !enProductos;
        }

        public async Task<string> GetDeleteErrorMessageAsync(int paisId)
        {
            if (await context.Proveedores.AnyAsync(p => p.PaisOrigenId == paisId))
                return "Este país está asignado a proveedores.";

            if (await context.Productos.AnyAsync(p => p.PaisOrigenId == paisId))
                return "Este país está asignado a productos.";

            return "No se puede eliminar este país.";
        }

 
        private static List<PaisDto> MapToDto(IEnumerable<Pais> paises)
        {
            return paises.Select(p => new PaisDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                CodigoISO = p.CodigoISO,
                Activo = p.Activo,
                FechaCreacion = p.FechaCreacion,
                FechaModificacion = p.FechaModificacion
            }).ToList();
        }

      
        private static PaisDto MapToDto(Pais pais)
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