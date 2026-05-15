using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EVENT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IDENTITY_ROLE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDENTITY_ROLE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SECTOR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SECTOR", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SECTOR_EVENT_EventId",
                        column: x => x.EventId,
                        principalTable: "EVENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IDENTITY_ROLE_CLAIM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDENTITY_ROLE_CLAIM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IDENTITY_ROLE_CLAIM_IDENTITY_ROLE_RoleId",
                        column: x => x.RoleId,
                        principalTable: "IDENTITY_ROLE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AUDIT_LOG",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_LOG", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AUDIT_LOG_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IDENTITY_USER_CLAIM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDENTITY_USER_CLAIM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IDENTITY_USER_CLAIM_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IDENTITY_USER_LOGIN",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDENTITY_USER_LOGIN", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_IDENTITY_USER_LOGIN_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IDENTITY_USER_ROLE",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDENTITY_USER_ROLE", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_IDENTITY_USER_ROLE_IDENTITY_ROLE_RoleId",
                        column: x => x.RoleId,
                        principalTable: "IDENTITY_ROLE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IDENTITY_USER_ROLE_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IDENTITY_USER_TOKEN",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDENTITY_USER_TOKEN", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_IDENTITY_USER_TOKEN_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SEAT",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectorId = table.Column<int>(type: "int", nullable: false),
                    RowIdentifier = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SeatNumber = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEAT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SEAT_SECTOR_SectorId",
                        column: x => x.SectorId,
                        principalTable: "SECTOR",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RESERVATION",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReservedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESERVATION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RESERVATION_SEAT_SeatId",
                        column: x => x.SeatId,
                        principalTable: "SEAT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RESERVATION_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "EVENT",
                columns: new[] { "Id", "Description", "EventDate", "ImageUrl", "Name", "Status", "Venue" },
                values: new object[] { 1, null, new DateTime(2026, 7, 15, 21, 0, 0, 0, DateTimeKind.Unspecified), null, "Noches en Vivo 2026", "Published", "Microestadio UNAJ" });

            migrationBuilder.InsertData(
                table: "SECTOR",
                columns: new[] { "Id", "Capacity", "EventId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 50, 1, "Campo", 12000m },
                    { 2, 50, 1, "Platea", 18000m }
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

            migrationBuilder.CreateIndex(
                name: "IX_AUDIT_LOG_UserId",
                table: "AUDIT_LOG",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "IDENTITY_ROLE",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_IDENTITY_ROLE_CLAIM_RoleId",
                table: "IDENTITY_ROLE_CLAIM",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_IDENTITY_USER_CLAIM_UserId",
                table: "IDENTITY_USER_CLAIM",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IDENTITY_USER_LOGIN_UserId",
                table: "IDENTITY_USER_LOGIN",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IDENTITY_USER_ROLE_RoleId",
                table: "IDENTITY_USER_ROLE",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RESERVATION_SeatId",
                table: "RESERVATION",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_RESERVATION_UserId",
                table: "RESERVATION",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SEAT_SectorId_RowIdentifier_SeatNumber",
                table: "SEAT",
                columns: new[] { "SectorId", "RowIdentifier", "SeatNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SECTOR_EventId",
                table: "SECTOR",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "USER",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_USER_Email",
                table: "USER",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "USER",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUDIT_LOG");

            migrationBuilder.DropTable(
                name: "IDENTITY_ROLE_CLAIM");

            migrationBuilder.DropTable(
                name: "IDENTITY_USER_CLAIM");

            migrationBuilder.DropTable(
                name: "IDENTITY_USER_LOGIN");

            migrationBuilder.DropTable(
                name: "IDENTITY_USER_ROLE");

            migrationBuilder.DropTable(
                name: "IDENTITY_USER_TOKEN");

            migrationBuilder.DropTable(
                name: "RESERVATION");

            migrationBuilder.DropTable(
                name: "IDENTITY_ROLE");

            migrationBuilder.DropTable(
                name: "SEAT");

            migrationBuilder.DropTable(
                name: "USER");

            migrationBuilder.DropTable(
                name: "SECTOR");

            migrationBuilder.DropTable(
                name: "EVENT");
        }
    }
}
