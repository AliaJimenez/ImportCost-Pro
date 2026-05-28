namespace ImportCostPro.Core.Dtos
{
    public class ResumenFOBDto
    {
        public decimal CantidadTotal { get; set; }
        public decimal FOBTotal { get; set; }
        public decimal PesoTotal { get; set; }
        public decimal? VolumenTotal { get; set; }
        public string MonedaOrden { get; set; } = string.Empty;
        public string SimboloMoneda { get; set; } = string.Empty;
    }
}
