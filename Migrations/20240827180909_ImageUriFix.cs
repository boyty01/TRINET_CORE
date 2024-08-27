using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TRINET_CORE.Migrations
{
    /// <inheritdoc />
    public partial class ImageUriFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("8cccc9cc-0663-4cfa-aa20-5a876c347260"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e8e18d57-4a20-47ca-9d1e-e8b949aee18e"));

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("d122f9af-7af7-4e80-953d-5096d4d5ccc6"), "Default" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "LocationId", "Password", "PasswordResetRequired", "RefreshToken", "RefreshTokenExpiry", "UserAccessLevel", "Username" },
                values: new object[] { new Guid("6b105e10-b554-4e59-aa0a-e1aa0be3e1de"), new Guid("d122f9af-7af7-4e80-953d-5096d4d5ccc6"), "AQAAAAIAAYagAAAAEFMEaCOM/Gqro91lKAzhEYtBgGRVzN168/JPNFGjFkcblte62AA4zpBJiffzgRr9Cw==", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("d122f9af-7af7-4e80-953d-5096d4d5ccc6"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("6b105e10-b554-4e59-aa0a-e1aa0be3e1de"));

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("8cccc9cc-0663-4cfa-aa20-5a876c347260"), "Default" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "LocationId", "Password", "PasswordResetRequired", "RefreshToken", "RefreshTokenExpiry", "UserAccessLevel", "Username" },
                values: new object[] { new Guid("e8e18d57-4a20-47ca-9d1e-e8b949aee18e"), new Guid("8cccc9cc-0663-4cfa-aa20-5a876c347260"), "AQAAAAIAAYagAAAAEN2HtfRRqZLJn1JL07OV9ngM/BE7fMLymekcyqMMP2m+PpkFdxfw0aEXxejsR0Gm2A==", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Admin" });
        }
    }
}
