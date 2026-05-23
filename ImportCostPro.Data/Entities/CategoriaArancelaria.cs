using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ImportCostPro.Data.Entities
{
    public class CategoriaArancelaria
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public required string CodigoArancelario { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public required decimal PorcentajeArancel { get; set; }

        public required bool AplicaItbis { get; set; }
        public required bool AplicaImpuestoSelectivo { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal PorcentajeImpuestoSelectivo { get; set; }
        public required bool Activo { get; set; }= true;

        //navigation properties
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();

    }
}
