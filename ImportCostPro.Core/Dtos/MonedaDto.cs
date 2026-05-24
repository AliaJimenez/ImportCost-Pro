using System;

namespace ImportCostPro.Core.Dtos
{
    public class MonedaDto
    {
        public int Id { get; set; }
        public string ? CodigoISO { get; set; }
        public string ? Nombre { get; set; }
        public string ? Simbolo { get; set; }
        public bool EsMonedaLocal { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
