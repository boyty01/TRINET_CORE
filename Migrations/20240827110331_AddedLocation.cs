using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TRINET_CORE.Migrations
{
    /// <inheritdoc />
    public partial class AddedLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("4a024edc-36a8-44ba-813c-c699af63f9d2"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7342d8c3-a4cb-40aa-8d87-f95d9c421c06"));

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("6ed97c3b-834e-44d1-ba46-82a34602b14a"), "Default" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "LocationId", "Password", "PasswordResetRequired", "RefreshToken", "RefreshTokenExpiry", "UserAccessLevel", "Username" },
                values: new object[] { new Guid("4c83c9e8-ad41-474e-baca-81006ca49aae"), new Guid("6ed97c3b-834e-44d1-ba46-82a34602b14a"), "AQAAAAIAAYagAAAAEG5VLyLJvilJYISDYKZCT7IWyHRRBRh/F3G0Dtn7YkIKbAT3fj+M/XQwTmKW4lFMng==", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("6ed97c3b-834e-44d1-ba46-82a34602b14a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("4c83c9e8-ad41-474e-baca-81006ca49aae"));

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("4a024edc-36a8-44ba-813c-c699af63f9d2"), "Default" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "PasswordResetRequired", "RefreshToken", "RefreshTokenExpiry", "UserAccessLevel", "Username" },
                values: new object[] { new Guid("7342d8c3-a4cb-40aa-8d87-f95d9c421c06"), "AQAAAAIAAYagAAAAEHyQnk92+LLRzj31MKAFXWFR6fwsGY0TkmprE8m7PDGlbweylFymb9rZ0xmRc1i3IA==", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Admin" });
        }
    }
}
