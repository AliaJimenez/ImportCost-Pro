namespace ImportCostPro.Core.Dtos
{
    public class OrdenGastoDto
    {
        public int Id { get; set; }
        public int OrdenImportacionId { get; set; }
        public int MonedaId { get; set; }
        public string TipoGasto { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string MetodoDistribucion { get; set; } = string.Empty;
        public DateTime FechaGasto { get; set; }
        public decimal MontoEnMonedaLocal { get; set; }

        // Para mostrar en vistas
        public string NumeroOrden { get; set; } = string.Empty;
        public string NombreMoneda { get; set; } = string.Empty;
        public string SimboloMoneda { get; set; } = string.Empty;
        public string EstadoOrden { get; set; } = string.Empty;

        // Para saber si la orden permite modificaciones
        public bool OrdenPermiteModificaciones { get; set; }
    }
}