using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WorkFound.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Auth1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AppUserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminProfile_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("46de1a26-355f-4901-b7c4-7c122f6d1fa7"), null, "User", "USER" },
                    { new Guid("b0a3d7f1-f403-4f5e-9457-f7f8bcb963a9"), null, "Admin", "ADMIN" },
                    { new Guid("cfa01de5-1f98-4907-a8a5-9b42a6bb61dd"), null, "Company", "COMPANY" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminProfile_AppUserId",
                table: "AdminProfile",
                column: "AppUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminProfile");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("46de1a26-355f-4901-b7c4-7c122f6d1fa7"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b0a3d7f1-f403-4f5e-9457-f7f8bcb963a9"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cfa01de5-1f98-4907-a8a5-9b42a6bb61dd"));
        }
    }
}
