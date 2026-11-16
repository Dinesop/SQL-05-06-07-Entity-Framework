using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FailSafeRentals.ConsoleApp.Migrations;

/// <inheritdoc />
public partial class _20261116105211_PopulateRentalStations : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "RentalStations",
            columns: new[] { "Id", "City", "StationCode", "StreetAddress" },
            values: new object[,]
            {
                { 1, "Göteborg", "GOT-01", "Landvetter Flygplats" },
                { 2, "Stockholm", "STO-01", "Arlanda Flygplats" },
                { 3, "Malmö", "MMX-01", "Sturups Flygplats" }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "RentalStations",
            keyColumn: "Id",
            keyValue: 1);

        migrationBuilder.DeleteData(
            table: "RentalStations",
            keyColumn: "Id",
            keyValue: 2);

        migrationBuilder.DeleteData(
            table: "RentalStations",
            keyColumn: "Id",
            keyValue: 3);
    }
}
