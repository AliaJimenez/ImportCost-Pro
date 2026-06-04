using ImportCostPro.Core.Dtos;

namespace ImportCostPro.Core.Interfaces
{
    public interface ICalculoLandedCostService
    {
        Task<CalculoLandedCostDto> CalcularLandedCostAsync(int ordenImportacionId);
        
        Task GuardarCalculoOficialAsync(CalculoLandedCostDto calculoDto);
    }
}
