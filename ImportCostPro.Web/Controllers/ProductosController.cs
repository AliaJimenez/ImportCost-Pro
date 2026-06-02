using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.ViewModels.Producto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ImportCostPro.Web.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly ICategoriaArancelariaService _categoriaService;
        private readonly IPaisService _paisService;
        

        public ProductosController(
            IProductoService productoService,
            ICategoriaArancelariaService categoriaService,
            IPaisService paisService)
        {
            _productoService = productoService;
            _categoriaService = categoriaService;
            _paisService = paisService;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.ObtenerTodosAsync();
            return View(productos);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new ProductoViewModel();
            await LlenarSelectsAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LlenarSelectsAsync(viewModel);
                return View(viewModel);
            }

            var dto = ViewModelToDto(viewModel);
            var (exito, mensaje) = await _productoService.CrearAsync(dto);

            if (!exito)
            {
                ModelState.AddModelError("Nombre", mensaje);
                await LlenarSelectsAsync(viewModel);
                return View(viewModel);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _productoService.ObtenerPorIdAsync(id);

            if (dto == null)
                return NotFound();

            var viewModel = new ProductoViewModel
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                CodigoReferencia = dto.CodigoReferencia,
                PesoUnitario = dto.PesoUnitario,
                Largo = dto.Largo,
                Ancho = dto.Ancho,
                Alto = dto.Alto,
                UnidadMedida = dto.UnidadMedida,
                Descripcion = dto.Descripcion,
                Activo = dto.Activo,
                PaisOrigenId = dto.PaisOrigenId,
                CategoriaArancelariaId = dto.CategoriaArancelariaId,
                TieneOrdenesAsociadas = dto.TieneOrdenesAsociadas
            };

            await LlenarSelectsAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LlenarSelectsAsync(viewModel);
                return View(viewModel);
            }

            var dto = ViewModelToDto(viewModel);
            var (exito, mensaje) = await _productoService.EditarAsync(dto);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);
                await LlenarSelectsAsync(viewModel);
                return View(viewModel);
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _productoService.ObtenerPorIdAsync(id);

            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (exito, mensaje) = await _productoService.EliminarAsync(id);

            if (!exito)
            {
                TempData["Error"] = mensaje;
                return RedirectToAction(nameof(Delete), new { id });
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToAction(nameof(Index));
        }
        private async Task LlenarSelectsAsync(ProductoViewModel viewModel)
        {
            var categorias = await _categoriaService
                .ObtenerCategoriasActivasAsync();

            viewModel.CategoriasDisponibles = new SelectList(
                categorias, "Id", "Nombre",
                viewModel.CategoriaArancelariaId);

            viewModel.UnidadesMedidaDisponibles = new SelectList(
                new List<string>
                {
                    "Unidad", "Caja", "Paquete",
                    "Docena", "Galón", "Metro"
                },
                viewModel.UnidadMedida);

            var paises = await _paisService.GetActivosAsync();
            viewModel.PaisesDisponibles = new SelectList(
               paises, "Id", "Nombre",
               viewModel.PaisOrigenId);
        }

        private ProductoDto ViewModelToDto(ProductoViewModel viewModel)
        {
            return new ProductoDto
            {
                Id = viewModel.Id,
                Nombre = viewModel.Nombre,
                CodigoReferencia = viewModel.CodigoReferencia,
                PesoUnitario = viewModel.PesoUnitario,
                Largo = viewModel.Largo,
                Ancho = viewModel.Ancho,
                Alto = viewModel.Alto,
                UnidadMedida = viewModel.UnidadMedida,
                Descripcion = viewModel.Descripcion,
                Activo = viewModel.Activo,
                PaisOrigenId = viewModel.PaisOrigenId,
                CategoriaArancelariaId = viewModel.CategoriaArancelariaId
            };
        }
    }
}

