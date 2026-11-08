using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreatLens.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToScanRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ScanRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ScanRequests_UserId",
                table: "ScanRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScanRequests_AspNetUsers_UserId",
                table: "ScanRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScanRequests_AspNetUsers_UserId",
                table: "ScanRequests");

            migrationBuilder.DropIndex(
                name: "IX_ScanRequests_UserId",
                table: "ScanRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ScanRequests");
        }
    }
}
