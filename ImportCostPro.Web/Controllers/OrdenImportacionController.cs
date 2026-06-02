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
   
    public class OrdenImportacionController(
        IOrdenImportacionService ordenService,
        IImportadorService importadorService,
        IProveedorService proveedorService,
        IPaisService paisService,
        IMonedaService monedaService) : Controller
    {
        // GET: OrdenImportacion
        public async Task<IActionResult> Index()
        {
            var ordenes = await ordenService.GetAllAsync();
            var viewModel = MapToIndexViewModel(ordenes);
            return View(viewModel);
        }

        // GET: OrdenImportacion/Create
        public async Task<IActionResult> Create()
        {
           
            var model = new OrdenFormViewModel
            {
                NumeroOrden = string.Empty,
                ImportadorId = 0,
                ProveedorId = 0,
                PaisOrigenId = 0,
                MonedaId = 0,
                FechaOrden = DateTime.Now.Date,
                ModalidadTransporte = string.Empty,
                Activo = true,
                Importadores = await GetImportadoresSelectList(),
                Proveedores = await GetProveedoresSelectList(),
                Paises = await GetPaisesSelectList(),
                Monedas = await GetMonedasSelectList(),
                Modalidades = GetModalidadesSelectList()
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
                model.Modalidades = GetModalidadesSelectList();
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
                    FechaOrden = model.FechaOrden,
                    ModalidadTransporte = model.ModalidadTransporte,
                    Estado = "Abierta",
                    Activo = model.Activo,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now
                };

                await ordenService.CreateAsync(ordenDto);
                TempData["Success"] = "Orden creada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.Importadores = await GetImportadoresSelectList();
                model.Proveedores = await GetProveedoresSelectList();
                model.Paises = await GetPaisesSelectList();
                model.Monedas = await GetMonedasSelectList();
                model.Modalidades = GetModalidadesSelectList();
                return View(model);
            }
        }

        // GET: OrdenImportacion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return NotFound();

            var orden = await ordenService.GetByIdAsync(id.Value);
            if (orden is null)
                return NotFound();

            if (!await ordenService.CanEditAsync(id.Value))
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
                FechaOrden = orden.FechaOrden,
                ModalidadTransporte = orden.ModalidadTransporte,
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
                Monedas = await GetMonedasSelectList(),
                Modalidades = GetModalidadesSelectList()
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
                model.Modalidades = GetModalidadesSelectList();
                return View(model);
            }

            try
            {
                var ordenActual = await ordenService.GetByIdAsync(id)
                    ?? throw new Exception("Orden no encontrada");

                var ordenDto = new OrdenImportacionDto
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

                await ordenService.UpdateAsync(ordenDto);
                TempData["Success"] = "Orden actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.Importadores = await GetImportadoresSelectList();
                model.Proveedores = await GetProveedoresSelectList();
                model.Paises = await GetPaisesSelectList();
                model.Monedas = await GetMonedasSelectList();
                model.Modalidades = GetModalidadesSelectList();
                return View(model);
            }
        }

        // GET: OrdenImportacion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return NotFound();

            var orden = await ordenService.GetByIdAsync(id.Value);
            if (orden is null)
                return NotFound();

            if (!await ordenService.CanDeleteAsync(id.Value))
            {
                TempData["Error"] = await ordenService.GetDeleteErrorMessageAsync(id.Value);
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
                Activo = orden.Activo,
                FechaCreacion = orden.FechaCreacion
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
                await ordenService.DeleteAsync(id);
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
                await ordenService.CloseOrderAsync(id);
                TempData["Success"] = "Orden cerrada correctamente. Los datos son ahora inmutables.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Edit), new { id });
            }
        }

    
        private async Task<List<SelectListItem>> GetImportadoresSelectList()
        {
            var importadores = await importadorService.GetActivosAsync();
            return importadores.Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Nombre }).ToList();
        }

        private async Task<List<SelectListItem>> GetProveedoresSelectList()
        {
            var proveedores = await proveedorService.GetActivosAsync();
            return proveedores.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Nombre }).ToList();
        }

        private async Task<List<SelectListItem>> GetPaisesSelectList()
        {
            var paises = await paisService.GetActivosAsync();
            return paises.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Nombre }).ToList();
        }

        private async Task<List<SelectListItem>> GetMonedasSelectList()
        {
            var monedas = await monedaService.ObtenerActivasAsync();
            return monedas.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
        }

        private static List<SelectListItem> GetModalidadesSelectList()
        {
            return new List<SelectListItem>
            {
                new() { Value = "Marítimo", Text = "Marítimo" },
                new() { Value = "Aéreo", Text = "Aéreo" },
                new() { Value = "Terrestre", Text = "Terrestre" }
            };
        }
        private static List<OrdenIndexViewModel> MapToIndexViewModel(IEnumerable<OrdenImportacionDto> ordenes)
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
            }).ToList();
        }
    }
}