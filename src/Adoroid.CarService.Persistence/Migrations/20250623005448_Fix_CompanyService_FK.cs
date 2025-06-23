using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adoroid.CarService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fix_CompanyService_FK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServices_MasterServices_CompanyId",
                table: "CompanyServices");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServices_MasterServiceId",
                table: "CompanyServices",
                column: "MasterServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServices_MasterServices_MasterServiceId",
                table: "CompanyServices",
                column: "MasterServiceId",
                principalTable: "MasterServices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyServices_MasterServices_MasterServiceId",
                table: "CompanyServices");

            migrationBuilder.DropIndex(
                name: "IX_CompanyServices_MasterServiceId",
                table: "CompanyServices");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyServices_MasterServices_CompanyId",
                table: "CompanyServices",
                column: "CompanyId",
                principalTable: "MasterServices",
                principalColumn: "Id");
        }
    }
}
