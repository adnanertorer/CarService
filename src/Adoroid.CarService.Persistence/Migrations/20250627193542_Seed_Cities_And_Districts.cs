using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adoroid.CarService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Seed_Cities_And_Districts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = File.ReadAllText("Migrations/Scripts/SeedCitiesAndDistricts.sql");
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
