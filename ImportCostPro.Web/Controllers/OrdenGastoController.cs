using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels.OrdenGasto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class OrdenGastoController : Controller
    {
        private readonly IOrdenGastoService _service;
        private readonly IMonedaService _monedaService;

        public OrdenGastoController(
            IOrdenGastoService service,
            IMonedaService monedaService)
        {
            _service = service;
            _monedaService = monedaService;
        }
        public async Task<IActionResult> Index(int ordenId)
        {
            var gastos = await _service.ObtenerPorOrdenAsync(ordenId);

            var viewModel = new OrdenGastoIndexViewModel
            {
                OrdenImportacionId = ordenId,
                Gastos = gastos,
                NumeroOrden = gastos.FirstOrDefault()?.NumeroOrden ?? string.Empty,
                EstadoOrden = gastos.FirstOrDefault()?.EstadoOrden ?? string.Empty,
                OrdenPermiteModificaciones = gastos.FirstOrDefault()
                    ?.OrdenPermiteModificaciones ?? false
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Create(int ordenId)
        {
            var viewModel = new OrdenGastoFormViewModel
            {
                OrdenImportacionId = ordenId
            };

            await LlenarSelectsAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrdenGastoFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LlenarSelectsAsync(viewModel);
                return View(viewModel);
            }

            var dto = new OrdenGastoDto
            {
                OrdenImportacionId = viewModel.OrdenImportacionId,
                MonedaId = viewModel.MonedaId,
                TipoGasto = viewModel.TipoGasto,
                Monto = viewModel.Monto,
                MetodoDistribucion = viewModel.MetodoDistribucion,
                FechaGasto = viewModel.FechaGasto
            };

            var (exito, mensaje) = await _service.RegistrarAsync(dto);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);
                await LlenarSelectsAsync(viewModel);
                return View(viewModel);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction(nameof(Index),
                new { ordenId = viewModel.OrdenImportacionId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.ObtenerPorIdAsync(id);

            if (dto == null)
                return NotFound();

            if (!dto.OrdenPermiteModificaciones)
            {
                TempData["Error"] = "No se puede editar un gasto de una orden que no está abierta.";
                return RedirectToAction(nameof(Index),
                    new { ordenId = dto.OrdenImportacionId });
            }

            var viewModel = new OrdenGastoFormViewModel
            {
                Id = dto.Id,
                OrdenImportacionId = dto.OrdenImportacionId,
                MonedaId = dto.MonedaId,
                TipoGasto = dto.TipoGasto,
                Monto = dto.Monto,
                MetodoDistribucion = dto.MetodoDistribucion,
                FechaGasto = dto.FechaGasto,
                NumeroOrden = dto.NumeroOrden
            };

            await LlenarSelectsAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrdenGastoFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LlenarSelectsAsync(viewModel);
                return View(viewModel);
            }

            var dto = new OrdenGastoDto
            {
                Id = viewModel.Id,
                OrdenImportacionId = viewModel.OrdenImportacionId,
                MonedaId = viewModel.MonedaId,
                TipoGasto = viewModel.TipoGasto,
                Monto = viewModel.Monto,
                MetodoDistribucion = viewModel.MetodoDistribucion,
                FechaGasto = viewModel.FechaGasto
            };

            var (exito, mensaje) = await _service.EditarAsync(dto);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);
                await LlenarSelectsAsync(viewModel);
                return View(viewModel);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction(nameof(Index),
                new { ordenId = viewModel.OrdenImportacionId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.ObtenerPorIdAsync(id);

            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, int ordenId)
        {
            var (exito, mensaje) = await _service.EliminarAsync(id);

            if (!exito)
            {
                TempData["Error"] = mensaje;
                return RedirectToAction(nameof(Delete), new { id });
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction(nameof(Index), new { ordenId });
        }

        private async Task LlenarSelectsAsync(OrdenGastoFormViewModel viewModel)
        {
            viewModel.TiposGastoDisponibles = new SelectList(new List<string>
            {
                "FleteInternacional",
                "SeguroInternacional",
                "GastosPortuarios",
                "TransporteLocal",
                "HonorariosAduanales",
                "Almacenaje",
                "ManejoCarga",
                "OtrosGastos"
            }, viewModel.TipoGasto);

            viewModel.MetodosDistribucionDisponibles = new SelectList(new List<string>
            {
                "PorValorFOB",
                "PorPeso",
                "PorVolumen",
                "PorCantidad"
            }, viewModel.MetodoDistribucion);

            var monedas = await _monedaService.ObtenerActivasAsync();
            viewModel.MonedasDisponibles = new SelectList(
                monedas, "Id", "Nombre",
                viewModel.MonedaId);
        }
    }
}
