using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels.Pais;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImportCostPro.Web.Controllers
{
    public class PaisController : Controller
    {
        private readonly IPaisService _paisService;

        public PaisController(IPaisService paisService)
        {
            _paisService = paisService;
        }

        // GET: Pais
        public async Task<IActionResult> Index()
        {
            var paises = await _paisService.GetAllAsync();
            var viewModel = MapToIndexViewModel(paises);
            return View(viewModel);
        }

        // GET: Pais/Create
        public IActionResult Create()
        {
            return View(new PaisFormViewModel());
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
                    Activo = model.Activo
                };

                await _paisService.CreateAsync(paisDto);
                TempData["Success"] = "País creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Pais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var pais = await _paisService.GetByIdAsync(id.Value);
            if (pais == null)
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
                var paisDto = new PaisDto
                {
                    Id = model.Id,
                    Nombre = model.Nombre,
                    CodigoISO = model.CodigoISO,
                    Activo = model.Activo
                };

                await _paisService.UpdateAsync(paisDto);
                TempData["Success"] = "País actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Pais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var pais = await _paisService.GetByIdAsync(id.Value);
            if (pais == null)
                return NotFound();

            if (!await _paisService.CanDeleteAsync(id.Value))
            {
                TempData["Error"] = await _paisService.GetDeleteErrorMessageAsync(id.Value);
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
                await _paisService.DeleteAsync(id);
                TempData["Success"] = "País eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private IEnumerable<PaisIndexViewModel> MapToIndexViewModel(IEnumerable<PaisDto> paises)
        {
            return paises.Select(p => new PaisIndexViewModel
            {
                Id = p.Id,
                Nombre = p.Nombre,
                CodigoISO = p.CodigoISO,
                Activo = p.Activo,
                FechaCreacion = p.FechaCreacion
            });
        }
    }
}