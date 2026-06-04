using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels; 
using Microsoft.AspNetCore.Mvc;

namespace ImportCostPro.Web.Controllers
{
    public class MonedaController : Controller
    {
        private readonly IMonedaService _service;

        public MonedaController(IMonedaService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var monedas = await _service.ObtenerTodasAsync();
            return View(monedas);
        }

        public IActionResult Create()
        {
            return View(new MonedaViewModel { Activo = true }); // Por defecto activa
        }

        [HttpPost]
        public async Task<IActionResult> Create(MonedaViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = new MonedaDto
            {
                CodigoISO = viewModel.CodigoISO,
                Nombre = viewModel.Nombre,
                Simbolo = viewModel.Simbolo,
                EsMonedaLocal = viewModel.EsMonedaLocal,
                Activo = viewModel.Activo
            };

            var (exito, mensaje) = await _service.CrearAsync(dto);

            if (!exito)
            { 
                ModelState.AddModelError("", mensaje); // Agrega el error general al Summary
                return View(viewModel);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.ObtenerPorIdAsync(id);

            if (dto == null)
                return NotFound();

            var viewModel = new MonedaViewModel
            {
                Id = dto.Id,
                CodigoISO = dto.CodigoISO,
                Nombre = dto.Nombre,
                Simbolo = dto.Simbolo,
                EsMonedaLocal = dto.EsMonedaLocal,
                Activo = dto.Activo
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MonedaViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = new MonedaDto
            {
                Id = viewModel.Id,
                CodigoISO = viewModel.CodigoISO,
                Nombre = viewModel.Nombre,
                Simbolo = viewModel.Simbolo,
                EsMonedaLocal = viewModel.EsMonedaLocal,
                Activo = viewModel.Activo
            };

            var (exito, mensaje) = await _service.EditarAsync(dto);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);
                return View(viewModel);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.ObtenerPorIdAsync(id);

            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (exito, mensaje) = await _service.EliminarAsync(id);

            if (!exito)
            {
                TempData["Error"] = mensaje;
                return RedirectToAction(nameof(Delete), new { id });
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction(nameof(Index));
        }
    }
}