using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarangaySystem.Migrations
{
    /// <inheritdoc />
    public partial class AddReadyNotificationToDocumentRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReadyNotificationSeen",
                table: "DocumentRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 8, 18, 1, 45, 953, DateTimeKind.Local).AddTicks(1423), "$2a$11$BzRMQ.3lUQBE7eAvnOUxOuhGUcIseNq0DsWrOteucHnEw/n4iP6va" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReadyNotificationSeen",
                table: "DocumentRequests");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 5, 8, 14, 49, 10, 312, DateTimeKind.Local).AddTicks(1541), "$2a$11$IT3bvcmr2W15EkkKM.YNs.OE3v1600XZB4KojQLdfWYgsoQ911qHK" });
        }
    }
}
