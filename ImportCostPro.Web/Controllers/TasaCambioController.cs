using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class TasaCambioController : Controller
    {
        private readonly ITasaCambioService _tasaCambioService;
        private readonly IMonedaService _monedaService; 

        public TasaCambioController(ITasaCambioService tasaCambioService, IMonedaService monedaService)
        {
            _tasaCambioService = tasaCambioService;
            _monedaService = monedaService;
        }

        public async Task<IActionResult> Index()
        {
            var tasas = await _tasaCambioService.ObtenerTodasAsync();
            return View(tasas);
        }

        private async Task CargarViewBagsMonedas()
        {
            var monedas = await _monedaService.ObtenerActivasAsync();
            var listaMonedas = monedas.Select(m => new { Id = m.Id, NombreISO = $"{m.Nombre} ({m.CodigoISO})" });
            
            ViewBag.MonedasOrigen = new SelectList(listaMonedas, "Id", "NombreISO");
            ViewBag.MonedasDestino = new SelectList(listaMonedas, "Id", "NombreISO");
        }

        public async Task<IActionResult> Create()
        {
            await CargarViewBagsMonedas();
            return View(new TasaCambioViewModel { FechaVigencia = DateTime.Now, Activo = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TasaCambioViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await CargarViewBagsMonedas();
                return View(viewModel);
            }

            var dto = new TasaCambioDto
            {
                MonedaOrigenId = viewModel.MonedaOrigenId,
                MonedaDestinoId = viewModel.MonedaDestinoId,
                Tasa = viewModel.Tasa,
                FechaVigencia = viewModel.FechaVigencia,
                Activo = viewModel.Activo
            };

            var (exito, mensaje) = await _tasaCambioService.CrearAsync(dto);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);
                await CargarViewBagsMonedas();
                return View(viewModel);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _tasaCambioService.ObtenerPorIdAsync(id);
            if (dto == null) return NotFound();

            await CargarViewBagsMonedas();
            
            var viewModel = new TasaCambioViewModel
            {
                Id = dto.Id,
                MonedaOrigenId = dto.MonedaOrigenId,
                MonedaDestinoId = dto.MonedaDestinoId,
                Tasa = dto.Tasa,
                FechaVigencia = dto.FechaVigencia,
                Activo = dto.Activo
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TasaCambioViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await CargarViewBagsMonedas();
                return View(viewModel);
            }

            var dto = new TasaCambioDto
            {
                Id = viewModel.Id,
                MonedaOrigenId = viewModel.MonedaOrigenId,
                MonedaDestinoId = viewModel.MonedaDestinoId,
                Tasa = viewModel.Tasa,
                FechaVigencia = viewModel.FechaVigencia,
                Activo = viewModel.Activo
            };

            var (exito, mensaje) = await _tasaCambioService.EditarAsync(dto);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);
                await CargarViewBagsMonedas();
                return View(viewModel);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _tasaCambioService.ObtenerPorIdAsync(id);
            if (dto == null) return NotFound();

            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (exito, mensaje) = await _tasaCambioService.EliminarAsync(id);

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