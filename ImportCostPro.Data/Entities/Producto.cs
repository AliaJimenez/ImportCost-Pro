namespace ImportCostPro.Data.Entities
{
    public class Producto
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }
        public required string CodigoReferencia { get; set; }
        public required decimal PesoUnitario { get; set; }
        public decimal? Largo { get; set; }
        public decimal? Ancho{ get; set; }
        public decimal? Alto { get; set; }
        public required string UnidadMedida { get; set; }
        public string? Descripcion { get; set; }
        public required bool Activo { get; set; } = true;


        public required int PaisOrigenId { get; set; }
        public required int CategoriaArancelariaId { get; set; }
        public Pais PaisOrigen { get; set; }
        public CategoriaArancelaria CategoriaArancelaria { get; set; } 

        //public required int PaisOrigenId { get; set; }//fk
       // public required int CategoriaArancelariaId { get; set; }//fk

        //navigation properties
        //cuando se agregue el DbSet<Pais> tengo q descomentar 
        //public Pais PaisOrigen { get; set; }

       // public CategoriaArancelaria CategoriaArancelaria { get; set; } 

       // public CategoriaArancelaria CategoriaArancelaria { get; set; } 

    }
}
