using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adoroid.CarService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class alter_column_AdjustedTransactionId_to_AccountTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AdjustedTransactionId",
                table: "AccountingTransactions",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdjustedTransactionId",
                table: "AccountingTransactions");
        }
    }
}
