using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adoroid.CarService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class required_description_bookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Companies_CompanyId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_MobileUsers_MobileUserId",
                table: "Bookings");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Companies_CompanyId",
                table: "Bookings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_MobileUsers_MobileUserId",
                table: "Bookings",
                column: "MobileUserId",
                principalTable: "MobileUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Companies_CompanyId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_MobileUsers_MobileUserId",
                table: "Bookings");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Companies_CompanyId",
                table: "Bookings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_MobileUsers_MobileUserId",
                table: "Bookings",
                column: "MobileUserId",
                principalTable: "MobileUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
