namespace ImportCostPro.Data.Entities
{
    public class OrdenGasto
    {
        public int Id { get; set; }

        public int OrdenImportacionId { get; set; }//fk
        public int MonedaId { get; set; }//fk

        public string TipoGasto { get; set; } = string.Empty;

        public decimal Monto { get; set; }

        public string MetodoDistribucion { get; set; } = string.Empty;

        public DateTime FechaGasto { get; set; }

        public decimal MontoEnMonedaLocal { get; set; }

        // navigation properties
        public OrdenImportacion OrdenImportacion { get; set; } = null!;
        public Moneda Moneda { get; set; } = null!;
    }
}

