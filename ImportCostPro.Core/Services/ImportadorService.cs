using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Data.Contexts;
using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportCostPro.Core.Services
{
    public class ImportadorService : IImportadorService
    {
        private readonly ImportCostDbContext _context;

    public ImportadorService(ImportCostDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ImportadorDto>> GetAllAsync()
    {
        var importadores = await _context.Importadores
            .OrderBy(i => i.Nombre)
            .ToListAsync();
        return MapToDto(importadores);
    }

    public async Task<IEnumerable<ImportadorDto>> GetActivosAsync()
    {
        var importadores = await _context.Importadores
            .Where(i => i.Activo)
            .OrderBy(i => i.Nombre)
            .ToListAsync();
        return MapToDto(importadores);
    }

    public async Task<ImportadorDto> GetByIdAsync(int id)
    {
        var importador = await _context.Importadores.FindAsync(id);
        return importador == null ? null : MapToDto(importador);
    }

    public async Task<ImportadorDto> CreateAsync(ImportadorDto importadorDto)
    {
        if (await ExistsByRncAsync(importadorDto.Rnc))
            throw new Exception("El RNC ya existe");

        var importador = new Importador
        {
            Nombre = importadorDto.Nombre,
            Rnc = importadorDto.Rnc,
            Direccion = importadorDto.Direccion,
            Contacto = importadorDto.Contacto,
            Email = importadorDto.Email,
            Telefono = importadorDto.Telefono,
            Activo = importadorDto.Activo,
            FechaCreacion = DateTime.Now,
            FechaModificacion = DateTime.Now
        };

        _context.Importadores.Add(importador);
        await _context.SaveChangesAsync();
        return MapToDto(importador);
    }

    public async Task<ImportadorDto> UpdateAsync(ImportadorDto importadorDto)
    {
        var importador = await _context.Importadores.FindAsync(importadorDto.Id);
        if (importador == null)
            throw new Exception("Importador no encontrado");

        if (await ExistsByRncAsync(importadorDto.Rnc, importadorDto.Id))
            throw new Exception("El RNC ya existe");

        importador.Nombre = importadorDto.Nombre;
        importador.Rnc = importadorDto.Rnc;
        importador.Direccion = importadorDto.Direccion;
        importador.Contacto = importadorDto.Contacto;
        importador.Email = importadorDto.Email;
        importador.Telefono = importadorDto.Telefono;
        importador.Activo = importadorDto.Activo;
        importador.FechaModificacion = DateTime.Now;

        _context.Importadores.Update(importador);
        await _context.SaveChangesAsync();
        return MapToDto(importador);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var importador = await _context.Importadores.FindAsync(id);
        if (importador == null)
            return false;

        _context.Importadores.Remove(importador);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsByRncAsync(string rnc, int? excludeId = null)
    {
        var query = _context.Importadores
            .Where(i => i.Rnc == rnc);

        if (excludeId.HasValue)
            query = query.Where(i => i.Id != excludeId);

        return await query.AnyAsync();
    }

    public async Task<bool> CanDeleteAsync(int importadorId)
    {
        return true;
    }

    public async Task<string> GetDeleteErrorMessageAsync(int importadorId)
    {
        return "Este importador tiene órdenes de importación registradas.";
    }

    private IEnumerable<ImportadorDto> MapToDto(IEnumerable<Importador> importadores)
    {
        return importadores.Select(i => new ImportadorDto
        {
            Id = i.Id,
            Nombre = i.Nombre,
            Rnc = i.Rnc,
            Direccion = i.Direccion,
            Contacto = i.Contacto,
            Email = i.Email,
            Telefono = i.Telefono,
            Activo = i.Activo,
            FechaCreacion = i.FechaCreacion,
            FechaModificacion = i.FechaModificacion
        });
    }

    private ImportadorDto MapToDto(Importador importador)
    {
        return new ImportadorDto
        {
            Id = importador.Id,
            Nombre = importador.Nombre,
            Rnc = importador.Rnc,
            Direccion = importador.Direccion,
            Contacto = importador.Contacto,
            Email = importador.Email,
            Telefono = importador.Telefono,
            Activo = importador.Activo,
            FechaCreacion = importador.FechaCreacion,
            FechaModificacion = importador.FechaModificacion
        };
    }
}
}
