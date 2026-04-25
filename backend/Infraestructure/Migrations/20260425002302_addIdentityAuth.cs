using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class addIdentityAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AUDIT_LOG_USER_UserId",
                table: "AUDIT_LOG");

            migrationBuilder.DropForeignKey(
                name: "FK_RESERVATION_SEAT_SeatId",
                table: "RESERVATION");

            migrationBuilder.DropForeignKey(
                name: "FK_RESERVATION_USER_UserId",
                table: "RESERVATION");

            migrationBuilder.DropForeignKey(
                name: "FK_SEAT_SECTOR_SectorId",
                table: "SEAT");

            migrationBuilder.DropForeignKey(
                name: "FK_SECTOR_EVENT_EventId",
                table: "SECTOR");

            migrationBuilder.DropPrimaryKey(
                name: "PK_USER",
                table: "USER");

            migrationBuilder.DropIndex(
                name: "IX_USER_Email",
                table: "USER");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SECTOR",
                table: "SECTOR");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SEAT",
                table: "SEAT");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RESERVATION",
                table: "RESERVATION");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EVENT",
                table: "EVENT");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AUDIT_LOG",
                table: "AUDIT_LOG");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "USER",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "USER",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "USER",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "USER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "USER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "USER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "USER",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "USER",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "USER",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "USER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "USER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "USER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "USER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "USER",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_USER",
                table: "USER",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SECTOR",
                table: "SECTOR",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SEAT",
                table: "SEAT",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RESERVATION",
                table: "RESERVATION",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EVENT",
                table: "EVENT",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AUDIT_LOG",
                table: "AUDIT_LOG",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AUDIT_LOG_USER_UserId",
                table: "AUDIT_LOG",
                column: "UserId",
                principalTable: "USER",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RESERVATION_SEAT_SeatId",
                table: "RESERVATION",
                column: "SeatId",
                principalTable: "SEAT",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RESERVATION_USER_UserId",
                table: "RESERVATION",
                column: "UserId",
                principalTable: "USER",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SEAT_SECTOR_SectorId",
                table: "SEAT",
                column: "SectorId",
                principalTable: "SECTOR",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SECTOR_EVENT_EventId",
                table: "SECTOR",
                column: "EventId",
                principalTable: "EVENT",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AUDIT_LOG_USER_UserId",
                table: "AUDIT_LOG");

            migrationBuilder.DropForeignKey(
                name: "FK_RESERVATION_SEAT_SeatId",
                table: "RESERVATION");

            migrationBuilder.DropForeignKey(
                name: "FK_RESERVATION_USER_UserId",
                table: "RESERVATION");

            migrationBuilder.DropForeignKey(
                name: "FK_SEAT_SECTOR_SectorId",
                table: "SEAT");

            migrationBuilder.DropForeignKey(
                name: "FK_SECTOR_EVENT_EventId",
                table: "SECTOR");

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
                name: "IDENTITY_ROLE");

            migrationBuilder.DropPrimaryKey(
                name: "PK_USER",
                table: "USER");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "USER");

            migrationBuilder.DropIndex(
                name: "IX_USER_Email",
                table: "USER");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "USER");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SECTOR",
                table: "SECTOR");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SEAT",
                table: "SEAT");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RESERVATION",
                table: "RESERVATION");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EVENT",
                table: "EVENT");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AUDIT_LOG",
                table: "AUDIT_LOG");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "USER");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "USER",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "USER",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_USER",
                table: "USER",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SECTOR",
                table: "SECTOR",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SEAT",
                table: "SEAT",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RESERVATION",
                table: "RESERVATION",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EVENT",
                table: "EVENT",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AUDIT_LOG",
                table: "AUDIT_LOG",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_USER_Email",
                table: "USER",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AUDIT_LOG_USER_UserId",
                table: "AUDIT_LOG",
                column: "UserId",
                principalTable: "USER",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RESERVATION_SEAT_SeatId",
                table: "RESERVATION",
                column: "SeatId",
                principalTable: "SEAT",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RESERVATION_USER_UserId",
                table: "RESERVATION",
                column: "UserId",
                principalTable: "USER",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SEAT_SECTOR_SectorId",
                table: "SEAT",
                column: "SectorId",
                principalTable: "SECTOR",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SECTOR_EVENT_EventId",
                table: "SECTOR",
                column: "EventId",
                principalTable: "EVENT",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
