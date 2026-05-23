using System;

namespace ImportCostPro.Core.Dtos
{
    public class ProveedorDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public int PaisOrigenId { get; set; }
        public required string NombrePais { get; set; }          
        public int MonedaPrincipalId { get; set; }     //pendiente a moneda ali tomarse en cuenta
        public required string NombreMoneda { get; set; }    //pendiente a la moneda ali tomarse en cuenta

        public required string Contacto { get; set; }
        public  required string Email { get; set; }
        public required string Telefono { get; set; }
        public required string Direccion { get; set; }
        public bool Activo { get; set; }
        public bool TieneOrdenes { get; set; }        
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
