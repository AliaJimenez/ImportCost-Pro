using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels.Orden;

namespace ImportCostPro.Web.Controllers
{
    public class OrdenImportacionController : Controller
    {
        private readonly IOrdenImportacionService _ordenService;
        private readonly IImportadorService _importadorService;
        private readonly IProveedorService _proveedorService;
        private readonly IPaisService _paisService;
        private readonly IMonedaService _monedaService;

        public OrdenImportacionController(
            IOrdenImportacionService ordenService,
            IImportadorService importadorService,
            IProveedorService proveedorService,
            IPaisService paisService,
            IMonedaService monedaService)
        {
            _ordenService = ordenService;
            _importadorService = importadorService;
            _proveedorService = proveedorService;
            _paisService = paisService;
            _monedaService = monedaService;
        }

        // GET: OrdenImportacion
        public async Task<IActionResult> Index()
        {
            var ordenes = await _ordenService.GetAllAsync();
            var viewModel = MapToIndexViewModel(ordenes);
            return View(viewModel);
        }

        // GET: OrdenImportacion/Create
        public async Task<IActionResult> Create()
        {
            var model = new OrdenFormViewModel
            {
                Importadores = await GetImportadoresSelectList(),
                Proveedores = await GetProveedoresSelectList(),
                Paises = await GetPaisesSelectList(),
                Monedas = await GetMonedasSelectList()
            };
            return View(model);
        }

        // POST: OrdenImportacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrdenFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Importadores = await GetImportadoresSelectList();
                model.Proveedores = await GetProveedoresSelectList();
                model.Paises = await GetPaisesSelectList();
                model.Monedas = await GetMonedasSelectList();
                return View(model);
            }

            try
            {
                var ordenDto = new OrdenImportacionDto
                {
                    NumeroOrden = model.NumeroOrden,
                    ImportadorId = model.ImportadorId,
                    ProveedorId = model.ProveedorId,
                    PaisOrigenId = model.PaisOrigenId,
                    MonedaId = model.MonedaId,
                    Activo = model.Activo
                };

                await _ordenService.CreateAsync(ordenDto);
                TempData["Success"] = "Orden creada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.Importadores = await GetImportadoresSelectList();
                model.Proveedores = await GetProveedoresSelectList();
                model.Paises = await GetPaisesSelectList();
                model.Monedas = await GetMonedasSelectList();
                return View(model);
            }
        }

        // GET: OrdenImportacion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var orden = await _ordenService.GetByIdAsync(id.Value);
            if (orden == null)
                return NotFound();

            if (!await _ordenService.CanEditAsync(id.Value))
            {
                TempData["Error"] = "Solo se pueden editar órdenes en estado 'Abierta'.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new OrdenEditViewModel
            {
                Id = orden.Id,
                NumeroOrden = orden.NumeroOrden,
                ImportadorId = orden.ImportadorId,
                ProveedorId = orden.ProveedorId,
                PaisOrigenId = orden.PaisOrigenId,
                MonedaId = orden.MonedaId,
                Estado = orden.Estado,
                CostoFOB = orden.CostoFOB,
                CIF = orden.CIF,
                Arancel = orden.Arancel,
                ITBIS = orden.ITBIS,
                PrecioSugerido = orden.PrecioSugerido,
                Activo = orden.Activo,
                FechaCreacion = orden.FechaCreacion,
                FechaCierre = orden.FechaCierre,
                Importadores = await GetImportadoresSelectList(),
                Proveedores = await GetProveedoresSelectList(),
                Paises = await GetPaisesSelectList(),
                Monedas = await GetMonedasSelectList()
            };

            return View(viewModel);
        }

        // POST: OrdenImportacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrdenEditViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                model.Importadores = await GetImportadoresSelectList();
                model.Proveedores = await GetProveedoresSelectList();
                model.Paises = await GetPaisesSelectList();
                model.Monedas = await GetMonedasSelectList();
                return View(model);
            }

            try
            {
                var ordenDto = new OrdenImportacionDto
                {
                    Id = model.Id,
                    NumeroOrden = model.NumeroOrden,
                    ImportadorId = model.ImportadorId,
                    ProveedorId = model.ProveedorId,
                    PaisOrigenId = model.PaisOrigenId,
                    MonedaId = model.MonedaId,
                    Activo = model.Activo
                };

                await _ordenService.UpdateAsync(ordenDto);
                TempData["Success"] = "Orden actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.Importadores = await GetImportadoresSelectList();
                model.Proveedores = await GetProveedoresSelectList();
                model.Paises = await GetPaisesSelectList();
                model.Monedas = await GetMonedasSelectList();
                return View(model);
            }
        }

        // GET: OrdenImportacion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var orden = await _ordenService.GetByIdAsync(id.Value);
            if (orden == null)
                return NotFound();

            if (!await _ordenService.CanDeleteAsync(id.Value))
            {
                TempData["Error"] = await _ordenService.GetDeleteErrorMessageAsync(id.Value);
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new OrdenIndexViewModel
            {
                Id = orden.Id,
                NumeroOrden = orden.NumeroOrden,
                NombreImportador = orden.NombreImportador,
                NombreProveedor = orden.NombreProveedor,
                NombrePais = orden.NombrePais,
                Estado = orden.Estado,
                Activo = orden.Activo
            };

            return View(viewModel);
        }

        // POST: OrdenImportacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _ordenService.DeleteAsync(id);
                TempData["Success"] = "Orden eliminada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: OrdenImportacion/Close/5  Cierre de Orden
        [HttpPost]
        public async Task<IActionResult> CloseOrder(int id)
        {
            try
            {
                await _ordenService.CloseOrderAsync(id);
                TempData["Success"] = "Orden cerrada correctamente. Los datos son ahora inmutables.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Edit), new { id });
            }
        }

        // Métodos Privados
        private async Task<List<SelectListItem>> GetImportadoresSelectList()
        {
            var importadores = await _importadorService.GetActivosAsync();
            return importadores.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.Nombre
            }).ToList();
        }

        private async Task<List<SelectListItem>> GetProveedoresSelectList()
        {
            var proveedores = await _proveedorService.GetActivosAsync();
            return proveedores.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Nombre
            }).ToList();
        }

        private async Task<List<SelectListItem>> GetPaisesSelectList()
        {
            var paises = await _paisService.GetActivosAsync();
            return paises.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Nombre
            }).ToList();
        }

        private async Task<List<SelectListItem>> GetMonedasSelectList()
        {
            var monedas = await _monedaService.ObtenerActivasAsync();
            return monedas.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Nombre
            }).ToList();
        }

        private IEnumerable<OrdenIndexViewModel> MapToIndexViewModel(IEnumerable<OrdenImportacionDto> ordenes)
        {
            return ordenes.Select(o => new OrdenIndexViewModel
            {
                Id = o.Id,
                NumeroOrden = o.NumeroOrden,
                NombreImportador = o.NombreImportador,
                NombreProveedor = o.NombreProveedor,
                NombrePais = o.NombrePais,
                NombreMoneda = o.NombreMoneda,
                Estado = o.Estado,
                PrecioSugerido = o.PrecioSugerido,
                Activo = o.Activo,
                FechaCreacion = o.FechaCreacion
            });
        }
    }
}