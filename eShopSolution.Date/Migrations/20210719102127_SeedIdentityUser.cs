using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Date.Migrations
{
    public partial class SeedIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("92d17892-8687-4039-a779-5e0982bfdff0"), "68175585-7595-476a-a6af-fc273e30fde0", "Administrator role", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("a1207630-6084-4e74-8bed-4e52d4025e2b"), new Guid("92d17892-8687-4039-a779-5e0982bfdff0") });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("a1207630-6084-4e74-8bed-4e52d4025e2b"), 0, "806788ea-6735-40eb-8d45-4d77d04dd2cf", new DateTime(2020, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "quannm2812@gmail.com", true, "Quan", "Nguyen", false, null, "quannm2812@gmail.com", "admin", "AQAAAAEAACcQAAAAEHpD8opswKieh0FZLJpc7BRZKHV4rC0Lh+q0Ize7BJMowW3b+cV4jQy7Tc9G4LUPUw==", null, false, "", false, "admin" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("92d17892-8687-4039-a779-5e0982bfdff0"));

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("a1207630-6084-4e74-8bed-4e52d4025e2b"), new Guid("92d17892-8687-4039-a779-5e0982bfdff0") });

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("a1207630-6084-4e74-8bed-4e52d4025e2b"));

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
                value: new DateTime(2021, 7, 19, 16, 56, 26, 902, DateTimeKind.Local).AddTicks(5672));
        }
    }
}
