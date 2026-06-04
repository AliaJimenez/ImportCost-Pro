using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Data.Contexts;

namespace ImportCostPro.Web.Controllers
{
    public class CalculoLandedCostController : Controller
    {
        private readonly ICalculoLandedCostService _calculoService;
        private readonly ImportCostDbContext _context;

        public CalculoLandedCostController(ICalculoLandedCostService calculoService, ImportCostDbContext context)
        {
            _calculoService = calculoService;
            _context = context;
        }

        public IActionResult Index() => RedirectToAction(nameof(Calcular));

        public async Task<IActionResult> Calcular()
        {
            try
            {
                var ordenes = await _context.OrdenesImportacion
                    .Where(o => o.Estado == "Abierta")
                    .Select(o => new { o.Id, Nombre = (o.NumeroOrden ?? "Orden-") + o.Id })
                    .ToListAsync();

                ViewBag.Ordenes = new SelectList(ordenes, "Id", "Nombre");
            }
            catch
            {
                
                ViewBag.Ordenes = new SelectList(Enumerable.Empty<object>(), "Id", "Nombre");
                TempData["ErrorMessage"] = "No se pudo cargar la lista de órdenes. Verifique que la base de datos esté migrada correctamente.";
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Calcular(int ordenImportacionId)
        {
            try
            {
                var resultadoDto = await _calculoService.CalcularLandedCostAsync(ordenImportacionId);
                
                return View("Resultado", resultadoDto);
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Calcular));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarGuardado(int ordenImportacionId)
        {
            try
            {
                var resultadoDto = await _calculoService.CalcularLandedCostAsync(ordenImportacionId);
                
                await _calculoService.GuardarCalculoOficialAsync(resultadoDto);
                
                TempData["SuccessMessage"] = "¡Cálculo de Landed Cost confirmado y guardado exitosamente!";
                return RedirectToAction("Index", "Home");
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = "Error al guardar: " + ex.Message;
                return RedirectToAction(nameof(Calcular));
            }
        }
    }
}
