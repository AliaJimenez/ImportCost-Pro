using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class OrdenProductoService : IOrdenProductoService
    {
        private readonly ImportCostDbContext _context;

        public OrdenProductoService(ImportCostDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrdenProductoDto>> ObtenerPorOrdenAsync(int ordenId)
        {
            return await _context.OrdenProductos
                .Include(op => op.Producto)
                .Include(op => op.OrdenImportacion)
                .Where(op => op.OrdenImportacionId == ordenId)
                .Select(op => new OrdenProductoDto
                {
                    Id = op.Id,
                    OrdenImportacionId = op.OrdenImportacionId,
                    ProductoId = op.ProductoId,
                    Cantidad = op.Cantidad,
                    PrecioUnitarioFOB = op.PrecioUnitarioFOB,
                    MargenGananciaDeseado = op.MargenGananciaDeseado,
                    FOBTotal = op.FOBTotal,
                    PesoTotal = op.PesoTotal,
                    VolumenTotal = op.VolumenTotal,
                    NombreProducto = op.Producto.Nombre,
                    CodigoProducto = op.Producto.CodigoReferencia,
                    NumeroOrden = op.OrdenImportacion.NumeroOrden,
                    EstadoOrden = op.OrdenImportacion.Estado,
                    OrdenPermiteModificaciones =
                        op.OrdenImportacion.Estado == "Abierta"
                })
                .ToListAsync();
        }

        public async Task<OrdenProductoDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _context.OrdenProductos
                .Include(op => op.Producto)
                .Include(op => op.OrdenImportacion)
                .FirstOrDefaultAsync(op => op.Id == id);

            if (entidad == null) return null;

            return new OrdenProductoDto
            {
                Id = entidad.Id,
                OrdenImportacionId = entidad.OrdenImportacionId,
                ProductoId = entidad.ProductoId,
                Cantidad = entidad.Cantidad,
                PrecioUnitarioFOB = entidad.PrecioUnitarioFOB,
                MargenGananciaDeseado = entidad.MargenGananciaDeseado,
                FOBTotal = entidad.FOBTotal,
                PesoTotal = entidad.PesoTotal,
                VolumenTotal = entidad.VolumenTotal,
                NombreProducto = entidad.Producto.Nombre,
                CodigoProducto = entidad.Producto.CodigoReferencia,
                NumeroOrden = entidad.OrdenImportacion.NumeroOrden,
                EstadoOrden = entidad.OrdenImportacion.Estado,
                OrdenPermiteModificaciones =
                    entidad.OrdenImportacion.Estado == "Abierta"
            };
        }

        public async Task<(bool exito, string mensaje)> AgregarAsync(OrdenProductoDto dto)
        {
            
            var orden = await _context.OrdenesImportacion
                .FirstOrDefaultAsync(o => o.Id == dto.OrdenImportacionId);

            if (orden == null)
                return (false, "La orden de importación no existe.");

            if (orden.Estado != "Abierta")
                return (false, "No se pueden agregar productos a una orden que no está abierta.");

            
            var productoExiste = await _context.OrdenProductos
                .AnyAsync(op => op.OrdenImportacionId == dto.OrdenImportacionId
                    && op.ProductoId == dto.ProductoId);

            if (productoExiste)
                return (false, "Este producto ya fue agregado a la orden. Si desea modificarlo, edite el producto ya agregado.");

            
            if (dto.Cantidad <= 0)
                return (false, "La cantidad debe ser mayor que 0.");

            if (dto.PrecioUnitarioFOB <= 0)
                return (false, "El precio unitario FOB debe ser mayor que 0.");

            if (dto.MargenGananciaDeseado < 0 || dto.MargenGananciaDeseado >= 100)
                return (false, "El margen de ganancia debe ser mayor o igual a 0 y menor que 100.");

            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == dto.ProductoId);

            if (producto == null)
                return (false, "El producto seleccionado no existe.");

            // Calcular FOB, peso y volumen
            decimal fobTotal = dto.Cantidad * dto.PrecioUnitarioFOB;
            decimal pesoTotal = dto.Cantidad * producto.PesoUnitario;
            decimal? volumenTotal = null;

            if (producto.Largo.HasValue && producto.Ancho.HasValue && producto.Alto.HasValue)
                volumenTotal = dto.Cantidad * producto.Largo.Value
                    * producto.Ancho.Value * producto.Alto.Value;

            var entidad = new Data.Entities.OrdenProducto
            {
                OrdenImportacionId = dto.OrdenImportacionId,
                ProductoId = dto.ProductoId,
                Cantidad = dto.Cantidad,
                PrecioUnitarioFOB = dto.PrecioUnitarioFOB,
                MargenGananciaDeseado = dto.MargenGananciaDeseado,
                FOBTotal = fobTotal,
                PesoTotal = pesoTotal,
                VolumenTotal = volumenTotal
            };

            _context.OrdenProductos.Add(entidad);
            await _context.SaveChangesAsync();

            return (true, "Producto agregado a la orden correctamente.");
        }

        public async Task<(bool exito, string mensaje)> EditarAsync(OrdenProductoDto dto)
        {
            var entidad = await _context.OrdenProductos
                .Include(op => op.OrdenImportacion)
                .Include(op => op.Producto)
                .FirstOrDefaultAsync(op => op.Id == dto.Id);

            if (entidad == null)
                return (false, "El producto de la orden no fue encontrado.");

            if (entidad.OrdenImportacion.Estado != "Abierta")
                return (false, "No se puede editar un producto de una orden que no está abierta.");

            if (dto.Cantidad <= 0)
                return (false, "La cantidad debe ser mayor que 0.");

            if (dto.PrecioUnitarioFOB <= 0)
                return (false, "El precio unitario FOB debe ser mayor que 0.");

            if (dto.MargenGananciaDeseado < 0 || dto.MargenGananciaDeseado >= 100)
                return (false, "El margen de ganancia debe ser mayor o igual a 0 y menor que 100.");

            // Recalcular FOB, peso y volumen
            decimal fobTotal = dto.Cantidad * dto.PrecioUnitarioFOB;
            decimal pesoTotal = dto.Cantidad * entidad.Producto.PesoUnitario;
            decimal? volumenTotal = null;

            if (entidad.Producto.Largo.HasValue
                && entidad.Producto.Ancho.HasValue
                && entidad.Producto.Alto.HasValue)
                volumenTotal = dto.Cantidad * entidad.Producto.Largo.Value
                    * entidad.Producto.Ancho.Value * entidad.Producto.Alto.Value;

            entidad.Cantidad = dto.Cantidad;
            entidad.PrecioUnitarioFOB = dto.PrecioUnitarioFOB;
            entidad.MargenGananciaDeseado = dto.MargenGananciaDeseado;
            entidad.FOBTotal = fobTotal;
            entidad.PesoTotal = pesoTotal;
            entidad.VolumenTotal = volumenTotal;

            await _context.SaveChangesAsync();
            return (true, "Producto de la orden actualizado correctamente.");
        }

        public async Task<(bool exito, string mensaje)> EliminarAsync(int id)
        {
            var entidad = await _context.OrdenProductos
                .Include(op => op.OrdenImportacion)
                .FirstOrDefaultAsync(op => op.Id == id);

            if (entidad == null)
                return (false, "El producto de la orden no fue encontrado.");

            if (entidad.OrdenImportacion.Estado != "Abierta")
                return (false, "No se puede eliminar un producto de una orden que no está abierta.");

            _context.OrdenProductos.Remove(entidad);
            await _context.SaveChangesAsync();

            return (true, "Producto eliminado de la orden correctamente.");
        }

        public async Task<ResumenFOBDto> ObtenerResumenFOBAsync(int ordenId)
        {
            var orden = await _context.OrdenesImportacion
                .Include(o => o.Moneda)
                .FirstOrDefaultAsync(o => o.Id == ordenId);

            var productos = await _context.OrdenProductos
                .Where(op => op.OrdenImportacionId == ordenId)
                .ToListAsync();

            return new ResumenFOBDto
            {
                CantidadTotal = productos.Sum(p => p.Cantidad),
                FOBTotal = productos.Sum(p => p.FOBTotal),
                PesoTotal = productos.Sum(p => p.PesoTotal),
                VolumenTotal = productos.Any(p => p.VolumenTotal.HasValue)
                    ? productos.Sum(p => p.VolumenTotal ?? 0)
                    : null,
                MonedaOrden = orden?.Moneda?.Nombre ?? string.Empty,
                SimboloMoneda = orden?.Moneda?.Simbolo ?? string.Empty
            };
        }
    }
}
