using System;

namespace ImportCostPro.Core.Dtos
{
    public class TasaCambioDto
    {
        public int Id { get; set; }
        public int MonedaOrigenId { get; set; }
        public string ? NombreMonedaOrigen { get; set; }
        public int MonedaDestinoId { get; set; }
        public string ? NombreMonedaDestino { get; set; }
        public decimal Tasa { get; set; }
        public DateTime FechaVigencia { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
