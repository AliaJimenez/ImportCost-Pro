using System;

namespace ImportCostPro.Core.Dtos
{
    public class ConfiguracionImpuestoDto
    {
        public int Id { get; set; }
        public decimal PorcentajeITBIS { get; set; }
        public decimal PorcentajeTasaServicioAduanal { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}