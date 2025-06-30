using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adoroid.CarService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserToCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserToCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserType = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", maxLength: 64, nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", maxLength: 64, nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uuid", maxLength: 64, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserToCompanies_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserToCompanies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserToCompanies_CompanyId",
                table: "UserToCompanies",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToCompanies_UserId",
                table: "UserToCompanies",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserToCompanies");
        }
    }
}
