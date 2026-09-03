namespace ImportCostPro.Core.Dtos
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CodigoReferencia { get; set; } = string.Empty;
        public decimal PesoUnitario { get; set; }
        public decimal? Largo { get; set; }
        public decimal? Ancho { get; set; }
        public decimal? Alto { get; set; }
        public string UnidadMedida { get; set; } = string.Empty;
        public string? Descripcion {  get; set; }
        public bool Activo { get; set; }

        public int PaisOrigenId { get; set; }
        public int CategoriaArancelariaId { get; set; }

        public string ? NombrePais { get; set; }
        public string ? NombreCategoria { get; set; }

        public bool TieneOrdenesAsociadas { get; set; }


    }
}
