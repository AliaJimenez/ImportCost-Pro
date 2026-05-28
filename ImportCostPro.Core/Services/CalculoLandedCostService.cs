using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ImportCostPro.Core.Services
{
    public class CalculoLandedCostService : ICalculoLandedCostService
    {
        private readonly ImportCostDbContext _context;

        public CalculoLandedCostService(ImportCostDbContext context)
        {
            _context = context;
        }

        public async Task<CalculoLandedCostDto> CalcularLandedCostAsync(int ordenImportacionId)
        {
            // 1. OBTENER LA ORDEN (Se asume que Waldin ya mapeó el Id y Número de Orden)
            var orden = await _context.OrdenesImportacion
                .FirstOrDefaultAsync(o => o.Id == ordenImportacionId);

            if (orden == null)
                throw new Exception("La orden de importación no existe.");

            // TO DO: Cuando Waldin exponga la propiedad "Estado", habilitar esta línea:
            // if (orden.Estado != "Abierta") throw new Exception("Solo se pueden calcular órdenes en estado 'Abierta'.");

            /* TO DO: Cuando Yailyn fusione las tablas OrdenProducto y OrdenGasto:
                1. Validar que la orden tenga al menos un producto (Any()).
                2. Validar que tenga exactamente un Gasto de Tipo "Flete".
                3. Validar que tenga exactamente un Gasto de Tipo "Seguro".
                4. Buscar las tasas de cambio de Moneda Extranjera a Local y validarlas.*/

            // Aquí irá el bucle maestro que iterará sobre Yailyn's OrdenProductos
            // calculando factor de flete, peso, volumen y aplicando Arancel/ITBIS.


            return new CalculoLandedCostDto
            {
                OrdenImportacionId = orden.Id,
                NumeroOrden = orden.NumeroOrden ?? "SIMULADA-001",
                FechaCalculo = DateTime.Now,
                FobTotalLocal = 10000m,
                FleteTotalLocal = 1500m,
                SeguroTotalLocal = 200m,
                GastosLocalesTotal = 300m,
                CifTotalLocal = 11700m,
                TotalArancel = 2340m,       // Asumiendo un 20%
                TotalItbis = 2527m,         // (11700 + 2340) * 18%
                CostoTotalImportacion = 16867m,
                Detalles = new List<CalculoLandedCostDetalleDto>
                {
                    new CalculoLandedCostDetalleDto
                    {
                        ProductoId = 1,
                        NombreProducto = "Producto Simulado para Armar la Vista",
                        Cantidad = 10,
                        FobOriginalUnitario = 1000m,
                        ValorCif = 1170m,
                        CostoTotalImportado = 1686.7m,
                        CostoUnitarioImportado = 168.67m,
                        MargenDeseadoAplicado = 30m,
                        PrecioVentaSugerido = 240.95m
                    }
                }
            };
        }

        public async Task GuardarCalculoOficialAsync(CalculoLandedCostDto calculoDto)
        {
            // 1. Aquí mapearemos el DTO hacia tus entidades: CalculoLandedCost y CalculoLandedCostDetalle
            // 2. Ejecutaremos _context.CalculosLandedCost.Add(entidad);
            // 3. Cambiaremos el estado de la Orden de Waldin a "Calculada".
            // 4. Ejecutaremos _context.SaveChangesAsync();

            // Esto se descomentará en la fase final de integración.
            await Task.CompletedTask;
        }
    }
}