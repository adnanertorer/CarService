using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adoroid.CarService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mobile_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "Vehicles",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "MobileUserId",
                table: "Vehicles",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MobileUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RefreshToken = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    OtpCode = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    RefreshTokenExpr = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_MobileUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_MobileUserId",
                table: "Vehicles",
                column: "MobileUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_MobileUsers_MobileUserId",
                table: "Vehicles",
                column: "MobileUserId",
                principalTable: "MobileUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_MobileUsers_MobileUserId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "MobileUsers");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_MobileUserId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "MobileUserId",
                table: "Vehicles");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "Vehicles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
