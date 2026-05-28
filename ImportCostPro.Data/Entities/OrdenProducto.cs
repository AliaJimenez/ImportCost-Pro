namespace ImportCostPro.Data.Entities
{
    public class OrdenProducto
    {
        public int Id { get; set; }
        public int OrdenImportacionId { get; set; } //fk
        public int ProductoId { get; set; } //fk

        public decimal Cantidad { get; set; }
        public decimal PrecioUnitarioFOB { get; set; }
        public decimal MargenGananciaDeseado { get; set; }
        public decimal FOBTotal { get; set; }
        public decimal PesoTotal { get; set; }
        public decimal? VolumenTotal { get; set; }

        // navigation properties
        public OrdenImportacion OrdenImportacion { get; set; } = null!;
        public Producto Producto { get; set; } = null!;
    }
}

