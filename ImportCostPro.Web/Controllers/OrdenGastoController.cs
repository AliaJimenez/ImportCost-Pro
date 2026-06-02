using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels.OrdenGasto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class OrdenGastoController : Controller
    {
        private readonly IOrdenGastoService _gastoService;
        private readonly IMonedaService _monedaService;
        private readonly IOrdenImportacionService _ordenService;

        public OrdenGastoController(
            IOrdenGastoService gastoService, 
            IMonedaService monedaService,
            IOrdenImportacionService ordenService)
        {
            _gastoService = gastoService;
            _monedaService = monedaService;
            _ordenService = ordenService;
        }

        public async Task<IActionResult> Index(int? filtroOrdenId)
        {
            var ordenesAbiertas = await _gastoService.ObtenerOrdenesAbiertasAsync();
            ViewBag.Ordenes = new SelectList(ordenesAbiertas, "Id", "NumeroOrden", filtroOrdenId);
            
            var todosLosGastos = new List<OrdenGastoDto>();
            
            if (filtroOrdenId.HasValue)
            {
                todosLosGastos = await _gastoService.ObtenerPorOrdenAsync(filtroOrdenId.Value);
            }
            // Para simplificar, si no hay filtro, podríamos traer todos, 
            // pero IOrdenGastoService solo tiene ObtenerPorOrdenAsync en la interfaz. 
            // Así que si es nulo devolvemos vacío y obligamos a seleccionar.
            
            return View(todosLosGastos);
        }

        public async Task<IActionResult> Create(int ordenId)
        {
            var orden = await _ordenService.GetByIdAsync(ordenId);
            if (orden == null || orden.Estado != "Abierta") return RedirectToAction(nameof(Index));

            var dto = new OrdenGastoDto
            {
                OrdenImportacionId = ordenId,
                FechaGasto = DateTime.Today
            };

            await CargarMonedas(null);
            ViewBag.OrdenId = ordenId;
            ViewBag.NumeroOrden = orden.NumeroOrden;
            
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrdenGastoDto model)
        {
            if (!ModelState.IsValid)
            {
                await CargarMonedas(model.MonedaId);
                return View(model);
            }

            var (exito, mensaje) = await _gastoService.RegistrarAsync(model);
            if (exito)
            {
                TempData["Success"] = "Gasto registrado correctamente.";
                return RedirectToAction("Details", "Orden", new { id = model.OrdenImportacionId });
            }
            
            TempData["Error"] = mensaje;
            await CargarMonedas(model.MonedaId);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var gasto = await _gastoService.ObtenerPorIdAsync(id);
            if (gasto == null) return NotFound();

            await CargarMonedas(gasto.MonedaId);
            return View(gasto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrdenGastoDto model)
        {
            if (!ModelState.IsValid)
            {
                await CargarMonedas(model.MonedaId);
                return View(model);
            }

            var (exito, mensaje) = await _gastoService.EditarAsync(model);
            if (exito)
            {
                TempData["Success"] = "Gasto actualizado correctamente.";
                return RedirectToAction("Details", "Orden", new { id = model.OrdenImportacionId });
            }

            TempData["Error"] = mensaje;
            await CargarMonedas(model.MonedaId);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var gasto = await _gastoService.ObtenerPorIdAsync(id);
            if (gasto == null) return NotFound();
            return View(gasto);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, int OrdenImportacionId)
        {
            var (exito, mensaje) = await _gastoService.EliminarAsync(id);
            if (exito)
            {
                TempData["Success"] = "Gasto eliminado correctamente.";
            }
            else
            {
                TempData["Error"] = mensaje;
            }
            return RedirectToAction("Details", "Orden", new { id = OrdenImportacionId });
        }

        private async Task CargarMonedas(int? monedaId)
        {
            var monedas = await _monedaService.ObtenerTodasAsync();
            ViewBag.Monedas = new SelectList(monedas, "Id", "Nombre", monedaId);
        }
    }
}
