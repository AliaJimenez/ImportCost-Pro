using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ImportCostPro.Web.Controllers
{
    public class ConfiguracionImpuestoController : Controller
    {
        private readonly IConfiguracionImpuestoService _service;

        public ConfiguracionImpuestoController(IConfiguracionImpuestoService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var dto = await _service.ObtenerConfiguracionAsync();
            
            var viewModel = new ConfiguracionImpuestoViewModel
            {
                Id = dto.Id,
                PorcentajeITBIS = dto.PorcentajeITBIS,
                PorcentajeTasaServicioAduanal = dto.PorcentajeTasaServicioAduanal
            };

            return View(viewModel); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConfiguracionImpuestoViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = new ConfiguracionImpuestoDto
            {
                Id = viewModel.Id,
                PorcentajeITBIS = viewModel.PorcentajeITBIS,
                PorcentajeTasaServicioAduanal = viewModel.PorcentajeTasaServicioAduanal
            };

            var (exito, mensaje) = await _service.ActualizarConfiguracionAsync(dto);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);
                return View(viewModel);
            }

            TempData["Mensaje"] = mensaje; 
            return RedirectToAction(nameof(Index)); 
        }
    }
}