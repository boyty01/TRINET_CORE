using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TRINET_CORE.Migrations
{
    /// <inheritdoc />
    public partial class ImageUri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("88dc8a7a-826e-4832-9633-14103fc06176"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8be30be0-58d0-4de7-89e5-8f036481baab"));

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("8cccc9cc-0663-4cfa-aa20-5a876c347260"), "Default" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "LocationId", "Password", "PasswordResetRequired", "RefreshToken", "RefreshTokenExpiry", "UserAccessLevel", "Username" },
                values: new object[] { new Guid("e8e18d57-4a20-47ca-9d1e-e8b949aee18e"), new Guid("8cccc9cc-0663-4cfa-aa20-5a876c347260"), "AQAAAAIAAYagAAAAEN2HtfRRqZLJn1JL07OV9ngM/BE7fMLymekcyqMMP2m+PpkFdxfw0aEXxejsR0Gm2A==", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { new Guid("88dc8a7a-826e-4832-9633-14103fc06176"), "Default" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "LocationId", "Password", "PasswordResetRequired", "RefreshToken", "RefreshTokenExpiry", "UserAccessLevel", "Username" },
                values: new object[] { new Guid("8be30be0-58d0-4de7-89e5-8f036481baab"), new Guid("88dc8a7a-826e-4832-9633-14103fc06176"), "AQAAAAIAAYagAAAAEEUWF8cjdkSXDL+Pfl6RrUaaQg6Y7T2PryCCeF2O1al5YK5cFQbEvTw8+dTbA11/Lw==", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Admin" });
        }
    }
}
