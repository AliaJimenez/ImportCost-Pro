namespace ImportCostPro.Core.Dtos
{
    public class CalculoLandedCostDetalleDto
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        
        public decimal Cantidad { get; set; }
        public decimal FobOriginalUnitario { get; set; }
        public decimal FobLocalTotal { get; set; }
        
        public decimal FleteAsignado { get; set; }
        public decimal SeguroAsignado { get; set; }
        public decimal GastosLocalesAsignados { get; set; }
        
        public decimal ValorCif { get; set; }
        
        public decimal MontoArancel { get; set; }
        public decimal MontoIsc { get; set; }
        public decimal MontoTasaServicio { get; set; }
        public decimal MontoItbis { get; set; }
        
        public decimal CostoTotalImportado { get; set; }
        public decimal CostoUnitarioImportado { get; set; }
        public decimal MargenDeseadoAplicado { get; set; }
        public decimal PrecioVentaSugerido { get; set; }
    }
}