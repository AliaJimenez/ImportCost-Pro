namespace ImportCostPro.Core.Dtos
{
    public class CategoriaArancelariaDto
    {
        public int Id { get; set; }
        public string CodigoArancelario { get; set; }
        public string Nombre { get; set; }
        public decimal PorcentajeArancel { get; set; }    
        public bool AplicaItbis { get; set; }
        public bool AplicaImpuestoSelectivo { get; set; }
        public decimal PorcentajeImpuestoSelectivo { get; set; }
        public bool Activo {  get; set; }

        public bool TieneProductosAsociados { get; set; }
    }
}
