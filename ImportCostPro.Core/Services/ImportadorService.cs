using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Data.Contexts;
using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImportCostPro.Core.Services
{
    public class ImportadorService(ImportCostDbContext context) : IImportadorService
    {
        public async Task<List<ImportadorDto>> GetAllAsync()
        {
            var importadores = await context.Importadores
                .Include(i => i.Pais)
                .OrderBy(i => i.Nombre)
                .ToListAsync();
            return await MapToDtoAsync(importadores);
        }

        public async Task<List<ImportadorDto>> GetActivosAsync()
        {
            var importadores = await context.Importadores
                .Include(i => i.Pais)
                .Where(i => i.Activo)
                .OrderBy(i => i.Nombre)
                .ToListAsync();
            return await MapToDtoAsync(importadores);
        }

        public async Task<ImportadorDto?> GetByIdAsync(int id)
        {
            var importador = await context.Importadores
                .Include(i => i.Pais)
                .FirstOrDefaultAsync(i => i.Id == id);
            return importador is null ? null : await MapToDtoAsync(importador);
        }

        public async Task<ImportadorDto> CreateAsync(ImportadorDto importadorDto)
        {
            var rncLimpio = importadorDto.Rnc.Trim();

            if (await ExistsByRncAsync(rncLimpio))
                throw new Exception("El RNC ya existe");

            var importador = new Importador
            {
                Nombre = importadorDto.Nombre,
                Rnc = rncLimpio,
                PaisId = importadorDto.PaisId,
                Direccion = importadorDto.Direccion,
                Email = importadorDto.Email,
                Telefono = importadorDto.Telefono,
                Contacto = importadorDto.Contacto, 
                Activo = importadorDto.Activo,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };

            context.Importadores.Add(importador);
            await context.SaveChangesAsync();
            return await MapToDtoAsync(importador);
        }

        public async Task<ImportadorDto> UpdateAsync(ImportadorDto importadorDto)
        {
            var importador = await context.Importadores.FindAsync(importadorDto.Id)
                ?? throw new Exception("Importador no encontrado");

            var rncLimpio = importadorDto.Rnc.Trim();

            if (await ExistsByRncAsync(rncLimpio, importadorDto.Id))
                throw new Exception("El RNC ya existe");

            bool tieneOrdenes = await HasOrdersAsync(importador.Id);

            if (tieneOrdenes && importador.Rnc != rncLimpio)
                throw new Exception("No se puede modificar el RNC porque este importador ya tiene órdenes registradas.");

            importador.Nombre = importadorDto.Nombre;
            importador.Rnc = rncLimpio;
            importador.PaisId = importadorDto.PaisId;
            importador.Direccion = importadorDto.Direccion;
            importador.Email = importadorDto.Email;
            importador.Telefono = importadorDto.Telefono;
            importador.Contacto = importadorDto.Contacto;
            importador.Activo = importadorDto.Activo;
            importador.FechaModificacion = DateTime.Now;

            context.Importadores.Update(importador);
            await context.SaveChangesAsync();
            return await MapToDtoAsync(importador);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var importador = await context.Importadores.FindAsync(id);
            if (importador is null) return false;

            context.Importadores.Remove(importador);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByRncAsync(string rnc, int? excludeId = null)
        {
            var query = context.Importadores.Where(i => i.Rnc == rnc);
            if (excludeId.HasValue) query = query.Where(i => i.Id != excludeId);
            return await query.AnyAsync();
        }

        public async Task<bool> HasOrdersAsync(int importadorId) => await Task.FromResult(false);

        public async Task<bool> CanDeleteAsync(int importadorId) => !await HasOrdersAsync(importadorId);

        public async Task<string> GetDeleteErrorMessageAsync(int importadorId)
            => await HasOrdersAsync(importadorId) ? "No se puede eliminar porque tiene órdenes registradas." : "No se puede eliminar.";

        private async Task<List<ImportadorDto>> MapToDtoAsync(IEnumerable<Importador> importadores)
        {
            var result = new List<ImportadorDto>();
            foreach (var i in importadores)
            {
                result.Add(await MapToDtoAsync(i));
            }
            return result;
        }

        private async Task<ImportadorDto> MapToDtoAsync(Importador i)
        {
            return new ImportadorDto
            {
                Id = i.Id,
                Nombre = i.Nombre,
                Rnc = i.Rnc,
                PaisId = i.PaisId,
                NombrePais = i.Pais?.Nombre,
                Direccion = i.Direccion,
                Email = i.Email,
                Telefono = i.Telefono,
                Contacto = i.Contacto, 
                Activo = i.Activo,
                TieneOrdenes = await HasOrdersAsync(i.Id),
                FechaCreacion = i.FechaCreacion,
                FechaModificacion = i.FechaModificacion
            };
        }
    }
}