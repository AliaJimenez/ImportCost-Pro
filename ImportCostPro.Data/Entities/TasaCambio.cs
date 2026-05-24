using System;

namespace ImportCostPro.Data.Entities
{
    public class TasaCambio
    {
        public int Id { get; set; }
        
        // Moneda origen
        public int MonedaOrigenId { get; set; }
        public Moneda ? MonedaOrigen { get; set; }
        
        // Moneda destino
        public int MonedaDestinoId { get; set; }
        public Moneda ? MonedaDestino { get; set; }
        
        // Tasa (factor de conversión - decimal con 6 decimales)
        public decimal Tasa { get; set; }
        
        // Fecha de vigencia de la tasa
        public DateTime FechaVigencia { get; set; }
        
        // Estado
        public bool Activo { get; set; } = true;
        
        // Auditoría
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
    }
}