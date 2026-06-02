using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels.Proveedor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImportCostPro.Web.Controllers
{
    // Uso de Primary Constructor
    public class ProveedorController(
        IProveedorService proveedorService,
        IPaisService paisService,
        IMonedaService monedaService) : Controller
    {
        // GET: Proveedor
        public async Task<IActionResult> Index()
        {
            var proveedores = await proveedorService.GetAllAsync();
            var viewModel = MapToIndexViewModel(proveedores);
            return View(viewModel);
        }

        // GET: Proveedor/Create
        public async Task<IActionResult> Create()
        {
            var model = new ProveedorFormViewModel
            {
                Nombre = string.Empty,   
                PaisOrigenId = 0,      
                MonedaPrincipalId = 0,  
                Paises = await GetPaisesSelectList(),
                Monedas = await GetMonedasSelectList()
            };
            return View(model);
        }

        // POST: Proveedor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProveedorFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Paises = await GetPaisesSelectList();
                model.Monedas = await GetMonedasSelectList();
                return View(model);
            }

            try
            {
                var paisDto = await paisService.GetByIdAsync(model.PaisOrigenId);
                var monedaDto = await monedaService.ObtenerPorIdAsync(model.MonedaPrincipalId);

                var proveedorDto = new ProveedorDto
                {
                    Nombre = model.Nombre,
                    PaisOrigenId = model.PaisOrigenId,
                    NombrePais = paisDto?.Nombre,
                    MonedaPrincipalId = model.MonedaPrincipalId,
                    NombreMoneda = monedaDto?.Nombre,
                    Contacto = model.Contacto,
                    Email = model.Email,
                    Telefono = model.Telefono,
                    Direccion = model.Direccion,
                    Activo = model.Activo,
                    TieneOrdenes = false,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now
                };

                await proveedorService.CreateAsync(proveedorDto);
                TempData["Success"] = "Proveedor creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.Paises = await GetPaisesSelectList();
                model.Monedas = await GetMonedasSelectList();
                return View(model);
            }
        }

        // GET: Proveedor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return NotFound();

            var proveedor = await proveedorService.GetByIdAsync(id.Value);
            if (proveedor is null)
                return NotFound();

            bool tieneOrdenes = await proveedorService.HasOrdersAsync(id.Value);

            var viewModel = new ProveedorEditViewModel
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                PaisOrigenId = proveedor.PaisOrigenId,
                MonedaPrincipalId = proveedor.MonedaPrincipalId,
                Contacto = proveedor.Contacto,
                Email = proveedor.Email,
                Telefono = proveedor.Telefono,
                Direccion = proveedor.Direccion,
                Activo = proveedor.Activo,
                TieneOrdenes = tieneOrdenes,
                NombrePais = proveedor.NombrePais,
                NombreMoneda = proveedor.NombreMoneda,
                Paises = await GetPaisesSelectList(),
                Monedas = await GetMonedasSelectList()
            };

            return View(viewModel);
        }

        // POST: Proveedor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProveedorEditViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                model.Paises = await GetPaisesSelectList();
                model.Monedas = await GetMonedasSelectList();
                return View(model);
            }

            try
            {
                var paisDto = await paisService.GetByIdAsync(model.PaisOrigenId);
                var monedaDto = await monedaService.ObtenerPorIdAsync(model.MonedaPrincipalId);

                var proveedorActual = await proveedorService.GetByIdAsync(id)
                    ?? throw new Exception("Proveedor no encontrado");

                var proveedorDto = new ProveedorDto
                {
                    Id = model.Id,
                    Nombre = model.Nombre,
                    PaisOrigenId = model.PaisOrigenId,
                    NombrePais = paisDto?.Nombre,
                    MonedaPrincipalId = model.MonedaPrincipalId,
                    NombreMoneda = monedaDto?.Nombre,
                    Contacto = model.Contacto,
                    Email = model.Email,
                    Telefono = model.Telefono,
                    Direccion = model.Direccion,
                    Activo = model.Activo,
                    TieneOrdenes = proveedorActual.TieneOrdenes,
                    FechaCreacion = proveedorActual.FechaCreacion,
                    FechaModificacion = DateTime.Now
                };

                await proveedorService.UpdateAsync(proveedorDto);
                TempData["Success"] = "Proveedor actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.Paises = await GetPaisesSelectList();
                model.Monedas = await GetMonedasSelectList();
                return View(model);
            }
        }

        // GET: Proveedor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return NotFound();

            var proveedor = await proveedorService.GetByIdAsync(id.Value);
            if (proveedor is null)
                return NotFound();

            if (!await proveedorService.CanDeleteAsync(id.Value))
            {
                TempData["Error"] = await proveedorService.GetDeleteErrorMessageAsync(id.Value);
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new ProveedorFormViewModel
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                PaisOrigenId = proveedor.PaisOrigenId,
                MonedaPrincipalId = proveedor.MonedaPrincipalId,
                Contacto = proveedor.Contacto,
                Email = proveedor.Email,
                Telefono = proveedor.Telefono,
                Direccion = proveedor.Direccion,
                Activo = proveedor.Activo
            };

            return View(viewModel);
        }

        // POST: Proveedor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await proveedorService.DeleteAsync(id);
                TempData["Success"] = "Proveedor eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task<List<SelectListItem>> GetPaisesSelectList()
        {
            var paises = await paisService.GetActivosAsync();
            return paises.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Nombre
            }).ToList();
        }

        private async Task<List<SelectListItem>> GetMonedasSelectList()
        {
            var monedas = await monedaService.ObtenerActivasAsync();
            return monedas.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Nombre
            }).ToList();
        }

        // Convertido a estático y retornando List<T>
        private static List<ProveedorIndexViewModel> MapToIndexViewModel(IEnumerable<ProveedorDto> proveedores)
        {
            return proveedores.Select(p => new ProveedorIndexViewModel
            {
                Id = p.Id,
                Nombre = p.Nombre,
                NombrePais = p.NombrePais,
                NombreMoneda = p.NombreMoneda,
                Email = p.Email,
                Activo = p.Activo,
                TieneOrdenes = p.TieneOrdenes,
                FechaCreacion = p.FechaCreacion
            }).ToList();
        }
    }
}