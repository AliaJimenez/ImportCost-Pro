using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImportCostPro.Core.Dtos;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Data.Contexts;
using ImportCostPro.Data.Entities;
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
            var orden = await _context.OrdenesImportacion
                .Include(o => o.Moneda)
                .Include(o => o.Productos) 
                    .ThenInclude(op => op.Producto)
                        .ThenInclude(p => p.CategoriaArancelaria)
                .Include(o => o.Gastos) 
                .FirstOrDefaultAsync(o => o.Id == ordenImportacionId);

            if (orden == null)
                throw new Exception("La orden de importación no existe.");

            if (orden.Estado != "Abierta")
                throw new Exception("Solo se pueden calcular órdenes en estado 'Abierta'.");

            if (orden.Productos == null || !orden.Productos.Any())
                throw new Exception("La orden debe tener al menos un producto registrado para poder calcular el Landed Cost.");

            var flete = orden.Gastos?.FirstOrDefault(g => g.TipoGasto.Contains("Flete", StringComparison.OrdinalIgnoreCase));
            var seguro = orden.Gastos?.FirstOrDefault(g => g.TipoGasto.Contains("Seguro", StringComparison.OrdinalIgnoreCase));

            if (flete == null) throw new Exception("Falta registrar el gasto obligatorio de 'Flete Internacional'.");
            if (seguro == null) throw new Exception("Falta registrar el gasto obligatorio de 'Seguro Internacional'.");

            var configFiscal = await _context.ConfiguracionesImpuestos.FirstOrDefaultAsync();
            if (configFiscal == null) throw new Exception("No hay configuración de impuestos generales en el sistema.");

            var monedaLocal = await _context.Monedas.FirstOrDefaultAsync(m => m.EsMonedaLocal);
            if (monedaLocal == null) throw new Exception("No hay una moneda marcada como local en el sistema.");

            // Tasa de cambio de la Orden (Aplica solo a FOB)
            decimal tasaCambioOrden = 1m; 
            if (orden.MonedaId != monedaLocal.Id)
            {
                var tasa = await _context.TasasCambio
                    .OrderByDescending(t => t.FechaVigencia)
                    .FirstOrDefaultAsync(t => t.MonedaOrigenId == orden.MonedaId && t.MonedaDestinoId == monedaLocal.Id);
                if (tasa == null) throw new Exception("No se encontró tasa de cambio para la moneda de la orden.");
                tasaCambioOrden = tasa.Tasa;
            }

            // Tasa de cambio individual por Gasto (Flete)
            decimal tasaFlete = 1m;
            if (flete.MonedaId != monedaLocal.Id)
            {
                var tFlete = await _context.TasasCambio.OrderByDescending(t => t.FechaVigencia)
                    .FirstOrDefaultAsync(t => t.MonedaOrigenId == flete.MonedaId && t.MonedaDestinoId == monedaLocal.Id);
                if (tFlete == null) throw new Exception("No se encontró tasa de cambio para la moneda del Flete.");
                tasaFlete = tFlete.Tasa;
            }

            // Tasa de cambio individual por Gasto (Seguro)
            decimal tasaSeguro = 1m;
            if (seguro.MonedaId != monedaLocal.Id)
            {
                var tSeguro = await _context.TasasCambio.OrderByDescending(t => t.FechaVigencia)
                    .FirstOrDefaultAsync(t => t.MonedaOrigenId == seguro.MonedaId && t.MonedaDestinoId == monedaLocal.Id);
                if (tSeguro == null) throw new Exception("No se encontró tasa de cambio para la moneda del Seguro.");
                tasaSeguro = tSeguro.Tasa;
            }

            decimal fleteLocal = flete.Monto * tasaFlete;
            decimal seguroLocal = seguro.Monto * tasaSeguro;

            // Tasas de cambio individuales por Gastos Locales
            var gastosLocales = orden.Gastos.Where(g => g.Id != flete.Id && g.Id != seguro.Id).ToList();
            var gastosLocalesConvertidos = new List<(OrdenGasto Gasto, decimal MontoLocal)>();
            decimal totalGastosLocalesMonedaLocal = 0m;

            foreach (var g in gastosLocales)
            {
                decimal tGasto = 1m;
                if (g.MonedaId != monedaLocal.Id)
                {
                    var tLoc = await _context.TasasCambio.OrderByDescending(t => t.FechaVigencia)
                        .FirstOrDefaultAsync(t => t.MonedaOrigenId == g.MonedaId && t.MonedaDestinoId == monedaLocal.Id);
                    if (tLoc == null) throw new Exception($"No se encontró tasa para el gasto: {g.TipoGasto}.");
                    tGasto = tLoc.Tasa;
                }
                decimal montoGastoLocal = g.Monto * tGasto;
                gastosLocalesConvertidos.Add((g, montoGastoLocal));
                totalGastosLocalesMonedaLocal += montoGastoLocal;
            }

            // Denominadores de la Orden
            decimal totalFobOriginal = orden.Productos.Sum(p => p.Cantidad * p.FOBTotal);
            decimal totalFobLocal = totalFobOriginal * tasaCambioOrden;
            decimal totalPeso = orden.Productos.Sum(p => p.Cantidad * (p.Producto?.PesoUnitario ?? 1m));
            decimal totalVolumen = orden.Productos.Sum(p => p.Cantidad * ((p.Producto?.Largo ?? 1m) * (p.Producto?.Ancho ?? 1m) * (p.Producto?.Alto ?? 1m)));
            decimal totalCantidad = orden.Productos.Sum(p => p.Cantidad);

            var dto = new CalculoLandedCostDto
            {
                OrdenImportacionId = orden.Id,
                NumeroOrden = orden.NumeroOrden,
                FechaCalculo = DateTime.Now,
                FobTotalLocal = totalFobLocal,
                FleteTotalLocal = fleteLocal,
                SeguroTotalLocal = seguroLocal,
                GastosLocalesTotal = totalGastosLocalesMonedaLocal,
                CifTotalLocal = totalFobLocal + fleteLocal + seguroLocal,
                PorcentajeItbisUsado = configFiscal.PorcentajeITBIS,
                PorcentajeTasaServicioUsado = configFiscal.PorcentajeTasaServicioAduanal
            };

            foreach (var item in orden.Productos)
            {
                var producto = item.Producto;
                decimal fobOriginalUnitario = item.FOBTotal;
                decimal fobLocalTotalProducto = (item.Cantidad * fobOriginalUnitario) * tasaCambioOrden;

                decimal factorFob = totalFobLocal > 0 ? (fobLocalTotalProducto / totalFobLocal) : 0;
                decimal factorPeso = totalPeso > 0 ? ((item.Cantidad * (producto.PesoUnitario)) / totalPeso) : 0;
                decimal factorVolumen = (decimal)(totalVolumen > 0 ? (item.Cantidad * (producto.Largo * producto.Ancho * producto.Alto) / totalVolumen) : 0);
                decimal factorCantidad = totalCantidad > 0 ? (item.Cantidad / totalCantidad) : 0;

                decimal fleteAsignado = fleteLocal * ObtenerFactor(flete.MetodoDistribucion, factorFob, factorPeso, factorVolumen, factorCantidad);
                decimal seguroAsignado = seguroLocal * ObtenerFactor(seguro.MetodoDistribucion, factorFob, factorPeso, factorVolumen, factorCantidad);
                
                decimal gastosLocalesAsignados = 0;
                foreach(var localConv in gastosLocalesConvertidos)
                {
                    gastosLocalesAsignados += localConv.MontoLocal * ObtenerFactor(localConv.Gasto.MetodoDistribucion, factorFob, factorPeso, factorVolumen, factorCantidad);
                }

                decimal valorCif = fobLocalTotalProducto + fleteAsignado + seguroAsignado;

                var categoria = producto.CategoriaArancelaria;
                decimal montoArancel = valorCif * ((categoria?.PorcentajeArancel ?? 0m) / 100m);
                decimal montoIsc = (categoria != null && categoria.AplicaImpuestoSelectivo) ? (valorCif * (categoria.PorcentajeImpuestoSelectivo / 100m)) : 0m;
                decimal montoTasaServicio = valorCif * (configFiscal.PorcentajeTasaServicioAduanal / 100m);

                decimal baseItbis = valorCif + montoArancel + montoIsc + montoTasaServicio;
                decimal montoItbis = (categoria != null && categoria.AplicaItbis) ? (baseItbis * (configFiscal.PorcentajeITBIS / 100m)) : 0m;

                decimal costoTotalImportado = valorCif + montoArancel + montoIsc + montoTasaServicio + montoItbis + gastosLocalesAsignados;
                decimal costoUnitarioImportado = costoTotalImportado / item.Cantidad;
                
                if (item.MargenGananciaDeseado >= 100m)
                    throw new Exception($"El margen de ganancia para el producto {producto.Nombre} no puede ser igual o mayor al 100%.");

                decimal margenAplicado = item.MargenGananciaDeseado;
                decimal precioSugerido = margenAplicado == 0m 
                    ? costoUnitarioImportado 
                    : (costoUnitarioImportado / (1m - (margenAplicado / 100m)));

                dto.TotalArancel += montoArancel;
                dto.TotalIsc += montoIsc;
                dto.TotalTasaServicio += montoTasaServicio;
                dto.TotalItbis += montoItbis;
                dto.CostoTotalImportacion += costoTotalImportado;

                dto.Detalles.Add(new CalculoLandedCostDetalleDto
                {
                    ProductoId = producto.Id,
                    NombreProducto = producto.Nombre,
                    Cantidad = item.Cantidad,
                    FobOriginalUnitario = fobOriginalUnitario,
                    FobLocalTotal = fobLocalTotalProducto,
                    FleteAsignado = fleteAsignado,
                    SeguroAsignado = seguroAsignado,
                    GastosLocalesAsignados = gastosLocalesAsignados,
                    ValorCif = valorCif,
                    MontoArancel = montoArancel,
                    MontoIsc = montoIsc,
                    MontoTasaServicio = montoTasaServicio,
                    MontoItbis = montoItbis,
                    CostoTotalImportado = costoTotalImportado,
                    CostoUnitarioImportado = costoUnitarioImportado,
                    MargenDeseadoAplicado = margenAplicado,
                    PrecioVentaSugerido = precioSugerido
                });
            }

            return dto;
        }

        public async Task GuardarCalculoOficialAsync(CalculoLandedCostDto calculoDto)
        {
            var orden = await _context.OrdenesImportacion.FindAsync(calculoDto.OrdenImportacionId);
            if (orden == null) throw new Exception("Orden no encontrada.");
            
            var calculoHistorico = new CalculoLandedCost
            {
                OrdenImportacionId = calculoDto.OrdenImportacionId,
                FechaCalculo = DateTime.Now,
                FobTotalLocal = calculoDto.FobTotalLocal,
                FleteTotalLocal = calculoDto.FleteTotalLocal,
                SeguroTotalLocal = calculoDto.SeguroTotalLocal,
                GastosLocalesTotal = calculoDto.GastosLocalesTotal,
                CifTotalLocal = calculoDto.CifTotalLocal,
                TotalArancel = calculoDto.TotalArancel,
                TotalIsc = calculoDto.TotalIsc,
                TotalTasaServicio = calculoDto.TotalTasaServicio,
                TotalItbis = calculoDto.TotalItbis,
                CostoTotalImportacion = calculoDto.CostoTotalImportacion,
                PorcentajeTasaServicioUsado = calculoDto.PorcentajeTasaServicioUsado,
                PorcentajeItbisUsado = calculoDto.PorcentajeItbisUsado,
                Detalles = calculoDto.Detalles.Select(d => new CalculoLandedCostDetalle
                {
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    FobOriginalUnitario = d.FobOriginalUnitario,
                    FobLocalTotal = d.FobLocalTotal,
                    FleteAsignado = d.FleteAsignado,
                    SeguroAsignado = d.SeguroAsignado,
                    GastosLocalesAsignados = d.GastosLocalesAsignados,
                    ValorCif = d.ValorCif,
                    MontoArancel = d.MontoArancel,
                    MontoIsc = d.MontoIsc,
                    MontoTasaServicio = d.MontoTasaServicio,
                    MontoItbis = d.MontoItbis,
                    CostoTotalImportado = d.CostoTotalImportado,
                    CostoUnitarioImportado = d.CostoUnitarioImportado,
                    MargenDeseadoAplicado = d.MargenDeseadoAplicado,
                    PrecioVentaSugerido = d.PrecioVentaSugerido
                }).ToList()
            };

            _context.CalculosLandedCost.Add(calculoHistorico);
            
            orden.Estado = "Calculada";
            _context.OrdenesImportacion.Update(orden);

            await _context.SaveChangesAsync();
        }

        private decimal ObtenerFactor(string metodo, decimal factorFob, decimal factorPeso, decimal factorVolumen, decimal factorCantidad)
        {
            if (string.IsNullOrWhiteSpace(metodo)) return factorFob;
            
            return metodo.ToLower() switch
            {
                "peso" => factorPeso,
                "volumen" => factorVolumen,
                "cantidad" => factorCantidad,
                "fob" => factorFob,
                _ => factorFob
            };
        }
    }
}