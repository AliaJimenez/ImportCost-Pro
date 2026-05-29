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
    public class OrdenImportacionService : IOrdenImportacionService
    {
        private readonly ImportCostDbContext _context;

        public OrdenImportacionService(ImportCostDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrdenImportacionDto>> GetAllAsync()
        {
            var ordenes = await _context.OrdenesImportacion
                .Include(o => o.Importador)
                .Include(o => o.Proveedor)
                .Include(o => o.PaisOrigen)
                .Include(o => o.Moneda)
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();

            return MapToDto(ordenes);
        }

        public async Task<IEnumerable<OrdenImportacionDto>> GetActivasAsync()
        {
            var ordenes = await _context.OrdenesImportacion
                .Where(o => o.Activo && o.Estado == "Abierta")
                .Include(o => o.Importador)
                .Include(o => o.Proveedor)
                .Include(o => o.PaisOrigen)
                .Include(o => o.Moneda)
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();

            return MapToDto(ordenes);
        }

        public async Task<OrdenImportacionDto> GetByIdAsync(int id)
        {
            var orden = await _context.OrdenesImportacion
                .Include(o => o.Importador)
                .Include(o => o.Proveedor)
                .Include(o => o.PaisOrigen)
                .Include(o => o.Moneda)
                .FirstOrDefaultAsync(o => o.Id == id);

            return orden == null ? null : MapToDto(orden);
        }

        public async Task<OrdenImportacionDto> CreateAsync(OrdenImportacionDto ordenDto)
        {
            // Validar que importador, proveedor, país y moneda estén activos
            var importador = await _context.Importadores.FindAsync(ordenDto.ImportadorId);
            if (importador == null || !importador.Activo)
                throw new Exception("Importador no válido o inactivo");

            var proveedor = await _context.Proveedores.FindAsync(ordenDto.ProveedorId);
            if (proveedor == null || !proveedor.Activo)
                throw new Exception("Proveedor no válido o inactivo");

            var pais = await _context.Paises.FindAsync(ordenDto.PaisOrigenId);
            if (pais == null || !pais.Activo)
                throw new Exception("País no válido o inactivo");

            var moneda = await _context.Monedas.FindAsync(ordenDto.MonedaId);
            if (moneda == null || !moneda.Activo)
                throw new Exception("Moneda no válida o inactiva");

            // Validar número de orden único
            if (await ExistsByNumeroOrdenAsync(ordenDto.NumeroOrden))
                throw new Exception("El número de orden ya existe");

            var orden = new OrdenImportacion
            {
                NumeroOrden = ordenDto.NumeroOrden,
                ImportadorId = ordenDto.ImportadorId,
                ProveedorId = ordenDto.ProveedorId,
                PaisOrigenId = ordenDto.PaisOrigenId,
                MonedaId = ordenDto.MonedaId,
                Estado = "Abierta",
                Activo = true,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };

            _context.OrdenesImportacion.Add(orden);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(orden.Id);
        }

        public async Task<OrdenImportacionDto> UpdateAsync(OrdenImportacionDto ordenDto)
        {
            var orden = await _context.OrdenesImportacion.FindAsync(ordenDto.Id);
            if (orden == null)
                throw new Exception("Orden no encontrada");

            // 🔒 VALIDACIÓN: No editar si Estado != "Abierta"
            if (orden.Estado != "Abierta")
                throw new Exception("Solo se pueden editar órdenes en estado 'Abierta'");

            // Validar número de orden único
            if (await ExistsByNumeroOrdenAsync(ordenDto.NumeroOrden, ordenDto.Id))
                throw new Exception("El número de orden ya existe");

            orden.NumeroOrden = ordenDto.NumeroOrden;
            orden.ImportadorId = ordenDto.ImportadorId;
            orden.ProveedorId = ordenDto.ProveedorId;
            orden.PaisOrigenId = ordenDto.PaisOrigenId;
            orden.MonedaId = ordenDto.MonedaId;
            orden.Activo = ordenDto.Activo;
            orden.FechaModificacion = DateTime.Now;

            _context.OrdenesImportacion.Update(orden);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(orden.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var orden = await _context.OrdenesImportacion.FindAsync(id);
            if (orden == null)
                return false;

            _context.OrdenesImportacion.Remove(orden);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CanEditAsync(int ordenId)
        {
            var orden = await _context.OrdenesImportacion.FindAsync(ordenId);
            return orden?.Estado == "Abierta";
        }

        public async Task<bool> CanDeleteAsync(int ordenId)
        {
            var orden = await _context.OrdenesImportacion.FindAsync(ordenId);
            return orden?.Estado == "Abierta";
        }
        public async Task<bool> CloseOrderAsync(int ordenId)
        {
            var orden = await _context.OrdenesImportacion.FindAsync(ordenId);
            if (orden == null)
                return false;

            if (orden.Estado != "Calculada")
                throw new Exception("Solo se pueden cerrar órdenes en estado 'Calculada'");

            orden.Estado = "Cerrada";
            orden.FechaCierre = DateTime.Now;
            orden.FechaModificacion = DateTime.Now;

            _context.OrdenesImportacion.Update(orden);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsByNumeroOrdenAsync(string numeroOrden, int? excludeId = null)
        {
            var query = _context.OrdenesImportacion
                .Where(o => o.NumeroOrden == numeroOrden);

            if (excludeId.HasValue)
                query = query.Where(o => o.Id != excludeId);

            return await query.AnyAsync();
        }

        public async Task<string> GetDeleteErrorMessageAsync(int ordenId)
        {
            var orden = await _context.OrdenesImportacion.FindAsync(ordenId);

            if (orden?.Estado != "Abierta")
                return "Solo se pueden eliminar órdenes en estado 'Abierta'.";

            return "No se puede eliminar esta orden.";
        }

        private IEnumerable<OrdenImportacionDto> MapToDto(IEnumerable<OrdenImportacion> ordenes)
        {
            return ordenes.Select(o => new OrdenImportacionDto
            {
                Id = o.Id,
                NumeroOrden = o.NumeroOrden,
                ImportadorId = o.ImportadorId,
                NombreImportador = o.Importador?.Nombre,
                ProveedorId = o.ProveedorId,
                NombreProveedor = o.Proveedor?.Nombre,
                PaisOrigenId = o.PaisOrigenId,
                NombrePais = o.PaisOrigen?.Nombre,
                MonedaId = o.MonedaId,
                NombreMoneda = o.Moneda?.Nombre,
                Estado = o.Estado,
                CostoFOB = o.CostoFOB,
                CIF = o.CIF,
                Arancel = o.Arancel,
                ImpuestoSelectivo = o.ImpuestoSelectivo,
                TasaAduanal = o.TasaAduanal,
                ITBIS = o.ITBIS,
                PrecioSugerido = o.PrecioSugerido,
                Activo = o.Activo,
                FechaCreacion = o.FechaCreacion,
                FechaModificacion = o.FechaModificacion,
                FechaCierre = o.FechaCierre
            });
        }

        private OrdenImportacionDto MapToDto(OrdenImportacion orden)
        {
            return new OrdenImportacionDto
            {
                Id = orden.Id,
                NumeroOrden = orden.NumeroOrden,
                ImportadorId = orden.ImportadorId,
                NombreImportador = orden.Importador?.Nombre,
                ProveedorId = orden.ProveedorId,
                NombreProveedor = orden.Proveedor?.Nombre,
                PaisOrigenId = orden.PaisOrigenId,
                NombrePais = orden.PaisOrigen?.Nombre,
                MonedaId = orden.MonedaId,
                NombreMoneda = orden.Moneda?.Nombre,
                Estado = orden.Estado,
                CostoFOB = orden.CostoFOB,
                CIF = orden.CIF,
                Arancel = orden.Arancel,
                ImpuestoSelectivo = orden.ImpuestoSelectivo,
                TasaAduanal = orden.TasaAduanal,
                ITBIS = orden.ITBIS,
                PrecioSugerido = orden.PrecioSugerido,
                Activo = orden.Activo,
                FechaCreacion = orden.FechaCreacion,
                FechaModificacion = orden.FechaModificacion,
                FechaCierre = orden.FechaCierre
            };
        }
    }
}