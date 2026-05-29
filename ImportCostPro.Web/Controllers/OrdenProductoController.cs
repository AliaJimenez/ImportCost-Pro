using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels.OrdenProducto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class OrdenProductoController : Controller
    {
        private readonly IOrdenProductoService _service;
        private readonly IProductoService _productoService;

        public OrdenProductoController(
            IOrdenProductoService service,
            IProductoService productoService)
        {
            _service = service;
            _productoService = productoService;
        }
        public async Task<IActionResult> Index(int ordenId)
        {
            var productos = await _service.ObtenerPorOrdenAsync(ordenId);
            var resumen = await _service.ObtenerResumenFOBAsync(ordenId);

            var viewModel = new OrdenProductoIndexViewModel
            {
                OrdenImportacionId = ordenId,
                Productos = productos,
                Resumen = resumen,
                NumeroOrden = productos.FirstOrDefault()?.NumeroOrden ?? string.Empty,
                EstadoOrden = productos.FirstOrDefault()?.EstadoOrden ?? string.Empty,
                OrdenPermiteModificaciones = productos.FirstOrDefault()
                    ?.OrdenPermiteModificaciones ?? false
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Create(int ordenId)
        {
            var viewModel = new OrdenProductoFormViewModel
            {
                OrdenImportacionId = ordenId
            };

            await LlenarSelectsAsync(viewModel);
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrdenProductoFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LlenarSelectsAsync(viewModel);
                return View(viewModel);
            }

            var dto = new OrdenProductoDto
            {
                OrdenImportacionId = viewModel.OrdenImportacionId,
                ProductoId = viewModel.ProductoId,
                Cantidad = viewModel.Cantidad,
                PrecioUnitarioFOB = viewModel.PrecioUnitarioFOB,
                MargenGananciaDeseado = viewModel.MargenGananciaDeseado
            };

            var (exito, mensaje) = await _service.AgregarAsync(dto);

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
                TempData["Error"] = "No se puede editar un producto de una orden que no está abierta.";
                return RedirectToAction(nameof(Index),
                    new { ordenId = dto.OrdenImportacionId });
            }

            var viewModel = new OrdenProductoFormViewModel
            {
                Id = dto.Id,
                OrdenImportacionId = dto.OrdenImportacionId,
                ProductoId = dto.ProductoId,
                Cantidad = dto.Cantidad,
                PrecioUnitarioFOB = dto.PrecioUnitarioFOB,
                MargenGananciaDeseado = dto.MargenGananciaDeseado,
                NumeroOrden = dto.NumeroOrden
            };

            await LlenarSelectsAsync(viewModel);
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(OrdenProductoFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LlenarSelectsAsync(viewModel);
                return View(viewModel);
            }

            var dto = new OrdenProductoDto
            {
                Id = viewModel.Id,
                OrdenImportacionId = viewModel.OrdenImportacionId,
                ProductoId = viewModel.ProductoId,
                Cantidad = viewModel.Cantidad,
                PrecioUnitarioFOB = viewModel.PrecioUnitarioFOB,
                MargenGananciaDeseado = viewModel.MargenGananciaDeseado
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

        private async Task LlenarSelectsAsync(OrdenProductoFormViewModel viewModel)
        {
            var productos = await _productoService.ObtenerTodosAsync();
            var productosActivos = productos.Where(p => p.Activo).ToList();

            viewModel.ProductosDisponibles = new SelectList(
                productosActivos, "Id", "Nombre",
                viewModel.ProductoId);
        }
    }
}
