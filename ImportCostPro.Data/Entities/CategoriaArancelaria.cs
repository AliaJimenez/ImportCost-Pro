namespace ImportCostPro.Data.Entities
{
    public class CategoriaArancelaria
    {
        public int Id { get; set; }

        public required string CodigoArancelario { get; set; }

        public required string Nombre { get; set; }

        public required decimal PorcentajeArancel { get; set; }

        public required bool AplicaItbis { get; set; }
        public required bool AplicaImpuestoSelectivo { get; set; }

        public decimal PorcentajeImpuestoSelectivo { get; set; }
        public required bool Activo { get; set; }= true;

        //navigation properties
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();

    }
}
