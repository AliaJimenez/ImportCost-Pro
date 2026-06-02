using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class OrdenGastoService : IOrdenGastoService
    {
        private readonly ImportCostDbContext _context;

        public OrdenGastoService(ImportCostDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrdenGastoDto>> ObtenerPorOrdenAsync(int ordenId)
        {
            return await _context.OrdenGastos
                .Include(og => og.Moneda)
                .Include(og => og.OrdenImportacion)
                .Where(og => og.OrdenImportacionId == ordenId)
                .Select(og => new OrdenGastoDto
                {
                    Id = og.Id,
                    OrdenImportacionId = og.OrdenImportacionId,
                    MonedaId = og.MonedaId,
                    TipoGasto = og.TipoGasto,
                    Monto = og.Monto,
                    MetodoDistribucion = og.MetodoDistribucion,
                    FechaGasto = og.FechaGasto,
                    MontoEnMonedaLocal = og.MontoEnMonedaLocal,
                    NumeroOrden = og.OrdenImportacion.NumeroOrden,
                    NombreMoneda = og.Moneda.Nombre,
                    SimboloMoneda = og.Moneda.Simbolo,
                    EstadoOrden = og.OrdenImportacion.Estado,
                    OrdenPermiteModificaciones =
                        og.OrdenImportacion.Estado == "Abierta"
                })
                .ToListAsync();
        }

        public async Task<OrdenGastoDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _context.OrdenGastos
                .Include(og => og.Moneda)
                .Include(og => og.OrdenImportacion)
                .FirstOrDefaultAsync(og => og.Id == id);

            if (entidad == null) return null;

            return new OrdenGastoDto
            {
                Id = entidad.Id,
                OrdenImportacionId = entidad.OrdenImportacionId,
                MonedaId = entidad.MonedaId,
                TipoGasto = entidad.TipoGasto,
                Monto = entidad.Monto,
                MetodoDistribucion = entidad.MetodoDistribucion,
                FechaGasto = entidad.FechaGasto,
                MontoEnMonedaLocal = entidad.MontoEnMonedaLocal,
                NumeroOrden = entidad.OrdenImportacion.NumeroOrden,
                NombreMoneda = entidad.Moneda.Nombre,
                SimboloMoneda = entidad.Moneda.Simbolo,
                EstadoOrden = entidad.OrdenImportacion.Estado,
                OrdenPermiteModificaciones =
                    entidad.OrdenImportacion.Estado == "Abierta"
            };
        }

        public async Task<(bool exito, string mensaje)> RegistrarAsync(OrdenGastoDto dto)
        {
            var orden = await _context.OrdenesImportacion
                .FirstOrDefaultAsync(o => o.Id == dto.OrdenImportacionId);

            if (orden == null)
                return (false, "La orden de importación no existe.");

            if (orden.Estado != "Abierta")
                return (false, "No se pueden registrar gastos en una orden que no está abierta.");

            if (dto.TipoGasto == "FleteInternacional")
            {
                var fleteExiste = await _context.OrdenGastos
                    .AnyAsync(og => og.OrdenImportacionId == dto.OrdenImportacionId
                        && og.TipoGasto == "FleteInternacional");

                if (fleteExiste)
                    return (false, "Ya existe un gasto de flete internacional registrado para esta orden.");
            }

            if (dto.TipoGasto == "SeguroInternacional")
            {
                var seguroExiste = await _context.OrdenGastos
                    .AnyAsync(og => og.OrdenImportacionId == dto.OrdenImportacionId
                        && og.TipoGasto == "SeguroInternacional");

                if (seguroExiste)
                    return (false, "Ya existe un gasto de seguro internacional registrado para esta orden.");
            }

            if (dto.Monto <= 0)
                return (false, "El monto del gasto debe ser mayor que 0.");

            var monedaLocal = await _context.Monedas
                .FirstOrDefaultAsync(m => m.EsMonedaLocal && m.Activo);

            if (monedaLocal == null)
                return (false, "No existe una moneda local configurada en el sistema.");

            decimal montoLocal = dto.Monto;

            if (dto.MonedaId != monedaLocal.Id)
            {
                var tasa = await _context.TasasCambio
                    .Where(t => t.MonedaOrigenId == dto.MonedaId
                        && t.MonedaDestinoId == monedaLocal.Id
                        && t.Activo
                        && t.FechaVigencia <= dto.FechaGasto)
                    .OrderByDescending(t => t.FechaVigencia)
                    .FirstOrDefaultAsync();

                if (tasa == null)
                    return (false, "No existe una tasa de cambio activa desde la moneda del gasto hacia la moneda local para la fecha del gasto.");

                montoLocal = dto.Monto * tasa.Tasa;
            }

            var entidad = new Data.Entities.OrdenGasto
            {
                OrdenImportacionId = dto.OrdenImportacionId,
                MonedaId = dto.MonedaId,
                TipoGasto = dto.TipoGasto,
                Monto = dto.Monto,
                MetodoDistribucion = dto.MetodoDistribucion,
                FechaGasto = dto.FechaGasto,
                MontoEnMonedaLocal = montoLocal
            };

            _context.OrdenGastos.Add(entidad);
            await _context.SaveChangesAsync();

            return (true, "Gasto registrado correctamente.");
        }

        public async Task<(bool exito, string mensaje)> EditarAsync(OrdenGastoDto dto)
        {
            var entidad = await _context.OrdenGastos
                .Include(og => og.OrdenImportacion)
                .FirstOrDefaultAsync(og => og.Id == dto.Id);

            if (entidad == null)
                return (false, "El gasto no fue encontrado.");

            if (entidad.OrdenImportacion.Estado != "Abierta")
                return (false, "No se puede editar un gasto de una orden que no está abierta.");

            if (dto.TipoGasto == "FleteInternacional")
            {
                var fleteExiste = await _context.OrdenGastos
                    .AnyAsync(og => og.OrdenImportacionId == dto.OrdenImportacionId
                        && og.TipoGasto == "FleteInternacional"
                        && og.Id != dto.Id);

                if (fleteExiste)
                    return (false, "Ya existe un gasto de flete internacional registrado para esta orden.");
            }

            if (dto.TipoGasto == "SeguroInternacional")
            {
                var seguroExiste = await _context.OrdenGastos
                    .AnyAsync(og => og.OrdenImportacionId == dto.OrdenImportacionId
                        && og.TipoGasto == "SeguroInternacional"
                        && og.Id != dto.Id);

                if (seguroExiste)
                    return (false, "Ya existe un gasto de seguro internacional registrado para esta orden.");
            }

            if (dto.Monto <= 0)
                return (false, "El monto del gasto debe ser mayor que 0.");

            // Recalcular monto en moneda local
            var monedaLocal = await _context.Monedas
                .FirstOrDefaultAsync(m => m.EsMonedaLocal && m.Activo);

            if (monedaLocal == null)
                return (false, "No existe una moneda local configurada en el sistema.");

            decimal montoLocal = dto.Monto;

            if (dto.MonedaId != monedaLocal.Id)
            {
                var tasa = await _context.TasasCambio
                    .Where(t => t.MonedaOrigenId == dto.MonedaId
                        && t.MonedaDestinoId == monedaLocal.Id
                        && t.Activo
                        && t.FechaVigencia <= dto.FechaGasto)
                    .OrderByDescending(t => t.FechaVigencia)
                    .FirstOrDefaultAsync();

                if (tasa == null)
                    return (false, "No existe una tasa de cambio activa desde la moneda del gasto hacia la moneda local para la fecha del gasto.");

                montoLocal = dto.Monto * tasa.Tasa;
            }

            entidad.MonedaId = dto.MonedaId;
            entidad.TipoGasto = dto.TipoGasto;
            entidad.Monto = dto.Monto;
            entidad.MetodoDistribucion = dto.MetodoDistribucion;
            entidad.FechaGasto = dto.FechaGasto;
            entidad.MontoEnMonedaLocal = montoLocal;

            await _context.SaveChangesAsync();
            return (true, "Gasto actualizado correctamente.");
        }

        public async Task<(bool exito, string mensaje)> EliminarAsync(int id)
        {
            var entidad = await _context.OrdenGastos
                .Include(og => og.OrdenImportacion)
                .FirstOrDefaultAsync(og => og.Id == id);

            if (entidad == null)
                return (false, "El gasto no fue encontrado.");

            if (entidad.OrdenImportacion.Estado != "Abierta")
                return (false, "No se puede eliminar un gasto de una orden que no está abierta.");

            _context.OrdenGastos.Remove(entidad);
            await _context.SaveChangesAsync();

            return (true, "Gasto eliminado correctamente.");
        }

        public async Task<List<OrdenImportacionDto>> ObtenerOrdenesAbiertasAsync()
        {
            return await _context.OrdenesImportacion
                .Include(o => o.Importador)
                .Where(o => o.Estado == "Abierta" && o.Activo)
                .Select(o => new OrdenImportacionDto
                {
                    Id = o.Id,
                    NumeroOrden = o.NumeroOrden,
                    Estado = o.Estado,
                    NombreImportador = o.Importador != null ? o.Importador.Nombre : string.Empty,
                    ImportadorId = o.ImportadorId,
                    ProveedorId = o.ProveedorId,
                    PaisOrigenId = o.PaisOrigenId,
                    MonedaId = o.MonedaId,
                    FechaOrden = o.FechaOrden,
                    ModalidadTransporte = o.ModalidadTransporte,
                    Activo = o.Activo,
                    FechaCreacion = o.FechaCreacion,
                    FechaModificacion = o.FechaModificacion
                })
                .ToListAsync();
        }
    }
}