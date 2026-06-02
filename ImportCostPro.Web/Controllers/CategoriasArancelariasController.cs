using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels.CategoriaArancelaria;
using Microsoft.AspNetCore.Mvc;

namespace ImportCostPro.Web.Controllers
{
    public class CategoriasArancelariasController : Controller
    {
        private readonly ICategoriaArancelariaService _service;
        public CategoriasArancelariasController(
            ICategoriaArancelariaService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var categorias = await _service.ObtenerTodasAsync();
            return View(categorias);
        }
        public IActionResult Create()
        {
            return View(new CategoriaArancelariaViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoriaArancelariaViewModel viewModel)
        {
            
            if (!ModelState.IsValid)
            
                return View(viewModel);

            var dto = new CategoriaArancelariaDto
            {
                CodigoArancelario = viewModel.CodigoArancelario,
                Nombre = viewModel.Nombre,
                PorcentajeArancel = viewModel.PorcentajeArancel,
                AplicaItbis = viewModel.AplicaItbis,
                AplicaImpuestoSelectivo = viewModel.AplicaImpuestoSelectivo,
                PorcentajeImpuestoSelectivo = viewModel.PorcentajeImpuestoSelectivo,
                Activo = viewModel.Activo
            };
            var (exito, mensaje) = await _service.CrearAsync(dto);

            if (!exito)
            { 
                ModelState.AddModelError("Nombre", mensaje);
                return View(viewModel);
            }
            TempData["Mensaje"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.ObtenerPorIdAsync(id);

            if (dto == null)
                return NotFound();

            var viewModel = new CategoriaArancelariaViewModel
            {
                Id = dto.Id,
                CodigoArancelario = dto.CodigoArancelario,
                Nombre = dto.Nombre,
                PorcentajeArancel = dto.PorcentajeArancel,
                AplicaItbis = dto.AplicaItbis,
                AplicaImpuestoSelectivo = dto.AplicaImpuestoSelectivo,
                PorcentajeImpuestoSelectivo = dto.PorcentajeImpuestoSelectivo,
                Activo = dto.Activo,
                TieneProductosAsociados = dto.TieneProductosAsociados
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(
            CategoriaArancelariaViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = new CategoriaArancelariaDto
            {
                Id = viewModel.Id,
                CodigoArancelario = viewModel.CodigoArancelario,
                Nombre = viewModel.Nombre,
                PorcentajeArancel = viewModel.PorcentajeArancel,
                AplicaItbis = viewModel.AplicaItbis,
                AplicaImpuestoSelectivo = viewModel.AplicaImpuestoSelectivo,
                PorcentajeImpuestoSelectivo = viewModel.PorcentajeImpuestoSelectivo,
                Activo = viewModel.Activo
            };

            var (exito, mensaje) = await _service.EditarAsync(dto);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);
                return View(viewModel);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.ObtenerPorIdAsync(id);

            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (exito, mensaje) = await _service.EliminarAsync(id);

            if (!exito)
            {
                TempData["Error"] = mensaje;
                return RedirectToAction(nameof(Delete), new { id });
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction(nameof(Index));
        }
    }
}
