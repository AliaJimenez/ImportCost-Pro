using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImportCostPro.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTablesOrdenGastoProductoEImportacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdenesImportacion_Importadores_ImportadorId",
                table: "OrdenesImportacion");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenesImportacion_Monedas_MonedaId",
                table: "OrdenesImportacion");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenesImportacion_Paises_PaisOrigenId",
                table: "OrdenesImportacion");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenesImportacion_Proveedores_ProveedorId",
                table: "OrdenesImportacion");

            migrationBuilder.AlterColumn<string>(
                name: "NumeroOrden",
                table: "OrdenesImportacion",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaModificacion",
                table: "OrdenesImportacion",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacion",
                table: "OrdenesImportacion",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "OrdenesImportacion",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Abierta",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Activo",
                table: "OrdenesImportacion",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaOrden",
                table: "OrdenesImportacion",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModalidadTransporte",
                table: "OrdenesImportacion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Contacto",
                table: "Importadores",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaisId",
                table: "Importadores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesImportacion_NumeroOrden",
                table: "OrdenesImportacion",
                column: "NumeroOrden",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Importadores_PaisId",
                table: "Importadores",
                column: "PaisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Importadores_Paises_PaisId",
                table: "Importadores",
                column: "PaisId",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenesImportacion_Importadores_ImportadorId",
                table: "OrdenesImportacion",
                column: "ImportadorId",
                principalTable: "Importadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenesImportacion_Monedas_MonedaId",
                table: "OrdenesImportacion",
                column: "MonedaId",
                principalTable: "Monedas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenesImportacion_Paises_PaisOrigenId",
                table: "OrdenesImportacion",
                column: "PaisOrigenId",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenesImportacion_Proveedores_ProveedorId",
                table: "OrdenesImportacion",
                column: "ProveedorId",
                principalTable: "Proveedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Importadores_Paises_PaisId",
                table: "Importadores");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenesImportacion_Importadores_ImportadorId",
                table: "OrdenesImportacion");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenesImportacion_Monedas_MonedaId",
                table: "OrdenesImportacion");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenesImportacion_Paises_PaisOrigenId",
                table: "OrdenesImportacion");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenesImportacion_Proveedores_ProveedorId",
                table: "OrdenesImportacion");

            migrationBuilder.DropIndex(
                name: "IX_OrdenesImportacion_NumeroOrden",
                table: "OrdenesImportacion");

            migrationBuilder.DropIndex(
                name: "IX_Importadores_PaisId",
                table: "Importadores");

            migrationBuilder.DropColumn(
                name: "FechaOrden",
                table: "OrdenesImportacion");

            migrationBuilder.DropColumn(
                name: "ModalidadTransporte",
                table: "OrdenesImportacion");

            migrationBuilder.DropColumn(
                name: "PaisId",
                table: "Importadores");

            migrationBuilder.AlterColumn<string>(
                name: "NumeroOrden",
                table: "OrdenesImportacion",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaModificacion",
                table: "OrdenesImportacion",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacion",
                table: "OrdenesImportacion",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "OrdenesImportacion",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Abierta");

            migrationBuilder.AlterColumn<bool>(
                name: "Activo",
                table: "OrdenesImportacion",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Contacto",
                table: "Importadores",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenesImportacion_Importadores_ImportadorId",
                table: "OrdenesImportacion",
                column: "ImportadorId",
                principalTable: "Importadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenesImportacion_Monedas_MonedaId",
                table: "OrdenesImportacion",
                column: "MonedaId",
                principalTable: "Monedas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenesImportacion_Paises_PaisOrigenId",
                table: "OrdenesImportacion",
                column: "PaisOrigenId",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenesImportacion_Proveedores_ProveedorId",
                table: "OrdenesImportacion",
                column: "ProveedorId",
                principalTable: "Proveedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
