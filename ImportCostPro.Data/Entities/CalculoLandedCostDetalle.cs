namespace ImportCostPro.Data.Entities
{
    public class CalculoLandedCostDetalle
    {
        public int Id { get; set; }
        public int CalculoLandedCostId { get; set; }
        public CalculoLandedCost ? CalculoLandedCost { get; set; }
        
        public int ProductoId { get; set; }
        public Producto ? Producto { get; set; }
        
        public decimal Cantidad { get; set; }
        public decimal FobOriginalUnitario { get; set; }
        public decimal FobLocalTotal { get; set; }
        
        // Porciones asignadas mediante el prorrateo
        public decimal FleteAsignado { get; set; }
        public decimal SeguroAsignado { get; set; }
        public decimal GastosLocalesAsignados { get; set; }
        
        public decimal ValorCif { get; set; }
        
        // Impuestos individuales
        public decimal MontoArancel { get; set; }
        public decimal MontoIsc { get; set; }
        public decimal MontoTasaServicio { get; set; }
        public decimal MontoItbis { get; set; }
        
        // Resultados finales
        public decimal CostoTotalImportado { get; set; }
        public decimal CostoUnitarioImportado { get; set; }
        public decimal MargenDeseadoAplicado { get; set; }
        public decimal PrecioVentaSugerido { get; set; }
    }
}