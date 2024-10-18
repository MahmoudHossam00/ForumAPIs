using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogProject.Migrations
{
    /// <inheritdoc />
    public partial class last : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RestrictedUntill",
                table: "AspNetUsers",
                type: "DATETIME",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1d83f2c6-58d9-49d7-a6d0-a0e91de07364", "AQAAAAIAAYagAAAAEGjFzBlGF/tHGy9p+i6SYQF0TWQBMxc09dC4gT9EzLV+AFXXx6gosQGzp1nvP0vmXA==" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2024, 10, 10, 3, 29, 39, 238, DateTimeKind.Local).AddTicks(7144));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2024, 10, 10, 3, 29, 39, 238, DateTimeKind.Local).AddTicks(7160));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2024, 10, 10, 3, 29, 39, 239, DateTimeKind.Local).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2024, 10, 11, 3, 29, 39, 239, DateTimeKind.Local).AddTicks(6655));

            migrationBuilder.UpdateData(
                table: "Replies",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2024, 10, 10, 3, 29, 39, 240, DateTimeKind.Local).AddTicks(3784));

            migrationBuilder.UpdateData(
                table: "Replies",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2024, 10, 10, 3, 29, 39, 240, DateTimeKind.Local).AddTicks(3799));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RestrictedUntill",
                table: "AspNetUsers",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ac8a09ad-9e3d-4bfa-b29f-5c667c31c205", "AQAAAAIAAYagAAAAEKi42rNwS1CKvRb27cp7Y9DRVpCmoBSywYnrpgG23wX+RGkOly4P5AF8ZAbEWP2fcA==" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2024, 10, 10, 3, 28, 48, 101, DateTimeKind.Local).AddTicks(9406));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2024, 10, 10, 3, 28, 48, 101, DateTimeKind.Local).AddTicks(9428));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2024, 10, 10, 3, 28, 48, 102, DateTimeKind.Local).AddTicks(9802));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2024, 10, 11, 3, 28, 48, 102, DateTimeKind.Local).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "Replies",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2024, 10, 10, 3, 28, 48, 103, DateTimeKind.Local).AddTicks(6680));

            migrationBuilder.UpdateData(
                table: "Replies",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2024, 10, 10, 3, 28, 48, 103, DateTimeKind.Local).AddTicks(6695));
        }
    }
}
