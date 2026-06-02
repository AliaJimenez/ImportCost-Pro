using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels.Pais;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImportCostPro.Web.Controllers
{
    public class PaisController(IPaisService paisService) : Controller
    {
        // GET: Pais
        public async Task<IActionResult> Index()
        {
            var paises = await paisService.GetAllAsync();
            var viewModel = MapToIndexViewModel(paises);
            return View(viewModel);
        }

        // GET: Pais/Create
        public IActionResult Create()
        {
            var model = new PaisFormViewModel
            {
                Nombre = string.Empty,
                CodigoISO = string.Empty,
                Activo = true
            };
            return View(model);
        }

        // POST: Pais/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaisFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var paisDto = new PaisDto
                {
                    Nombre = model.Nombre,
                    CodigoISO = model.CodigoISO,
                    Activo = model.Activo,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now
                };

                await paisService.CreateAsync(paisDto);
                TempData["Success"] = "País creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        // GET: Pais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return NotFound();

            var pais = await paisService.GetByIdAsync(id.Value);
            if (pais is null)
                return NotFound();

            var viewModel = new PaisFormViewModel
            {
                Id = pais.Id,
                Nombre = pais.Nombre,
                CodigoISO = pais.CodigoISO,
                Activo = pais.Activo
            };

            return View(viewModel);
        }

        // POST: Pais/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PaisFormViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var paisActual = await paisService.GetByIdAsync(id)
                    ?? throw new Exception("País no encontrado");

                var paisDto = new PaisDto
                {
                    Id = model.Id,
                    Nombre = model.Nombre,
                    CodigoISO = model.CodigoISO,
                    Activo = model.Activo,
                    FechaCreacion = paisActual.FechaCreacion,
                    FechaModificacion = DateTime.Now
                };

                await paisService.UpdateAsync(paisDto);
                TempData["Success"] = "País actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        // GET: Pais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return NotFound();

            var pais = await paisService.GetByIdAsync(id.Value);
            if (pais is null)
                return NotFound();

            if (!await paisService.CanDeleteAsync(id.Value))
            {
                TempData["Error"] = await paisService.GetDeleteErrorMessageAsync(id.Value);
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new PaisFormViewModel
            {
                Id = pais.Id,
                Nombre = pais.Nombre,
                CodigoISO = pais.CodigoISO,
                Activo = pais.Activo
            };

            return View(viewModel);
        }

        // POST: Pais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await paisService.DeleteAsync(id);
                TempData["Success"] = "País eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
        private static List<PaisIndexViewModel> MapToIndexViewModel(IEnumerable<PaisDto> paises)
        {
            return paises.Select(p => new PaisIndexViewModel
            {
                Id = p.Id,
                Nombre = p.Nombre,
                CodigoISO = p.CodigoISO,
                Activo = p.Activo,
                FechaCreacion = p.FechaCreacion
            }).ToList();
        }
    }
}