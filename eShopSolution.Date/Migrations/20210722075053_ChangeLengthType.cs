using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Date.Migrations
{
    public partial class ChangeLengthType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("92d17892-8687-4039-a779-5e0982bfdff0"),
                column: "ConcurrencyStamp",
                value: "2042427d-df11-4d11-b4fd-2089393b6b6d");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("a1207630-6084-4e74-8bed-4e52d4025e2b"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bdc1eb19-e544-46c9-872b-925f312243aa", "AQAAAAEAACcQAAAAEMpwT6YS3L2OoaOO+/oZddsIhp1SEe3Ua50Up+tDTrUbYmUe3nnPjLEIWqfuKeOtmA==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 7, 22, 14, 50, 51, 708, DateTimeKind.Local).AddTicks(9107));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("92d17892-8687-4039-a779-5e0982bfdff0"),
                column: "ConcurrencyStamp",
                value: "68175585-7595-476a-a6af-fc273e30fde0");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("a1207630-6084-4e74-8bed-4e52d4025e2b"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "806788ea-6735-40eb-8d45-4d77d04dd2cf", "AQAAAAEAACcQAAAAEHpD8opswKieh0FZLJpc7BRZKHV4rC0Lh+q0Ize7BJMowW3b+cV4jQy7Tc9G4LUPUw==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 7, 19, 17, 21, 26, 595, DateTimeKind.Local).AddTicks(9971));
        }
    }
}
