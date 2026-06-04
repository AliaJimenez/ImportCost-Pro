using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels.Importador;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImportCostPro.Web.Controllers
{
    public class ImportadorController(
        IImportadorService importadorService,
        IPaisService paisService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var importadores = await importadorService.GetAllAsync();
            var viewModel = MapToIndexViewModel(importadores);
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var model = new ImportadorFormViewModel
            {
                Nombre = string.Empty,
                Rnc = string.Empty,
                Pais = 0,
                Activo = true,
                Paises = await GetPaisesSelectList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ImportadorFormViewModel model)
        {
            if (model.Pais <= 0)
                ModelState.AddModelError("Pais", "Debe seleccionar un país.");

            if (!ModelState.IsValid)
            {
                model.Paises = await GetPaisesSelectList();
                return View(model);
            }

            try
            {
                var importadorDto = new ImportadorDto
                {
                    Nombre = model.Nombre,
                    Rnc = model.Rnc,
                    PaisId = model.Pais,
                    Direccion = model.Direccion,
                    Email = model.Email,
                    Telefono = model.Telefono,
                    Contacto = model.Contacto,
                    Activo = model.Activo,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now
                };

                await importadorService.CreateAsync(importadorDto);
                TempData["Success"] = "Importador creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.Paises = await GetPaisesSelectList();
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return NotFound();

            var importador = await importadorService.GetByIdAsync(id.Value);
            if (importador is null)
                return NotFound();

            var viewModel = new ImportadorFormViewModel
            {
                Id = importador.Id,
                Nombre = importador.Nombre,
                Rnc = importador.Rnc,
                Pais = importador.PaisId,
                Direccion = importador.Direccion,
                Email = importador.Email,
                Telefono = importador.Telefono,
                Contacto = importador.Contacto,
                Activo = importador.Activo,
                Paises = await GetPaisesSelectList(importador.PaisId) 
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ImportadorFormViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (model.Pais <= 0)
                ModelState.AddModelError("Pais", "Debe seleccionar un país.");

            if (!ModelState.IsValid)
            {
                model.Paises = await GetPaisesSelectList(model.Pais);
                return View(model);
            }

            try
            {
                var importadorActual = await importadorService.GetByIdAsync(id)
                    ?? throw new Exception("Importador no encontrado");

                var importadorDto = new ImportadorDto
                {
                    Id = model.Id,
                    Nombre = model.Nombre,
                    Rnc = model.Rnc,
                    PaisId = model.Pais,
                    Direccion = model.Direccion,
                    Email = model.Email,
                    Telefono = model.Telefono,
                    Contacto = model.Contacto,
                    Activo = model.Activo,
                    FechaCreacion = importadorActual.FechaCreacion,
                    FechaModificacion = DateTime.Now
                };

                await importadorService.UpdateAsync(importadorDto);
                TempData["Success"] = "Importador actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.Paises = await GetPaisesSelectList(model.Pais); 
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return NotFound();

            var importador = await importadorService.GetByIdAsync(id.Value);
            if (importador is null)
                return NotFound();

            if (!await importadorService.CanDeleteAsync(id.Value))
            {
                TempData["Error"] = await importadorService.GetDeleteErrorMessageAsync(id.Value);
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new ImportadorFormViewModel
            {
                Id = importador.Id,
                Nombre = importador.Nombre,
                Rnc = importador.Rnc,
                Pais = importador.PaisId,
                Direccion = importador.Direccion,
                Email = importador.Email,
                Telefono = importador.Telefono,
                Contacto = importador.Contacto,
                Activo = importador.Activo
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await importadorService.DeleteAsync(id);
                TempData["Success"] = "Importador eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task<List<SelectListItem>> GetPaisesSelectList(int paisActualId = 0)
        {
            var paises = (await paisService.GetActivosAsync()).ToList();

            if (paisActualId > 0 && !paises.Any(p => p.Id == paisActualId))
            {
                var paisInactivo = await paisService.GetByIdAsync(paisActualId);
                if (paisInactivo != null)
                {
                    paisInactivo.Nombre = $"{paisInactivo.Nombre} (Inactivo)";
                    paises = paises.Append(paisInactivo).ToList();
                }
            }

            return paises.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Nombre
            }).ToList();
        }

        private static List<ImportadorIndexViewModel> MapToIndexViewModel(IEnumerable<ImportadorDto> importadores)
        {
            return importadores.Select(i => new ImportadorIndexViewModel
            {
                Id = i.Id,
                Nombre = i.Nombre,
                Rnc = i.Rnc,
                NombrePais = i.NombrePais,
                Email = i.Email,
                Telefono = i.Telefono,
                Activo = i.Activo,
                TieneOrdenes = i.TieneOrdenes,
                FechaCreacion = i.FechaCreacion
            }).ToList();
        }
    }
}