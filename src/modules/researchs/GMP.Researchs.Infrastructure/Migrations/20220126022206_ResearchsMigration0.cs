using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GMP.Researchs.Infrastructure.Migrations
{
    public partial class ResearchsMigration0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "researchs");

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "researchs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Neighborhood = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "researchs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specialty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Post = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LandLine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyCellPhone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MapHomes",
                schema: "researchs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContainerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapHomes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "researchs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Solicitations",
                schema: "researchs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SolicitationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Journey = table.Column<int>(type: "int", nullable: false),
                    Reviewed = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AddressId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MapHomeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserCreatesSolicitationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserManagesSolicitationId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solicitations_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "researchs",
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitations_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "researchs",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitations_MapHomes_MapHomeId",
                        column: x => x.MapHomeId,
                        principalSchema: "researchs",
                        principalTable: "MapHomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitations_Users_UserCreatesSolicitationId",
                        column: x => x.UserCreatesSolicitationId,
                        principalSchema: "researchs",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitations_Users_UserManagesSolicitationId",
                        column: x => x.UserManagesSolicitationId,
                        principalSchema: "researchs",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solicitations_AddressId",
                schema: "researchs",
                table: "Solicitations",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitations_EmployeeId",
                schema: "researchs",
                table: "Solicitations",
                column: "EmployeeId",
                unique: true,
                filter: "[EmployeeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitations_MapHomeId",
                schema: "researchs",
                table: "Solicitations",
                column: "MapHomeId",
                unique: true,
                filter: "[MapHomeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitations_UserCreatesSolicitationId",
                schema: "researchs",
                table: "Solicitations",
                column: "UserCreatesSolicitationId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitations_UserManagesSolicitationId",
                schema: "researchs",
                table: "Solicitations",
                column: "UserManagesSolicitationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Solicitations",
                schema: "researchs");

            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "researchs");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "researchs");

            migrationBuilder.DropTable(
                name: "MapHomes",
                schema: "researchs");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "researchs");
        }
    }
}
