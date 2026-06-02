using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels.Importador;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImportCostPro.Web.Controllers
{
    public class ImportadorController : Controller
    {
        private readonly IImportadorService _importadorService;

        public ImportadorController(IImportadorService importadorService)
        {
            _importadorService = importadorService;
        }

        // GET: Importador
        public async Task<IActionResult> Index()
        {
            var importadores = await _importadorService.GetAllAsync();
            var viewModel = MapToIndexViewModel(importadores);
            return View(viewModel);
        }

        // GET: Importador/Create
        public IActionResult Create()
        {
            return View(new ImportadorFormViewModel());
        }

        // POST: Importador/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImportadorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var importadorDto = new ImportadorDto
                {
                    Nombre = model.Nombre!,
                    Rnc = model.Rnc!,
                    Direccion = model.Direccion,
                    Contacto = model.Contacto,
                    Email = model.Email,
                    Telefono = model.Telefono,
                    Activo = model.Activo
                };

                await _importadorService.CreateAsync(importadorDto);
                TempData["Success"] = "Importador creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Importador/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var importador = await _importadorService.GetByIdAsync(id.Value);
            if (importador == null)
                return NotFound();

            var viewModel = new ImportadorFormViewModel
            {
                Id = importador.Id,
                Nombre = importador.Nombre,
                Rnc = importador.Rnc,
                Direccion = importador.Direccion,
                Contacto = importador.Contacto,
                Email = importador.Email,
                Telefono = importador.Telefono,
                Activo = importador.Activo
            };

            return View(viewModel);
        }

        // POST: Importador/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ImportadorFormViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var importadorDto = new ImportadorDto
                {
                    Id = model.Id,
                    Nombre = model.Nombre!,
                    Rnc = model.Rnc!,
                    Direccion = model.Direccion,
                    Contacto = model.Contacto,
                    Email = model.Email,
                    Telefono = model.Telefono,
                    Activo = model.Activo
                };

                await _importadorService.UpdateAsync(importadorDto);
                TempData["Success"] = "Importador actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Importador/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var importador = await _importadorService.GetByIdAsync(id.Value);
            if (importador == null)
                return NotFound();

            if (!await _importadorService.CanDeleteAsync(id.Value))
            {
                TempData["Error"] = await _importadorService.GetDeleteErrorMessageAsync(id.Value);
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new ImportadorFormViewModel
            {
                Id = importador.Id,
                Nombre = importador.Nombre,
                Rnc = importador.Rnc,
                Direccion = importador.Direccion,
                Contacto = importador.Contacto,
                Email = importador.Email,
                Telefono = importador.Telefono,
                Activo = importador.Activo
            };

            return View(viewModel);
        }

        // POST: Importador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _importadorService.DeleteAsync(id);
                TempData["Success"] = "Importador eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private IEnumerable<ImportadorIndexViewModel> MapToIndexViewModel(IEnumerable<ImportadorDto> importadores)
        {
            return importadores.Select(i => new ImportadorIndexViewModel
            {
                Id = i.Id,
                Nombre = i.Nombre,
                Rnc = i.Rnc,
                Email = i.Email,
                Telefono = i.Telefono,
                Activo = i.Activo,
                FechaCreacion = i.FechaCreacion
            });
        }
    }
}
