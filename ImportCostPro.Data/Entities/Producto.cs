using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ImportCostPro.Data.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string CodigoReferencia { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public required decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Largo { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Ancho{ get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Alto { get; set; }
        public required string UnidadMedida { get; set; }
        public string? Descripcion { get; set; }
        public required bool Activo { get; set; } = true;

        public required int PaisOrigenId { get; set; }
        public required int CategoriaArancelariaId { get; set; }
        public Pais PaisOrigen { get; set; }
        public CategoriaArancelaria CategoriaArancelaria { get; set; } 


    }
}
