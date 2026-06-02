using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ImportCostPro.Data.Contexts;


namespace ImportCostPro.Core.Services
{
    public class CategoriaArancelariaService : ICategoriaArancelariaService
    {
        private readonly ImportCostDbContext _context;
        public CategoriaArancelariaService(ImportCostDbContext context)
        {
            _context = context;
        }
        public async Task<List<CategoriaArancelariaDto>> ObtenerTodasAsync()
        {
            return await _context.CategoriasArancelarias
                .Select(c => new CategoriaArancelariaDto
                {
                    Id = c.Id,
                    CodigoArancelario = c.CodigoArancelario,
                    Nombre = c.Nombre,
                    PorcentajeArancel = c.PorcentajeArancel,
                    AplicaItbis = c.AplicaItbis,
                    AplicaImpuestoSelectivo = c.AplicaImpuestoSelectivo,
                    PorcentajeImpuestoSelectivo = c.PorcentajeImpuestoSelectivo,
                    Activo = c.Activo,
                    TieneProductosAsociados = c.Productos.Any()
                })
                .ToListAsync();

        }
        public async Task<CategoriaArancelariaDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _context.CategoriasArancelarias
                .Include(c => c.Productos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (entidad == null) return null;

            return new CategoriaArancelariaDto
            {
                Id = entidad.Id,
                CodigoArancelario = entidad.CodigoArancelario,
                Nombre = entidad.Nombre,
                PorcentajeArancel = entidad.PorcentajeArancel,
                AplicaItbis = entidad.AplicaItbis,
                AplicaImpuestoSelectivo = entidad.AplicaImpuestoSelectivo,
                PorcentajeImpuestoSelectivo = entidad.PorcentajeImpuestoSelectivo,
                Activo = entidad.Activo,
                TieneProductosAsociados = entidad.Productos.Any()
            };
        }
        public async Task<(bool exito, string mensaje)> CrearAsync(CategoriaArancelariaDto dto)
        {
        var codigoExiste = await _context.CategoriasArancelarias
            .AnyAsync(c => c.CodigoArancelario.ToUpper()
                    == dto.CodigoArancelario.Trim().ToUpper());

            if (codigoExiste)
                return (false, "Ya existe una categoría con este código arancelario.");

            if (dto.PorcentajeArancel < 0 || dto.PorcentajeArancel > 100)
                return (false, "El porcentaje de arancel debe estar entre 0 y 100.");

            if (dto.AplicaImpuestoSelectivo && dto.PorcentajeImpuestoSelectivo <= 0)
                return (false, "Si aplica impuesto selectivo, el porcentaje debe ser mayor que 0.");

            if (dto.AplicaImpuestoSelectivo && dto.PorcentajeImpuestoSelectivo > 100)  
                return (false, "El porcentaje de impuesto selectivo no puede ser mayor que 100.");

            if (!dto.AplicaImpuestoSelectivo)
                dto.PorcentajeImpuestoSelectivo = 0;

            var entidad = new CategoriaArancelaria
            {
                CodigoArancelario = dto.CodigoArancelario.Trim().ToUpper(),
                Nombre = dto.Nombre.Trim(),
                PorcentajeArancel = dto.PorcentajeArancel,
                AplicaItbis = dto.AplicaItbis,
                AplicaImpuestoSelectivo = dto.AplicaImpuestoSelectivo,
                PorcentajeImpuestoSelectivo = dto.PorcentajeImpuestoSelectivo,
                Activo = dto.Activo
            };

            _context.CategoriasArancelarias.Add(entidad);
            await _context.SaveChangesAsync();

            return (true, "Categoría arancelaria creada correctamente.");
        }
        public async Task<(bool exito, string mensaje)> EditarAsync(CategoriaArancelariaDto dto)
        {
            var entidad = await _context.CategoriasArancelarias
                .Include(c => c.Productos)
                .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (entidad == null)
                return (false, "Categoría arancelaria no encontrada.");

            bool tieneProductos = entidad.Productos.Any();

            
            if (tieneProductos)
            {
                entidad.Nombre = dto.Nombre.Trim();
                entidad.Activo = dto.Activo;
                await _context.SaveChangesAsync();
                return (true, "Solo se actualizaron nombre y estado porque la categoría tiene productos asociados.");
            }

            
            var codigoExiste = await _context.CategoriasArancelarias
                .AnyAsync(c => c.CodigoArancelario.ToUpper()
                    == dto.CodigoArancelario.Trim().ToUpper()
                    && c.Id != dto.Id);

            if (codigoExiste)
                return (false, "Ya existe otra categoría con este código arancelario.");

            if (dto.PorcentajeArancel < 0 || dto.PorcentajeArancel > 100)
                return (false, "El porcentaje de arancel debe estar entre 0 y 100.");

            if (dto.AplicaImpuestoSelectivo && dto.PorcentajeImpuestoSelectivo <= 0)
                return (false, "Si aplica impuesto selectivo, el porcentaje debe ser mayor que 0.");

            if (dto.AplicaImpuestoSelectivo && dto.PorcentajeImpuestoSelectivo > 100) 
                return (false, "El porcentaje de impuesto selectivo no puede ser mayor que 100.");

            if (!dto.AplicaImpuestoSelectivo)
                dto.PorcentajeImpuestoSelectivo = 0;

            // Actualizar todos los campos
            entidad.CodigoArancelario = dto.CodigoArancelario.Trim().ToUpper();
            entidad.Nombre = dto.Nombre.Trim();
            entidad.PorcentajeArancel = dto.PorcentajeArancel;
            entidad.AplicaItbis = dto.AplicaItbis;
            entidad.AplicaImpuestoSelectivo = dto.AplicaImpuestoSelectivo;
            entidad.PorcentajeImpuestoSelectivo = dto.PorcentajeImpuestoSelectivo;
            entidad.Activo = dto.Activo;

            await _context.SaveChangesAsync();
            return (true, "Categoría arancelaria actualizada correctamente.");
        }

        public async Task<(bool exito, string mensaje)> EliminarAsync(int id)
        {
            var entidad = await _context.CategoriasArancelarias
                .Include(c => c.Productos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (entidad == null)
                return (false, "Categoría arancelaria no encontrada.");

            if (entidad.Productos.Any())
                return (false, "No se puede eliminar esta categoría porque tiene productos asociados.");

            _context.CategoriasArancelarias.Remove(entidad);
            await _context.SaveChangesAsync();

            return (true, "Categoría arancelaria eliminada correctamente.");
        }
        public async Task<List<CategoriaArancelariaDto>> ObtenerCategoriasActivasAsync()
        {
            return await _context.CategoriasArancelarias
                .Where(c => c.Activo)
                .Select(c => new CategoriaArancelariaDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    CodigoArancelario = c.CodigoArancelario
                })
                .ToListAsync();
        }
    }
}
    
