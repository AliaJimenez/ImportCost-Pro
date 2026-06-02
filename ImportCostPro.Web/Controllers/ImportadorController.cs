using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels.Importador;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImportCostPro.Web.Controllers
{
    public class ImportadorController(
        IImportadorService importadorService,
        IPaisService paisService) : Controller
    {
        // GET: Importador
        public async Task<IActionResult> Index()
        {
            var importadores = await importadorService.GetAllAsync();
            var viewModel = MapToIndexViewModel(importadores);
            return View(viewModel);
        }

        // GET: Importador/Create
        public async Task<IActionResult> Create()
        {
            var model = new ImportadorFormViewModel
            {
                Nombre = string.Empty,  
                Rnc = string.Empty,     
                PaisId = 0,             
                Activo = true,
                Paises = await GetPaisesSelectList()
            };
            return View(model);
        }

        // POST: Importador/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImportadorFormViewModel model)
        {
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
                    PaisId = model.PaisId,
                    Direccion = model.Direccion,
                    Email = model.Email,
                    Telefono = model.Telefono,
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

        // GET: Importador/Edit/5
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
                PaisId = importador.PaisId,
                Direccion = importador.Direccion,
                Email = importador.Email,
                Telefono = importador.Telefono,
                Activo = importador.Activo,
                Paises = await GetPaisesSelectList()
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
            {
                model.Paises = await GetPaisesSelectList();
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
                    PaisId = model.PaisId,
                    Direccion = model.Direccion,
                    Email = model.Email,
                    Telefono = model.Telefono,
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
                model.Paises = await GetPaisesSelectList();
                return View(model);
            }
        }

        // GET: Importador/Delete/5
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
                PaisId = importador.PaisId,
                Direccion = importador.Direccion,
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

        private async Task<List<SelectListItem>> GetPaisesSelectList()
        {
            var paises = await paisService.GetActivosAsync();
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
                Telefono = i.Telefono,
                Activo = i.Activo,
                TieneOrdenes = i.TieneOrdenes, 
                FechaCreacion = i.FechaCreacion
            }).ToList();
        }
    }
}