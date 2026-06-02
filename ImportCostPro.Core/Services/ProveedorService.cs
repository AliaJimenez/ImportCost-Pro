using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Data.Contexts;
using ImportCostPro.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly ImportCostDbContext _context;

        public ProveedorService(ImportCostDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProveedorDto>> GetAllAsync()
        {
            var proveedores = await _context.Proveedores
                .Include(p => p.PaisOrigen)
                .Include(p => p.MonedaPrincipal)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            return await MapToDtoAsync(proveedores);
        }

        public async Task<IEnumerable<ProveedorDto>> GetActivosAsync()
        {
            var proveedores = await _context.Proveedores
                .Where(p => p.Activo)
                .Include(p => p.PaisOrigen)
                .Include(p => p.MonedaPrincipal)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            return await MapToDtoAsync(proveedores);
        }

        public async Task<ProveedorDto?> GetByIdAsync(int id)  
        {
            var proveedor = await _context.Proveedores
                .Include(p => p.PaisOrigen)
                .Include(p => p.MonedaPrincipal)
                .FirstOrDefaultAsync(p => p.Id == id);

            return proveedor == null ? null : await MapToDtoAsync(proveedor);
        }

        public async Task<ProveedorDto> CreateAsync(ProveedorDto proveedorDto)
        {
            var proveedor = new Proveedor
            {
                Nombre = proveedorDto.Nombre,
                PaisOrigenId = proveedorDto.PaisOrigenId,
                MonedaPrincipalId = proveedorDto.MonedaPrincipalId,
                Contacto = proveedorDto.Contacto,
                Email = proveedorDto.Email,
                Telefono = proveedorDto.Telefono,
                Direccion = proveedorDto.Direccion,
                Activo = proveedorDto.Activo,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };

            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
            return await MapToDtoAsync(proveedor);
        }

        public async Task<ProveedorDto> UpdateAsync(ProveedorDto proveedorDto)
        {
            var proveedor = await _context.Proveedores.FindAsync(proveedorDto.Id);
            if (proveedor == null)
                throw new Exception("Proveedor no encontrado");

            bool tieneOrdenes = await HasOrdersAsync(proveedorDto.Id);

            if (tieneOrdenes)
            {
                if (proveedorDto.PaisOrigenId != proveedor.PaisOrigenId ||
                    proveedorDto.MonedaPrincipalId != proveedor.MonedaPrincipalId)
                {
                    throw new Exception("No se puede cambiar país/moneda. Este proveedor tiene órdenes.");
                }
            }

            proveedor.Nombre = proveedorDto.Nombre;
            proveedor.Contacto = proveedorDto.Contacto;
            proveedor.Email = proveedorDto.Email;
            proveedor.Telefono = proveedorDto.Telefono;
            proveedor.Direccion = proveedorDto.Direccion;
            proveedor.Activo = proveedorDto.Activo;

            if (!tieneOrdenes)
            {
                proveedor.PaisOrigenId = proveedorDto.PaisOrigenId;
                proveedor.MonedaPrincipalId = proveedorDto.MonedaPrincipalId;
            }

            proveedor.FechaModificacion = DateTime.Now;

            _context.Proveedores.Update(proveedor);
            await _context.SaveChangesAsync();
            return await MapToDtoAsync(proveedor);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
                return false;

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasOrdersAsync(int proveedorId)
        {
            return false;
        }

        public async Task<bool> CanDeleteAsync(int proveedorId)
        {
            return !await HasOrdersAsync(proveedorId);
        }

        public async Task<bool> CanEditPaisMonedaAsync(int proveedorId)
        {
            return !await HasOrdersAsync(proveedorId);
        }

        public async Task<string> GetDeleteErrorMessageAsync(int proveedorId)
        {
            if (await HasOrdersAsync(proveedorId))
                return "Este proveedor tiene órdenes de importación y no puede ser eliminado.";

            return "No se puede eliminar este proveedor.";
        }

        private async Task<IEnumerable<ProveedorDto>> MapToDtoAsync(IEnumerable<Proveedor> proveedores)
        {
            var result = new List<ProveedorDto>();
            foreach (var p in proveedores)
            {
                result.Add(await MapToDtoAsync(p));
            }
            return result;
        }

        private async Task<ProveedorDto> MapToDtoAsync(Proveedor proveedor)
        {
            bool tieneOrdenes = await HasOrdersAsync(proveedor.Id);

            return new ProveedorDto
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                PaisOrigenId = proveedor.PaisOrigenId,
                NombrePais = proveedor.PaisOrigen?.Nombre,
                MonedaPrincipalId = proveedor.MonedaPrincipalId,
                NombreMoneda = proveedor.MonedaPrincipal?.Nombre,
                Contacto = proveedor.Contacto!,
                Email = proveedor.Email!,
                Telefono = proveedor.Telefono!,
                Direccion = proveedor.Direccion!,
                Activo = proveedor.Activo,
                TieneOrdenes = tieneOrdenes,
                FechaCreacion = proveedor.FechaCreacion,
                FechaModificacion = proveedor.FechaModificacion
            };
        }
    }
}
