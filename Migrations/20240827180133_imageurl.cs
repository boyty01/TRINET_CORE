using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TRINET_CORE.Migrations
{
    /// <inheritdoc />
    public partial class imageurl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("6ed97c3b-834e-44d1-ba46-82a34602b14a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("4c83c9e8-ad41-474e-baca-81006ca49aae"));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Rooms",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("88dc8a7a-826e-4832-9633-14103fc06176"), "Default" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "LocationId", "Password", "PasswordResetRequired", "RefreshToken", "RefreshTokenExpiry", "UserAccessLevel", "Username" },
                values: new object[] { new Guid("8be30be0-58d0-4de7-89e5-8f036481baab"), new Guid("88dc8a7a-826e-4832-9633-14103fc06176"), "AQAAAAIAAYagAAAAEEUWF8cjdkSXDL+Pfl6RrUaaQg6Y7T2PryCCeF2O1al5YK5cFQbEvTw8+dTbA11/Lw==", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("88dc8a7a-826e-4832-9633-14103fc06176"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8be30be0-58d0-4de7-89e5-8f036481baab"));

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Rooms");

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("6ed97c3b-834e-44d1-ba46-82a34602b14a"), "Default" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "LocationId", "Password", "PasswordResetRequired", "RefreshToken", "RefreshTokenExpiry", "UserAccessLevel", "Username" },
                values: new object[] { new Guid("4c83c9e8-ad41-474e-baca-81006ca49aae"), new Guid("6ed97c3b-834e-44d1-ba46-82a34602b14a"), "AQAAAAIAAYagAAAAEG5VLyLJvilJYISDYKZCT7IWyHRRBRh/F3G0Dtn7YkIKbAT3fj+M/XQwTmKW4lFMng==", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Admin" });
        }
    }
}
