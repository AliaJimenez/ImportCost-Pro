using ImportCostPro.Core.Dtos;
using ImportCostPro.Data.Contexts;
using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ImportCostPro.Core.Interfaces;

namespace ImportCostPro.Core.Services
{
    public class ProductoService: IProductoService
    {
        private readonly ImportCostDbContext _context;

        public ProductoService(ImportCostDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProductoDto>> ObtenerTodosAsync()
        {
            return await _context.Productos
                .Include(p => p.CategoriaArancelaria)
                .Include(p => p.PaisOrigen) 
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    CodigoReferencia = p.CodigoReferencia,
                    PesoUnitario = p.PesoUnitario,
                    Largo = p.Largo,
                    Ancho = p.Ancho,
                    Alto = p.Alto,
                    UnidadMedida = p.UnidadMedida,
                    Descripcion = p.Descripcion,
                    Activo = p.Activo,
                    PaisOrigenId = p.PaisOrigenId,
                    CategoriaArancelariaId = p.CategoriaArancelariaId,
                    NombreCategoria = p.CategoriaArancelaria.Nombre,
                    NombrePais = p.PaisOrigen.Nombre 
                })
                .ToListAsync();
        }
        public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _context.Productos
                .Include(p => p.CategoriaArancelaria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entidad == null) return null;

            return new ProductoDto
            {
                Id = entidad.Id,
                Nombre = entidad.Nombre,
                CodigoReferencia = entidad.CodigoReferencia,
                PesoUnitario = entidad.PesoUnitario,
                Largo = entidad.Largo,
                Ancho = entidad.Ancho,
                Alto = entidad.Alto,
                UnidadMedida = entidad.UnidadMedida,
                Descripcion = entidad.Descripcion,
                Activo = entidad.Activo,
                PaisOrigenId = entidad.PaisOrigenId,
                CategoriaArancelariaId = entidad.CategoriaArancelariaId,
                NombreCategoria = entidad.CategoriaArancelaria.Nombre
            };
        }
        public async Task<(bool exito, string mensaje)> CrearAsync(ProductoDto dto)
        {
            // Validar código único
            var codigoExiste = await _context.Productos
                .AnyAsync(p => p.CodigoReferencia.ToUpper()
                    == dto.CodigoReferencia.Trim().ToUpper());

            if (codigoExiste)
                return (false, "Ya existe un producto con este código o referencia.");

            // Validar dimensiones
            bool algunaTieneDimension = dto.Largo.HasValue
                || dto.Ancho.HasValue
                || dto.Alto.HasValue;

            bool todasTienenDimension = dto.Largo.HasValue
                && dto.Ancho.HasValue
                && dto.Alto.HasValue;

            if (algunaTieneDimension && !todasTienenDimension)
                return (false, "Si ingresa una dimensión debe ingresar largo, ancho y alto.");

            if (todasTienenDimension)
            {
                if (dto.Largo <= 0 || dto.Ancho <= 0 || dto.Alto <= 0)
                    return (false, "El largo, ancho y alto deben ser mayores que 0.");
            }

            if (dto.PesoUnitario <= 0)
                return (false, "El peso unitario debe ser mayor que 0.");

            var entidad = new Producto
            {
                Nombre = dto.Nombre.Trim(),
                CodigoReferencia = dto.CodigoReferencia.Trim().ToUpper(),
                PesoUnitario = dto.PesoUnitario,
                Largo = dto.Largo,
                Ancho = dto.Ancho,
                Alto = dto.Alto,
                UnidadMedida = dto.UnidadMedida,
                Descripcion = dto.Descripcion?.Trim(),
                Activo = dto.Activo,
                PaisOrigenId = dto.PaisOrigenId,
                CategoriaArancelariaId = dto.CategoriaArancelariaId
            };

            _context.Productos.Add(entidad);
            await _context.SaveChangesAsync();

            return (true, "Producto creado correctamente.");
        }
        public async Task<(bool exito, string mensaje)> EditarAsync(ProductoDto dto)
        {
            var entidad = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (entidad == null)
                return (false, "Producto no encontrado.");

            var codigoExiste = await _context.Productos
                .AnyAsync(p => p.CodigoReferencia.ToUpper()
                    == dto.CodigoReferencia.Trim().ToUpper()
                    && p.Id != dto.Id);

            if (codigoExiste)
                return (false, "Ya existe otro producto con este código o referencia.");

            bool algunaTieneDimension = dto.Largo.HasValue
                || dto.Ancho.HasValue
                || dto.Alto.HasValue;

            bool todasTienenDimension = dto.Largo.HasValue
                && dto.Ancho.HasValue
                && dto.Alto.HasValue;

            if (algunaTieneDimension && !todasTienenDimension)
                return (false, "Si ingresa una dimensión debe ingresar largo, ancho y alto.");

            if (todasTienenDimension)
            {
                if (dto.Largo <= 0 || dto.Ancho <= 0 || dto.Alto <= 0)
                    return (false, "El largo, ancho y alto deben ser mayores que 0.");
            }

            if (dto.PesoUnitario <= 0)
                return (false, "El peso unitario debe ser mayor que 0.");

            entidad.Nombre = dto.Nombre.Trim();
            entidad.CodigoReferencia = dto.CodigoReferencia.Trim().ToUpper();
            entidad.PesoUnitario = dto.PesoUnitario;
            entidad.Largo = dto.Largo;
            entidad.Ancho = dto.Ancho;
            entidad.Alto = dto.Alto;
            entidad.UnidadMedida = dto.UnidadMedida;
            entidad.Descripcion = dto.Descripcion?.Trim();
            entidad.Activo = dto.Activo;
            entidad.PaisOrigenId = dto.PaisOrigenId;
            entidad.CategoriaArancelariaId = dto.CategoriaArancelariaId;

            await _context.SaveChangesAsync();
            return (true, "Producto actualizado correctamente.");
        }
        public async Task<(bool exito, string mensaje)> EliminarAsync(int id)
        {
            var entidad = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entidad == null)
                return (false, "Producto no encontrado.");

            // Aquí voy a valdar el producto asociado cuando ordenes este creado
            
            // bool tieneOrdenes = await _context.OrdenProductos.AnyAsync(op => op.ProductoId == id);
            // if (tieneOrdenes)
            //     return (false, "No se puede eliminar porque está asociado a órdenes.");

            _context.Productos.Remove(entidad);
            await _context.SaveChangesAsync();

            return (true, "Producto eliminado correctamente.");
        }
    }
}

