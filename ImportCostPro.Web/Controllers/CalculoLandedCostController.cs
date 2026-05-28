using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
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

        // GET: /CalculoLandedCost/Calcular
        public async Task<IActionResult> Calcular()
        {
            // Buscamos las órdenes para el Select (Dropdown).
            // Cuando Ken termine, filtraremos por .Where(o => o.Estado == "Abierta")
            var ordenes = await _context.OrdenesImportacion
                .Select(o => new { o.Id, Nombre = (o.NumeroOrden ?? "Orden-") + o.Id })
                .ToListAsync();

            ViewBag.Ordenes = new SelectList(ordenes, "Id", "Nombre");
            return View();
        }

        // POST: /CalculoLandedCost/Calcular
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

        // POST: /CalculoLandedCost/ConfirmarGuardado
        [HttpPost]
        public async Task<IActionResult> ConfirmarGuardado(int ordenImportacionId)
        {
            try
            {
                var resultadoDto = await _calculoService.CalcularLandedCostAsync(ordenImportacionId);
                
                // Llamamos al servicio para guardar en la BD Inmutable y cambiar estado
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