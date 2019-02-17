using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceBook.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Make = table.Column<string>(nullable: false),
                    Model = table.Column<string>(nullable: false),
                    ExactModelName = table.Column<string>(nullable: true),
                    VIN = table.Column<string>(nullable: true),
                    RegistrationPlate = table.Column<string>(nullable: false),
                    OwnerId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceInterventions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Mileage = table.Column<int>(nullable: false),
                    NextServiceIntervalDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    VehicleId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceInterventions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceInterventions_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceItems",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Part = table.Column<string>(nullable: true),
                    PricePerUnit = table.Column<decimal>(nullable: false),
                    Units = table.Column<double>(nullable: false),
                    ServiceInterventionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceItems_ServiceInterventions_ServiceInterventionId",
                        column: x => x.ServiceInterventionId,
                        principalTable: "ServiceInterventions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInterventions_VehicleId",
                table: "ServiceInterventions",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceItems_ServiceInterventionId",
                table: "ServiceItems",
                column: "ServiceInterventionId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_OwnerId",
                table: "Vehicles",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceItems");

            migrationBuilder.DropTable(
                name: "ServiceInterventions");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
