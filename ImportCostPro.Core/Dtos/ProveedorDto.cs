using System;

namespace ImportCostPro.Core.Dtos
{
    public class ProveedorDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int PaisOrigenId { get; set; }
        public string? NombrePais { get; set; }  // ✅ NULLABLE
        public int MonedaPrincipalId { get; set; }
        public string? NombreMoneda { get; set; }  // ✅ NULLABLE
        public string ? Contacto { get; set; }
        public string ? Email { get; set; }
        public string ? Telefono { get; set; }
        public string ? Direccion { get; set; }
        public bool Activo { get; set; }
        public bool TieneOrdenes { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
