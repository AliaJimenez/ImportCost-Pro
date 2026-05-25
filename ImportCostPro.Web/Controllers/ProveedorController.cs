using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels.Proveedor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly IProveedorService _proveedorService;
        private readonly IPaisService _paisService;
        private readonly IMonedaService _monedaService;

        public ProveedorController(IProveedorService proveedorService, IPaisService paisService, IMonedaService monedaService)
        {
            _proveedorService = proveedorService;
            _paisService = paisService;
            _monedaService = monedaService;
        }

        // GET: Proveedor
        public async Task<IActionResult> Index()
        {
            var proveedores = await _proveedorService.GetAllAsync();
            var viewModel = MapToIndexViewModel(proveedores);
            return View(viewModel);
        }

        // GET: Proveedor/Create
        public async Task<IActionResult> Create()
        {
            var model = new ProveedorFormViewModel
            {
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
                var paisDto = await _paisService.GetByIdAsync(model.PaisOrigenId);
                var monedaDto = await _monedaService.GetByIdAsync(model.MonedaPrincipalId);

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
                    Activo = model.Activo
                };

                await _proveedorService.CreateAsync(proveedorDto);
                TempData["Success"] = "Proveedor creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.Paises = await GetPaisesSelectList();
                model.Monedas = await GetMonedasSelectList();
                return View(model);
            }
        }

        // GET: Proveedor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var proveedor = await _proveedorService.GetByIdAsync(id.Value);
            if (proveedor == null)
                return NotFound();

            bool tieneOrdenes = await _proveedorService.HasOrdersAsync(id.Value);

            var viewModel = new ProveedorEditViewModel
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                PaisOrigenId = proveedor.PaisOrigenId,
                MonedaPrincipalId = proveedor.MonedaPrincipalId,
                Contacto = proveedor.Contacto,  // ✅ AGREGAR
                Email = proveedor.Email,
                Telefono = proveedor.Telefono,
                Direccion = proveedor.Direccion,
                Activo = proveedor.Activo,
                TieneOrdenes = tieneOrdenes,
                NombrePais = proveedor.NombrePais,
                NombreMoneda = proveedor.NombreMoneda,
                Paises = await GetPaisesSelectList(),  // ✅ AGREGAR
                Monedas = await GetMonedasSelectList()  // ✅ AGREGAR
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
                var paisDto = await _paisService.GetByIdAsync(model.PaisOrigenId);
                var monedaDto = await _monedaService.GetByIdAsync(model.MonedaPrincipalId);

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
                    Activo = model.Activo
                };

                await _proveedorService.UpdateAsync(proveedorDto);
                TempData["Success"] = "Proveedor actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.Paises = await GetPaisesSelectList();
                model.Monedas = await GetMonedasSelectList();
                return View(model);
            }
        }
        // GET: Proveedor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var proveedor = await _proveedorService.GetByIdAsync(id.Value);
            if (proveedor == null)
                return NotFound();

            if (!await _proveedorService.CanDeleteAsync(id.Value))
            {
                TempData["Error"] = await _proveedorService.GetDeleteErrorMessageAsync(id.Value);
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
                await _proveedorService.DeleteAsync(id);
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
            var paises = await _paisService.GetActivosAsync();
            return paises.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Nombre
            }).ToList();
        }

        private async Task<List<SelectListItem>> GetMonedasSelectList()
        {
            var monedas = await _monedaService.GetActivasAsync();
            return monedas.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Nombre
            }).ToList();
        }

        private IEnumerable<ProveedorIndexViewModel> MapToIndexViewModel(IEnumerable<ProveedorDto> proveedores)
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
            });
        }
    }
}
