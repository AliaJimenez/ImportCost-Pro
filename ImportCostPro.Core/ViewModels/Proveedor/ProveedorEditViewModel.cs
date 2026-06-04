
namespace ImportCostPro.Core.ViewModels.Proveedor
{
    public class ProveedorEditViewModel : ProveedorFormViewModel
    {
        public bool TieneOrdenes { get; set; }
        public string? NombrePais { get; set; }
        public string? NombreMoneda { get; set; }
    }
}