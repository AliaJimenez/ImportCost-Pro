using System;
using System.Collections.Generic;

namespace ImportCostPro.Data.Entities
{
    public class Moneda
    {
        public int Id { get; set; }
        
        // Código ISO de 3 caracteres (USD, EUR, DOP, etc.)
        public required string CodigoISO { get; set; }
        
        // Nombre de la moneda
        public required string Nombre { get; set; }
        
        // Símbolo ($, €, etc.)
        public required string Simbolo { get; set; }
        
        // Una única moneda local activa en el sistema
        public required bool EsMonedaLocal { get; set; } = false;
        
        // Estado
        public required bool Activo { get; set; } = true;
        
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
        
        // Navegaciones
        public ICollection<TasaCambio> TasasCambioOrigen { get; set; } = new List<TasaCambio>();
        public ICollection<TasaCambio> TasasCambioDestino { get; set; } = new List<TasaCambio>();
    }
}
