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
   
    public class OrdenImportacionService(ImportCostDbContext context) : IOrdenImportacionService
    {
        public async Task<List<OrdenImportacionDto>> GetAllAsync()
        {
            var ordenes = await context.OrdenesImportacion
                .Include(o => o.Importador)
                .Include(o => o.Proveedor)
                .Include(o => o.PaisOrigen)
                .Include(o => o.Moneda)
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();

            return MapToDto(ordenes);
        }

        public async Task<List<OrdenImportacionDto>> GetActivasAsync()
        {
            var ordenes = await context.OrdenesImportacion
                .Include(o => o.Importador)
                .Include(o => o.Proveedor)
                .Include(o => o.PaisOrigen)
                .Include(o => o.Moneda)
                .Where(o => o.Activo && o.Estado == "Abierta")
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();

            return MapToDto(ordenes);
        }

        public async Task<OrdenImportacionDto?> GetByIdAsync(int id)
        {
            var orden = await context.OrdenesImportacion
                .Include(o => o.Importador)
                .Include(o => o.Proveedor)
                .Include(o => o.PaisOrigen)
                .Include(o => o.Moneda)
                .FirstOrDefaultAsync(o => o.Id == id);

            return orden is null ? null : MapToDto(orden);
        }

        public async Task<OrdenImportacionDto> CreateAsync(OrdenImportacionDto ordenDto)
        {
            var numOrdenLimpio = ordenDto.NumeroOrden.Trim();

            if (await ExistsByNumeroOrdenAsync(numOrdenLimpio))
                throw new Exception("El número de orden ya existe");

            var orden = new OrdenImportacion
            {
                NumeroOrden = numOrdenLimpio,
                ImportadorId = ordenDto.ImportadorId,
                ProveedorId = ordenDto.ProveedorId,
                PaisOrigenId = ordenDto.PaisOrigenId,
                MonedaId = ordenDto.MonedaId,
                FechaOrden = ordenDto.FechaOrden,
                ModalidadTransporte = ordenDto.ModalidadTransporte,
                Estado = "Abierta",
                Activo = true,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };

            context.OrdenesImportacion.Add(orden);
            await context.SaveChangesAsync();

            return await GetByIdAsync(orden.Id) ?? throw new Exception("Error al recuperar orden");
        }

        public async Task<OrdenImportacionDto> UpdateAsync(OrdenImportacionDto ordenDto)
        {
            var orden = await context.OrdenesImportacion.FindAsync(ordenDto.Id)
                ?? throw new Exception("Orden no encontrada");

            if (orden.Estado != "Abierta")
                throw new Exception("Solo se pueden editar órdenes en estado 'Abierta'");

            var numOrdenLimpio = ordenDto.NumeroOrden.Trim();

            if (await ExistsByNumeroOrdenAsync(numOrdenLimpio, ordenDto.Id))
                throw new Exception("El número de orden ya existe");

            orden.NumeroOrden = numOrdenLimpio;
            orden.ImportadorId = ordenDto.ImportadorId;
            orden.ProveedorId = ordenDto.ProveedorId;
            orden.PaisOrigenId = ordenDto.PaisOrigenId;
            orden.MonedaId = ordenDto.MonedaId;
            orden.FechaOrden = ordenDto.FechaOrden;
            orden.ModalidadTransporte = ordenDto.ModalidadTransporte;
            orden.Activo = ordenDto.Activo;
            orden.FechaModificacion = DateTime.Now;

            context.OrdenesImportacion.Update(orden);
            await context.SaveChangesAsync();

            return await GetByIdAsync(orden.Id) ?? throw new Exception("Error al recuperar orden");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var orden = await context.OrdenesImportacion.FindAsync(id);
            if (orden is null)
                return false;

            context.OrdenesImportacion.Remove(orden);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CanEditAsync(int ordenId)
        {
            var orden = await context.OrdenesImportacion.FindAsync(ordenId);
            return orden?.Estado == "Abierta";
        }

        public async Task<bool> CanDeleteAsync(int ordenId)
        {
            var orden = await context.OrdenesImportacion.FindAsync(ordenId);
            return orden?.Estado == "Abierta";
        }

        public async Task<bool> CloseOrderAsync(int ordenId)
        {
            var orden = await context.OrdenesImportacion.FindAsync(ordenId)
                ?? throw new Exception("Orden no encontrada");

            if (orden.Estado != "Calculada")
                throw new Exception("Solo se pueden cerrar órdenes en estado 'Calculada'");

            orden.Estado = "Cerrada";
            orden.FechaCierre = DateTime.Now;
            orden.FechaModificacion = DateTime.Now;

            context.OrdenesImportacion.Update(orden);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsByNumeroOrdenAsync(string numeroOrden, int? excludeId = null)
        {
            var query = context.OrdenesImportacion.Where(o => o.NumeroOrden == numeroOrden);

            if (excludeId.HasValue)
                query = query.Where(o => o.Id != excludeId);

            return await query.AnyAsync();
        }

        public async Task<string> GetDeleteErrorMessageAsync(int ordenId)
        {
            var orden = await context.OrdenesImportacion.FindAsync(ordenId);
            if (orden?.Estado != "Abierta")
                return "Solo se pueden eliminar órdenes en estado 'Abierta'.";

            return "No se puede eliminar esta orden.";
        }

        private static List<OrdenImportacionDto> MapToDto(IEnumerable<OrdenImportacion> ordenes)
        {
            return ordenes.Select(MapToDto).ToList();
        }

        private static OrdenImportacionDto MapToDto(OrdenImportacion orden)
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
                FechaOrden = orden.FechaOrden,
                ModalidadTransporte = orden.ModalidadTransporte,
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