using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarangaySystem.Migrations
{
    /// <inheritdoc />
    public partial class MakeAdminRemarksNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AdminRemarks",
                table: "DocumentRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 8, 14, 49, 10, 312, DateTimeKind.Local).AddTicks(1541), "$2a$11$IT3bvcmr2W15EkkKM.YNs.OE3v1600XZB4KojQLdfWYgsoQ911qHK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AdminRemarks",
                table: "DocumentRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 6, 16, 56, 39, 988, DateTimeKind.Local).AddTicks(129), "$2a$11$z.po9H9NaDmMWF2VB33Ua.2K5bIOtO70LQHSh3vxuFkgc000hkVDS" });
        }
    }
}
