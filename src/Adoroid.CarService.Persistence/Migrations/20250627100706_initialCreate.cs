using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Adoroid.CarService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    AuthorizedName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AuthorizedSurname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TaxNumber = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    TaxOffice = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    DistrictId = table.Column<int>(type: "integer", nullable: false),
                    CompanyAddress = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    CompanyPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CompanyEmail = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
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
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_MasterServices", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "AccountingTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountOwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountOwnerType = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionType = table.Column<int>(type: "integer", nullable: false),
                    Claim = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Debt = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK_AccountingTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountingTransactions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    TaxNumber = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    TaxOffice = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
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
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    Address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ContactName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    Address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suppliers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompanyServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    MasterServiceId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_CompanyServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyServices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompanyServices_MasterServices_MasterServiceId",
                        column: x => x.MasterServiceId,
                        principalTable: "MasterServices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Brand = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Plate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Engine = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FuelTypeId = table.Column<int>(type: "integer", nullable: false),
                    SerialNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    MobileUserId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vehicles_MobileUsers_MobileUserId",
                        column: x => x.MobileUserId,
                        principalTable: "MobileUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MainServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ServiceStatus = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_MainServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainServices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MainServices_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MainServiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Operation = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Material = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    MaterialBrand = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: true),
                    Discount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
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
                    table.PrimaryKey("PK_SubServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubServices_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubServices_MainServices_MainServiceId",
                        column: x => x.MainServiceId,
                        principalTable: "MainServices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubServices_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "MasterServices",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "IsDeleted", "OrderIndex", "ServiceName", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("1a572b68-67a9-4eb6-8136-176059fd1b70"), new Guid("8dbdb3b5-4cc1-4aa0-9ed0-b6ea96a6797f"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000000"), null, false, 12, "Oto Temizlik", new Guid("00000000-0000-0000-0000-000000000000"), null },
                    { new Guid("1c3d15c3-9446-406e-b1de-884cc78142ec"), new Guid("dd52404b-7d2e-4c9b-b462-16f75d76dc4e"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000000"), null, false, 4, "Lastik Tamir/Değişim", new Guid("00000000-0000-0000-0000-000000000000"), null },
                    { new Guid("269358de-194a-4ce9-a93f-f68fd58b371d"), new Guid("ba78e33b-75e5-4ac7-8715-04fbc0c6f07b"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000000"), null, false, 5, "Oto Cam", new Guid("00000000-0000-0000-0000-000000000000"), null },
                    { new Guid("2d310fa3-8e36-44d3-8df1-5a07adcb4d1e"), new Guid("b91d228f-0325-493e-bef3-01955cf00b95"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000000"), null, false, 8, "Lpg Montaj/Dönüşüm", new Guid("00000000-0000-0000-0000-000000000000"), null },
                    { new Guid("88e1c1ae-ef3b-472a-bb61-c0e80f9c0a90"), new Guid("f06e2b66-7cc3-4418-9c84-bb4e5072c87f"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000000"), null, false, 9, "Ekspertiz", new Guid("00000000-0000-0000-0000-000000000000"), null },
                    { new Guid("8ce99e0e-cb66-4ef1-b0cd-52ea11ea4620"), new Guid("b97c1cf4-6d68-4039-851a-0c00401713b2"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000000"), null, false, 6, "Egzoz", new Guid("00000000-0000-0000-0000-000000000000"), null },
                    { new Guid("8f370c01-8ad7-4f6e-a427-62a0e6b8aa29"), new Guid("7a3a0f64-b3d1-4c8e-bb44-7bcb3e2520a1"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000000"), null, false, 0, "Motor Tamir/Bakım", new Guid("00000000-0000-0000-0000-000000000000"), null },
                    { new Guid("a5c17247-10f4-4f59-a6e5-8df2030f5da3"), new Guid("61856049-c7d1-4eb8-8c9b-67894ad7dfe2"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000000"), null, false, 7, "Elektronik", new Guid("00000000-0000-0000-0000-000000000000"), null },
                    { new Guid("a7d2db3a-74e0-498b-8bce-7ed265e2e2fb"), new Guid("9cb23cb5-1761-4374-9479-55db33f14762"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000000"), null, false, 10, "Oto Döşeme", new Guid("00000000-0000-0000-0000-000000000000"), null },
                    { new Guid("ca46e3f6-5e4e-4e61-9215-59f5dd8dd2f8"), new Guid("3df9ed8b-8dc0-4fa3-994e-bd19668f1971"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000000"), null, false, 3, "Isıtma/Klima", new Guid("00000000-0000-0000-0000-000000000000"), null },
                    { new Guid("ce7e613a-6935-4370-b299-26fcd73f49db"), new Guid("94df0ae0-45d4-4dcf-94a7-72b5116c4517"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000000"), null, false, 1, "Oto Elektrik", new Guid("00000000-0000-0000-0000-000000000000"), null },
                    { new Guid("e66fa2f6-1e7d-4c5d-98d7-eec49e5c8cbe"), new Guid("bff8ac59-f735-4cd4-bef6-6ec785c5a65a"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000000"), null, false, 11, "Oto Modifiye", new Guid("00000000-0000-0000-0000-000000000000"), null },
                    { new Guid("fa7cba68-8412-4cf5-a42b-0fa970902374"), new Guid("a1c51c0d-4efb-4e11-94b8-041b7f57298e"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000000"), null, false, 2, "Kaporta/Boya", new Guid("00000000-0000-0000-0000-000000000000"), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountingTransactions_CompanyId",
                table: "AccountingTransactions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServices_CompanyId",
                table: "CompanyServices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyServices_MasterServiceId",
                table: "CompanyServices",
                column: "MasterServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyId",
                table: "Customers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MainServices_CompanyId",
                table: "MainServices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MainServices_VehicleId",
                table: "MainServices",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_SubServices_EmployeeId",
                table: "SubServices",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubServices_MainServiceId",
                table: "SubServices",
                column: "MainServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SubServices_SupplierId",
                table: "SubServices",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CompanyId",
                table: "Suppliers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                table: "Users",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CustomerId",
                table: "Vehicles",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_MobileUserId",
                table: "Vehicles",
                column: "MobileUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingTransactions");

            migrationBuilder.DropTable(
                name: "CompanyServices");

            migrationBuilder.DropTable(
                name: "SubServices");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "MasterServices");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "MainServices");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "MobileUsers");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
