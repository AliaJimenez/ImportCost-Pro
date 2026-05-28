using System;

namespace ImportCostPro.Data.Entities
{
    public class ConfiguracionImpuesto
    {
        public int Id { get; set; }
        
        // Porcentaje ITBIS (0-100)
        public required decimal PorcentajeITBIS { get; set; }
        
        // Porcentaje Tasa de Servicio Aduanal (0-100)
        public required decimal PorcentajeTasaServicioAduanal { get; set; }
        
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
    }
}
