using System;
using System.Collections.Generic;

namespace ImportCostPro.Data.Entities
{
    public class Pais
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }

        public required string CodigoISO
        {
            get => _codigoISO;
            set => _codigoISO = value?.ToUpper() ?? string.Empty;
        }
        private string _codigoISO = string.Empty;

        public required bool Activo { get; set; } = true;

        public required DateTime FechaCreacion { get; set; } = DateTime.Now;  
        public required DateTime FechaModificacion { get; set; } = DateTime.Now; 
        public ICollection<Proveedor> Proveedores { get; set; } = [];
        public ICollection<Producto> Productos { get; set; } = [];
    }
}