using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class addSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EVENT",
                columns: new[] { "Id", "EventDate", "Name", "Status", "Venue" },
                values: new object[] { 1, new DateTime(2026, 7, 15, 21, 0, 0, 0, DateTimeKind.Unspecified), "Concierto de Rock", "Published", "Estadio Central" });

            migrationBuilder.InsertData(
                table: "SECTOR",
                columns: new[] { "Id", "Capacity", "EventId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 50, 1, "Sector A", 12000m },
                    { 2, 50, 1, "Sector B", 18000m }
                });

            migrationBuilder.InsertData(
                table: "SEAT",
                columns: new[] { "Id", "RowIdentifier", "SeatNumber", "SectorId", "Status", "Version" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-010000000001"), "A", 1, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000002"), "A", 2, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000003"), "A", 3, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000004"), "A", 4, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000005"), "A", 5, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000006"), "A", 6, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000007"), "A", 7, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000008"), "A", 8, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000009"), "A", 9, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000010"), "A", 10, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000011"), "B", 1, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000012"), "B", 2, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000013"), "B", 3, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000014"), "B", 4, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000015"), "B", 5, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000016"), "B", 6, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000017"), "B", 7, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000018"), "B", 8, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000019"), "B", 9, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000020"), "B", 10, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000021"), "C", 1, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000022"), "C", 2, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000023"), "C", 3, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000024"), "C", 4, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000025"), "C", 5, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000026"), "C", 6, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000027"), "C", 7, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000028"), "C", 8, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000029"), "C", 9, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000030"), "C", 10, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000031"), "D", 1, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000032"), "D", 2, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000033"), "D", 3, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000034"), "D", 4, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000035"), "D", 5, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000036"), "D", 6, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000037"), "D", 7, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000038"), "D", 8, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000039"), "D", 9, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000040"), "D", 10, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000041"), "E", 1, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000042"), "E", 2, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000043"), "E", 3, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000044"), "E", 4, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000045"), "E", 5, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000046"), "E", 6, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000047"), "E", 7, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000048"), "E", 8, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000049"), "E", 9, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-010000000050"), "E", 10, 1, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000001"), "A", 1, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000002"), "A", 2, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000003"), "A", 3, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000004"), "A", 4, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000005"), "A", 5, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000006"), "A", 6, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000007"), "A", 7, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000008"), "A", 8, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000009"), "A", 9, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000010"), "A", 10, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000011"), "B", 1, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000012"), "B", 2, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000013"), "B", 3, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000014"), "B", 4, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000015"), "B", 5, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000016"), "B", 6, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000017"), "B", 7, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000018"), "B", 8, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000019"), "B", 9, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000020"), "B", 10, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000021"), "C", 1, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000022"), "C", 2, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000023"), "C", 3, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000024"), "C", 4, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000025"), "C", 5, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000026"), "C", 6, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000027"), "C", 7, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000028"), "C", 8, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000029"), "C", 9, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000030"), "C", 10, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000031"), "D", 1, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000032"), "D", 2, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000033"), "D", 3, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000034"), "D", 4, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000035"), "D", 5, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000036"), "D", 6, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000037"), "D", 7, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000038"), "D", 8, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000039"), "D", 9, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000040"), "D", 10, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000041"), "E", 1, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000042"), "E", 2, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000043"), "E", 3, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000044"), "E", 4, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000045"), "E", 5, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000046"), "E", 6, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000047"), "E", 7, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000048"), "E", 8, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000049"), "E", 9, 2, "Available", 1 },
                    { new Guid("00000000-0000-0000-0000-020000000050"), "E", 10, 2, "Available", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000001"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000002"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000003"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000004"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000005"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000006"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000007"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000008"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000009"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000010"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000011"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000012"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000013"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000014"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000015"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000016"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000017"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000018"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000019"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000020"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000021"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000022"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000023"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000024"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000025"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000026"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000027"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000028"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000029"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000030"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000031"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000032"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000033"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000034"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000035"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000036"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000037"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000038"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000039"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000040"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000041"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000042"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000043"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000044"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000045"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000046"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000047"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000048"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000049"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-010000000050"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000001"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000002"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000003"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000004"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000005"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000006"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000007"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000008"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000009"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000010"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000011"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000012"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000013"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000014"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000015"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000016"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000017"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000018"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000019"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000020"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000021"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000022"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000023"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000024"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000025"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000026"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000027"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000028"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000029"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000030"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000031"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000032"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000033"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000034"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000035"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000036"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000037"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000038"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000039"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000040"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000041"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000042"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000043"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000044"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000045"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000046"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000047"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000048"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000049"));

            migrationBuilder.DeleteData(
                table: "SEAT",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-020000000050"));

            migrationBuilder.DeleteData(
                table: "SECTOR",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SECTOR",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EVENT",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
