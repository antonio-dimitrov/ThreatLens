using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreatLens.Api.Migrations
{
    /// <inheritdoc />
    public partial class ScanStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScanRequests");

            migrationBuilder.CreateTable(
                name: "Scans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reputation = table.Column<int>(type: "int", nullable: true),
                    MaliciousDetections = table.Column<int>(type: "int", nullable: true),
                    SuspiciousDetections = table.Column<int>(type: "int", nullable: true),
                    HarmlessDetections = table.Column<int>(type: "int", nullable: true),
                    UndetectedDetections = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scans_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DomainScans",
                columns: table => new
                {
                    ScanId = table.Column<int>(type: "int", nullable: false),
                    Registrar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tld = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainScans", x => x.ScanId);
                    table.ForeignKey(
                        name: "FK_DomainScans_Scans_ScanId",
                        column: x => x.ScanId,
                        principalTable: "Scans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IpScans",
                columns: table => new
                {
                    ScanId = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AsOwner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Asn = table.Column<int>(type: "int", nullable: true),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpScans", x => x.ScanId);
                    table.ForeignKey(
                        name: "FK_IpScans_Scans_ScanId",
                        column: x => x.ScanId,
                        principalTable: "Scans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scans_UserId_CreatedAtUtc",
                table: "Scans",
                columns: new[] { "UserId", "CreatedAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DomainScans");

            migrationBuilder.DropTable(
                name: "IpScans");

            migrationBuilder.DropTable(
                name: "Scans");

            migrationBuilder.CreateTable(
                name: "ScanRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HarmlessDetections = table.Column<int>(type: "int", nullable: true),
                    MaliciousDetections = table.Column<int>(type: "int", nullable: true),
                    Reputation = table.Column<int>(type: "int", nullable: true),
                    SuspiciousDetections = table.Column<int>(type: "int", nullable: true),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UndetectedDetections = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScanRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScanRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScanRequests_UserId",
                table: "ScanRequests",
                column: "UserId");
        }
    }
}
