
using System.ComponentModel.DataAnnotations;

namespace ImportCostPro.Core.ViewModels.Importador
{
    public class ImportadorFormViewModel
        
        {
            public int Id { get; set; }

            [Required]
            [StringLength(150)]
            public required  string Nombre { get; set; }

            [Required]
            [StringLength(20)]
            public required string Rnc { get; set; }

            [StringLength(300)]
            public required string Direccion { get; set; }

            [StringLength(100)]
            public required string Contacto { get; set; }

            [EmailAddress]
            public required string Email { get; set; }

            [Phone]
            public required string Telefono { get; set; }

            public bool Activo { get; set; } = true;
        }
}
