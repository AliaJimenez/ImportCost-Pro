using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImportCostPro.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdenImportacionPendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Paises_PaisOrigenId",
                table: "Productos");

            migrationBuilder.AlterColumn<decimal>(
                name: "Largo",
                table: "Productos",
                type: "decimal(10,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ancho",
                table: "Productos",
                type: "decimal(10,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Alto",
                table: "Productos",
                type: "decimal(10,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,4)");

            migrationBuilder.AddColumn<int>(
                name: "PaisId",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumeroOrden",
                table: "OrdenesImportacion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "OrdenGastos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenImportacionId = table.Column<int>(type: "int", nullable: false),
                    MonedaId = table.Column<int>(type: "int", nullable: false),
                    TipoGasto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    MetodoDistribucion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaGasto = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MontoEnMonedaLocal = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenGastos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdenGastos_Monedas_MonedaId",
                        column: x => x.MonedaId,
                        principalTable: "Monedas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdenGastos_OrdenesImportacion_OrdenImportacionId",
                        column: x => x.OrdenImportacionId,
                        principalTable: "OrdenesImportacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdenProductos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenImportacionId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    PrecioUnitarioFOB = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    MargenGananciaDeseado = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    FOBTotal = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PesoTotal = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    VolumenTotal = table.Column<decimal>(type: "decimal(18,4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenProductos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdenProductos_OrdenesImportacion_OrdenImportacionId",
                        column: x => x.OrdenImportacionId,
                        principalTable: "OrdenesImportacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenProductos_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Productos_PaisId",
                table: "Productos",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenGastos_MonedaId",
                table: "OrdenGastos",
                column: "MonedaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenGastos_OrdenImportacionId",
                table: "OrdenGastos",
                column: "OrdenImportacionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenProductos_OrdenImportacionId",
                table: "OrdenProductos",
                column: "OrdenImportacionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenProductos_ProductoId",
                table: "OrdenProductos",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Paises_PaisId",
                table: "Productos",
                column: "PaisId",
                principalTable: "Paises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Paises_PaisOrigenId",
                table: "Productos",
                column: "PaisOrigenId",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Paises_PaisId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Paises_PaisOrigenId",
                table: "Productos");

            migrationBuilder.DropTable(
                name: "OrdenGastos");

            migrationBuilder.DropTable(
                name: "OrdenProductos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_PaisId",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "PaisId",
                table: "Productos");

            migrationBuilder.AlterColumn<decimal>(
                name: "Largo",
                table: "Productos",
                type: "decimal(10,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Ancho",
                table: "Productos",
                type: "decimal(10,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Alto",
                table: "Productos",
                type: "decimal(10,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumeroOrden",
                table: "OrdenesImportacion",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Paises_PaisOrigenId",
                table: "Productos",
                column: "PaisOrigenId",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
