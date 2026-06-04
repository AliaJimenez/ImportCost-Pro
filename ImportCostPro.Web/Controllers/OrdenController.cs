using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels.Orden;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class OrdenController(
        IOrdenImportacionService ordenService,
        IImportadorService importadorService,
        IProveedorService proveedorService,
        IPaisService paisService,
        IMonedaService monedaService,
        IOrdenGastoService gastoService,
        IOrdenProductoService productoService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var ordenes = await ordenService.GetAllAsync();
            var vm = ordenes.Select(o => new OrdenIndexViewModel
            {
                Id = o.Id,
                NumeroOrden = o.NumeroOrden,
                NombreImportador = o.NombreImportador ?? "N/A",
                NombreProveedor = o.NombreProveedor ?? "N/A",
                NombrePais = o.NombrePais ?? "N/A",
                NombreMoneda = o.NombreMoneda ?? "N/A",
                Estado = o.Estado,
                PrecioSugerido = o.PrecioSugerido,
                Activo = o.Activo,
                FechaCreacion = o.FechaCreacion
            }).ToList();

            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var orden = await ordenService.GetByIdAsync(id);
            if (orden is null) return NotFound();

            ViewBag.Productos = await productoService.ObtenerPorOrdenAsync(id) ?? [];
            ViewBag.Gastos = await gastoService.ObtenerPorOrdenAsync(id) ?? [];

            return View(orden);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new OrdenFormViewModel
            {
                NumeroOrden = string.Empty,
                ImportadorId = 0,         
                ProveedorId = 0,          
                PaisOrigenId = 0,         
                MonedaId = 0,            
                ModalidadTransporte = string.Empty, 
                FechaOrden = DateTime.Now
            };
            await CargarListas(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                FechaOrden = model.FechaOrden,
                ModalidadTransporte = model.ModalidadTransporte,
                Estado = "Abierta",
                Activo = model.Activo,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };

            await ordenService.CreateAsync(dto);
            TempData["Success"] = "Orden creada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var orden = await ordenService.GetByIdAsync(id);
            if (orden is null) return NotFound();

            var vm = new OrdenEditViewModel
            {
                Id = orden.Id,
                NumeroOrden = orden.NumeroOrden,
                ImportadorId = orden.ImportadorId,
                ProveedorId = orden.ProveedorId,
                PaisOrigenId = orden.PaisOrigenId,
                MonedaId = orden.MonedaId,
                FechaOrden = orden.FechaOrden,
                ModalidadTransporte = orden.ModalidadTransporte
            };

            await CargarListas(vm);
            ViewBag.EstadoOrden = orden.Estado;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrdenEditViewModel model)
        {
            var ordenActual = await ordenService.GetByIdAsync(model.Id);
            if (ordenActual is null) return NotFound();

            if (ordenActual.Estado != "Abierta")
            {
                TempData["Error"] = "Solo las órdenes abiertas pueden ser editadas.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                await CargarListas(model);
                return View(model);
            }

            var dto = new OrdenImportacionDto
            {
                Id = model.Id,
                NumeroOrden = model.NumeroOrden,
                ImportadorId = model.ImportadorId,
                ProveedorId = model.ProveedorId,
                PaisOrigenId = model.PaisOrigenId,
                MonedaId = model.MonedaId,
                FechaOrden = model.FechaOrden,
                ModalidadTransporte = model.ModalidadTransporte,
                Estado = ordenActual.Estado,
                Activo = model.Activo,
                FechaCreacion = ordenActual.FechaCreacion,
                FechaModificacion = DateTime.Now
            };

            await ordenService.UpdateAsync(dto);
            TempData["Success"] = "Orden actualizada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarListas(OrdenFormViewModel vm)
        {
            var importadores = await importadorService.GetActivosAsync();
            var proveedores = await proveedorService.GetActivosAsync();
            var paises = await paisService.GetActivosAsync();
            var monedas = await monedaService.ObtenerActivasAsync();

            vm.Importadores = importadores.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nombre }).ToList();
            vm.Proveedores = proveedores.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nombre }).ToList();
            vm.Paises = paises.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nombre }).ToList();
            vm.Monedas = monedas.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nombre }).ToList();
            vm.Modalidades = new List<SelectListItem> {
                new("Marítimo", "Marítimo"), new("Aéreo", "Aéreo"), new("Terrestre", "Terrestre")
            };
        }
    }
}