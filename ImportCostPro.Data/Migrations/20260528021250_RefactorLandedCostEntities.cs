using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImportCostPro.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorLandedCostEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdenesImportacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroOrden = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImportadorId = table.Column<int>(type: "int", nullable: false),
                    ProveedorId = table.Column<int>(type: "int", nullable: false),
                    PaisOrigenId = table.Column<int>(type: "int", nullable: false),
                    MonedaId = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CostoFOB = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CIF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Arancel = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ImpuestoSelectivo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TasaAduanal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ITBIS = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PrecioSugerido = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenesImportacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdenesImportacion_Importadores_ImportadorId",
                        column: x => x.ImportadorId,
                        principalTable: "Importadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenesImportacion_Monedas_MonedaId",
                        column: x => x.MonedaId,
                        principalTable: "Monedas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenesImportacion_Paises_PaisOrigenId",
                        column: x => x.PaisOrigenId,
                        principalTable: "Paises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenesImportacion_Proveedores_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalculosLandedCost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenImportacionId = table.Column<int>(type: "int", nullable: false),
                    FobTotalLocal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FleteTotalLocal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SeguroTotalLocal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GastosLocalesTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CifTotalLocal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalArancel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalIsc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTasaServicio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalItbis = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoTotalImportacion = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PorcentajeTasaServicioUsado = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PorcentajeItbisUsado = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    FechaCalculo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculosLandedCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculosLandedCost_OrdenesImportacion_OrdenImportacionId",
                        column: x => x.OrdenImportacionId,
                        principalTable: "OrdenesImportacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesLandedCost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalculoLandedCostId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FobOriginalUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FobLocalTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FleteAsignado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SeguroAsignado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GastosLocalesAsignados = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorCif = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoArancel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoIsc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoTasaServicio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoItbis = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoTotalImportado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoUnitarioImportado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MargenDeseadoAplicado = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PrecioVentaSugerido = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesLandedCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesLandedCost_CalculosLandedCost_CalculoLandedCostId",
                        column: x => x.CalculoLandedCostId,
                        principalTable: "CalculosLandedCost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesLandedCost_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalculosLandedCost_OrdenImportacionId",
                table: "CalculosLandedCost",
                column: "OrdenImportacionId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesLandedCost_CalculoLandedCostId",
                table: "DetallesLandedCost",
                column: "CalculoLandedCostId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesLandedCost_ProductoId",
                table: "DetallesLandedCost",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesImportacion_ImportadorId",
                table: "OrdenesImportacion",
                column: "ImportadorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesImportacion_MonedaId",
                table: "OrdenesImportacion",
                column: "MonedaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesImportacion_PaisOrigenId",
                table: "OrdenesImportacion",
                column: "PaisOrigenId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesImportacion_ProveedorId",
                table: "OrdenesImportacion",
                column: "ProveedorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesLandedCost");

            migrationBuilder.DropTable(
                name: "CalculosLandedCost");

            migrationBuilder.DropTable(
                name: "OrdenesImportacion");
        }
    }
}
