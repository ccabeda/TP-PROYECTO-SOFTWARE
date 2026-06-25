using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_PROYECTO_SOFTWARE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiresAt",
                table: "USER",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshTokenHash",
                table: "USER",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_USER_RefreshTokenHash",
                table: "USER",
                column: "RefreshTokenHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_USER_RefreshTokenHash",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiresAt",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "RefreshTokenHash",
                table: "USER");
        }
    }
}
