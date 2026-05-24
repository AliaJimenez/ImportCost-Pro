using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Data.Contexts;
using ImportCostPro.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class ConfiguracionImpuestoService : IConfiguracionImpuestoService
    {
        private readonly ImportCostDbContext _context;

        public ConfiguracionImpuestoService(ImportCostDbContext context)
        {
            _context = context;
        }

        public async Task<ConfiguracionImpuestoDto> ObtenerConfiguracionAsync()
        {
            var entidad = await _context.ConfiguracionesImpuestos.FirstOrDefaultAsync();
            
            if (entidad == null)
            {
                // Valores por defecto
                return new ConfiguracionImpuestoDto { PorcentajeITBIS = 18m, PorcentajeTasaServicioAduanal = 0m };
            }

            return new ConfiguracionImpuestoDto
            {
                Id = entidad.Id,
                PorcentajeITBIS = entidad.PorcentajeITBIS,
                PorcentajeTasaServicioAduanal = entidad.PorcentajeTasaServicioAduanal
            };
        }

        public async Task<(bool exito, string mensaje)> ActualizarConfiguracionAsync(ConfiguracionImpuestoDto dto)
        {
            if (dto.PorcentajeITBIS < 0 || dto.PorcentajeITBIS > 100 || dto.PorcentajeTasaServicioAduanal < 0 || dto.PorcentajeTasaServicioAduanal > 100)
            {
                return (false, "Operación rechazada: Las tasas deben tener valores entre 0 y 100.");
            }

            var entidad = await _context.ConfiguracionesImpuestos.FirstOrDefaultAsync();

            if (entidad == null)
            {
                entidad = new ConfiguracionImpuesto
                {
                    PorcentajeITBIS = dto.PorcentajeITBIS,
                    PorcentajeTasaServicioAduanal = dto.PorcentajeTasaServicioAduanal,
                    FechaModificacion = DateTime.Now
                };
                _context.ConfiguracionesImpuestos.Add(entidad);
            }
            else
            {
                entidad.PorcentajeITBIS = dto.PorcentajeITBIS;
                entidad.PorcentajeTasaServicioAduanal = dto.PorcentajeTasaServicioAduanal;
                entidad.FechaModificacion = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return (true, "Configuración de impuestos guardada correctamente.");
        }
    }
}