using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class OrdenProductoController : Controller
    {
        private readonly IOrdenProductoService _productoService;
        private readonly IProductoService _catalogoProductoService;
        private readonly IOrdenImportacionService _ordenService;

        public OrdenProductoController(
            IOrdenProductoService productoService, 
            IProductoService catalogoProductoService,
            IOrdenImportacionService ordenService)
        {
            _productoService = productoService;
            _catalogoProductoService = catalogoProductoService;
            _ordenService = ordenService;
        }

        public async Task<IActionResult> Index(int? filtroOrdenId)
        {
            var todosLosProductos = new List<OrdenProductoDto>();
            if (filtroOrdenId.HasValue)
            {
                todosLosProductos = await _productoService.ObtenerPorOrdenAsync(filtroOrdenId.Value);
            }
            return View(todosLosProductos);
        }

        public async Task<IActionResult> Create(int ordenId)
        {
            var orden = await _ordenService.GetByIdAsync(ordenId);
            if (orden == null || orden.Estado != "Abierta") return RedirectToAction("Details", "Orden", new { id = ordenId });

            var dto = new OrdenProductoDto
            {
                OrdenImportacionId = ordenId
            };

            await CargarProductosCat(null);
            ViewBag.OrdenId = ordenId;
            ViewBag.NumeroOrden = orden.NumeroOrden;
            
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrdenProductoDto model)
        {
            if (!ModelState.IsValid)
            {
                await CargarProductosCat(model.ProductoId);
                return View(model);
            }

            var (exito, mensaje) = await _productoService.AgregarAsync(model);
            if (exito)
            {
                TempData["Success"] = "Producto agregado correctamente.";
                return RedirectToAction("Details", "Orden", new { id = model.OrdenImportacionId });
            }
            
            TempData["Error"] = mensaje;
            await CargarProductosCat(model.ProductoId);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var op = await _productoService.ObtenerPorIdAsync(id);
            if (op == null) return NotFound();

            await CargarProductosCat(op.ProductoId);
            return View(op);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrdenProductoDto model)
        {
            if (!ModelState.IsValid)
            {
                await CargarProductosCat(model.ProductoId);
                return View(model);
            }

            var (exito, mensaje) = await _productoService.EditarAsync(model);
            if (exito)
            {
                TempData["Success"] = "Producto actualizado correctamente.";
                return RedirectToAction("Details", "Orden", new { id = model.OrdenImportacionId });
            }

            TempData["Error"] = mensaje;
            await CargarProductosCat(model.ProductoId);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var op = await _productoService.ObtenerPorIdAsync(id);
            if (op == null) return NotFound();
            return View(op);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, int OrdenImportacionId)
        {
            var (exito, mensaje) = await _productoService.EliminarAsync(id);
            if (exito)
            {
                TempData["Success"] = "Producto eliminado correctamente.";
            }
            else
            {
                TempData["Error"] = mensaje;
            }
            return RedirectToAction("Details", "Orden", new { id = OrdenImportacionId });
        }

        private async Task CargarProductosCat(int? productoId)
        {
            var productos = await _catalogoProductoService.ObtenerTodosAsync();
            ViewBag.ProductosCat = new SelectList(productos, "Id", "Nombre", productoId);
        }
    }
}
