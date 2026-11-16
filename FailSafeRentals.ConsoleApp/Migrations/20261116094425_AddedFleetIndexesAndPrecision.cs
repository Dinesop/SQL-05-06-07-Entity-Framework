using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FailSafeRentals.ConsoleApp.Migrations;

/// <inheritdoc />
public partial class _20261116094425_AddedFleetIndexesAndPrecision : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<decimal>(
            name: "EstimatedCost",
            table: "DamageReports",
            type: "decimal(10,2)",
            precision: 10,
            scale: 2,
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)");

        migrationBuilder.CreateIndex(
            name: "IX_RentalStations_StationCode",
            table: "RentalStations",
            column: "StationCode",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_FleetVehicles_RegistrationNumber",
            table: "FleetVehicles",
            column: "RegistrationNumber",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_RentalStations_StationCode",
            table: "RentalStations");

        migrationBuilder.DropIndex(
            name: "IX_FleetVehicles_RegistrationNumber",
            table: "FleetVehicles");

        migrationBuilder.AlterColumn<decimal>(
            name: "EstimatedCost",
            table: "DamageReports",
            type: "decimal(18,2)",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(10,2)",
            oldPrecision: 10,
            oldScale: 2);
    }
}
