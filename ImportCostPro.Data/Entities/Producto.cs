namespace ImportCostPro.Data.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; } = string.Empty;
        public required string CodigoReferencia { get; set; } = string.Empty;
        public required decimal PesoUnitario { get; set; }
        public decimal? Largo { get; set; }
        public decimal? Ancho{ get; set; }
        public decimal? Alto { get; set; }
        public required string UnidadMedida { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public required bool Activo { get; set; } = true;

        public required int PaisOrigenId { get; set; } //fk
        public required int CategoriaArancelariaId { get; set; }//fk
   
        //navigation properties
        public Pais PaisOrigen { get; set; } = null!;
        public CategoriaArancelaria CategoriaArancelaria { get; set; } = null!;
        public ICollection<OrdenProducto> OrdenProductos { get; set; } = new List<OrdenProducto>();


    }
}
