using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels.Orden;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class OrdenController : Controller
    {
        private readonly IOrdenImportacionService _ordenService;
        private readonly IImportadorService _importadorService;
        private readonly IProveedorService _proveedorService;
        private readonly IPaisService _paisService;
        private readonly IMonedaService _monedaService;
        private readonly IOrdenGastoService _gastoService;
        private readonly IOrdenProductoService _productoService;

        public OrdenController(
            IOrdenImportacionService ordenService,
            IImportadorService importadorService,
            IProveedorService proveedorService,
            IPaisService paisService,
            IMonedaService monedaService,
            IOrdenGastoService gastoService,
            IOrdenProductoService productoService)
        {
            _ordenService = ordenService;
            _importadorService = importadorService;
            _proveedorService = proveedorService;
            _paisService = paisService;
            _monedaService = monedaService;
            _gastoService = gastoService;
            _productoService = productoService;
        }

        public async Task<IActionResult> Index()
        {
            var ordenes = await _ordenService.GetAllAsync();
            var vm = ordenes.Select(o => new OrdenIndexViewModel
            {
                Id = o.Id,
                NumeroOrden = o.NumeroOrden,
                NombreImportador = o.ImportadorId.ToString(), // Se necesitaría un join o traer el nombre
                NombreProveedor = o.ProveedorId.ToString(),
                NombrePais = o.PaisOrigenId.ToString(),
                NombreMoneda = o.MonedaId.ToString(),
                Estado = o.Estado,
                FechaCreacion = o.FechaCreacion,
                Activo = o.Activo
            }).ToList();

            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var orden = await _ordenService.GetByIdAsync(id);
            if (orden == null) return NotFound();

            var productos = await _productoService.ObtenerPorOrdenAsync(id);
            var gastos = await _gastoService.ObtenerPorOrdenAsync(id);

            ViewBag.Productos = productos ?? new List<OrdenProductoDto>();
            ViewBag.Gastos = gastos ?? new List<OrdenGastoDto>();
            
            return View(orden);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new OrdenFormViewModel();
            await CargarListas(vm);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrdenFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarListas(model);
                return View(model);
            }

            var dto = new OrdenImportacionDto
            {
                NumeroOrden = model.NumeroOrden,
                ImportadorId = model.ImportadorId,
                ProveedorId = model.ProveedorId,
                PaisOrigenId = model.PaisOrigenId,
                MonedaId = model.MonedaId,
                Estado = "Abierta"
            };

            await _ordenService.CreateAsync(dto);
            TempData["Success"] = "Orden creada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var orden = await _ordenService.GetByIdAsync(id);
            if (orden == null) return NotFound();

            var vm = new OrdenFormViewModel
            {
                Id = orden.Id,
                NumeroOrden = orden.NumeroOrden,
                ImportadorId = orden.ImportadorId,
                ProveedorId = orden.ProveedorId,
                PaisOrigenId = orden.PaisOrigenId,
                MonedaId = orden.MonedaId
            };

            await CargarListas(vm);
            ViewBag.EstadoOrden = orden.Estado;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrdenFormViewModel model)
        {
            var orden = await _ordenService.GetByIdAsync(model.Id);
            if (orden == null) return NotFound();
            
            ViewBag.EstadoOrden = orden.Estado;

            if (orden.Estado != "Abierta")
            {
                TempData["Error"] = "Solo las órdenes abiertas pueden ser editadas.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                await CargarListas(model);
                return View(model);
            }

            orden.NumeroOrden = model.NumeroOrden;
            orden.ImportadorId = model.ImportadorId;
            orden.ProveedorId = model.ProveedorId;
            orden.PaisOrigenId = model.PaisOrigenId;
            orden.MonedaId = model.MonedaId;

            await _ordenService.UpdateAsync(orden);
            TempData["Success"] = "Orden actualizada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var orden = await _ordenService.GetByIdAsync(id);
            if (orden == null) return NotFound();
            return View(orden);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orden = await _ordenService.GetByIdAsync(id);
            if (orden == null) return NotFound();

            if (orden.Estado != "Abierta")
            {
                TempData["Error"] = "No se puede eliminar una orden que ya fue calculada o cerrada.";
                return RedirectToAction(nameof(Index));
            }

            await _ordenService.DeleteAsync(id);
            TempData["Success"] = "Orden eliminada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarListas(OrdenFormViewModel vm)
        {
            var importadores = await _importadorService.GetAllAsync();
            var proveedores = await _proveedorService.GetAllAsync();
            var paises = await _paisService.GetAllAsync();
            var monedas = await _monedaService.ObtenerTodasAsync();

            vm.Importadores = importadores.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nombre }).ToList();
            vm.Proveedores = proveedores.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nombre }).ToList();
            vm.Paises = paises.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nombre }).ToList();
            vm.Monedas = monedas.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nombre }).ToList();
        }
    }
}
