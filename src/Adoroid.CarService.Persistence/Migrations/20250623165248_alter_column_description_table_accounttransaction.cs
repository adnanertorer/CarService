using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adoroid.CarService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class alter_column_description_table_accounttransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AccountingTransactions",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AccountingTransactions");
        }
    }
}
