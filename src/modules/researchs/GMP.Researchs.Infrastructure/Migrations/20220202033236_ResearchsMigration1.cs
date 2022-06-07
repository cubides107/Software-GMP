using Microsoft.EntityFrameworkCore.Migrations;

namespace GMP.Researchs.Infrastructure.Migrations
{
    public partial class ResearchsMigration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoVisitaEnum",
                schema: "researchs",
                table: "Solicitations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                schema: "researchs",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCritical",
                schema: "researchs",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TiposSolicitudes",
                schema: "researchs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    SolicitationId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposSolicitudes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TiposSolicitudes_Solicitations_SolicitationId",
                        column: x => x.SolicitationId,
                        principalSchema: "researchs",
                        principalTable: "Solicitations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TiposSolicitudes_SolicitationId",
                schema: "researchs",
                table: "TiposSolicitudes",
                column: "SolicitationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TiposSolicitudes",
                schema: "researchs");

            migrationBuilder.DropColumn(
                name: "TipoVisitaEnum",
                schema: "researchs",
                table: "Solicitations");

            migrationBuilder.DropColumn(
                name: "Company",
                schema: "researchs",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsCritical",
                schema: "researchs",
                table: "Employees");
        }
    }
}
