using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Data.Entities
{
    public class Pais
    {
        public int Id { get; set; }

    
        public required string Nombre { get; set; }

        public required string CodigoISO 
        {
            get => _codigoISO;
            set => _codigoISO = value.ToUpper();
        }
        private string _codigoISO = string.Empty;

        public required bool Activo { get; set; } = true;

        public ICollection<Proveedor> Proveedores { get; set; } = new List<Proveedor>();
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
    }
}