using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreatLens.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddThreatIntelligenceFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HarmlessDetections",
                table: "ScanRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaliciousDetections",
                table: "ScanRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Reputation",
                table: "ScanRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuspiciousDetections",
                table: "ScanRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UndetectedDetections",
                table: "ScanRequests",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HarmlessDetections",
                table: "ScanRequests");

            migrationBuilder.DropColumn(
                name: "MaliciousDetections",
                table: "ScanRequests");

            migrationBuilder.DropColumn(
                name: "Reputation",
                table: "ScanRequests");

            migrationBuilder.DropColumn(
                name: "SuspiciousDetections",
                table: "ScanRequests");

            migrationBuilder.DropColumn(
                name: "UndetectedDetections",
                table: "ScanRequests");
        }
    }
}
