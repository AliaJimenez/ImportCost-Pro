namespace ImportCostPro.Core.Dtos
{
    public class OrdenProductoDto
    {
        public int Id { get; set; }
        public int OrdenImportacionId { get; set; }
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitarioFOB { get; set; }
        public decimal MargenGananciaDeseado { get; set; }

        // Calculados
        public decimal FOBTotal { get; set; }
        public decimal PesoTotal { get; set; }
        public decimal? VolumenTotal { get; set; }

        // Para mostrar en vistas sin cargar objetos completos
        public string NombreProducto { get; set; } = string.Empty;
        public string CodigoProducto { get; set; } = string.Empty;
        public string NumeroOrden { get; set; } = string.Empty;
        public string EstadoOrden { get; set; } = string.Empty;

        // Para saber si la orden permite modificaciones
        public bool OrdenPermiteModificaciones { get; set; }
    }
}