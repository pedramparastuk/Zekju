using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class addIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_agency_id_origin_city_id_destination_city_id",
                table: "Subscriptions",
                columns: new[] { "agency_id", "origin_city_id", "destination_city_id" });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_origin_city_id_destination_city_id",
                table: "Routes",
                columns: new[] { "origin_city_id", "destination_city_id" });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_departure_time",
                table: "Flights",
                column: "departure_time");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_agency_id_origin_city_id_destination_city_id",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Routes_origin_city_id_destination_city_id",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Flights_departure_time",
                table: "Flights");
        }
    }
}
