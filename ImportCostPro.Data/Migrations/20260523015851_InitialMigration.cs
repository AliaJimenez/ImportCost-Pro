using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImportCostPro.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriasArancelarias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoArancelario = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PorcentajeArancel = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AplicaItbis = table.Column<bool>(type: "bit", nullable: false),
                    AplicaImpuestoSelectivo = table.Column<bool>(type: "bit", nullable: false),
                    PorcentajeImpuestoSelectivo = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasArancelarias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CodigoReferencia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PesoUnitario = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    Largo = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    Ancho = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    Alto = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    UnidadMedida = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    PaisOrigenId = table.Column<int>(type: "int", nullable: false),
                    CategoriaArancelariaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_CategoriasArancelarias_CategoriaArancelariaId",
                        column: x => x.CategoriaArancelariaId,
                        principalTable: "CategoriasArancelarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CategoriaArancelariaId",
                table: "Productos",
                column: "CategoriaArancelariaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "CategoriasArancelarias");
        }
    }
}
