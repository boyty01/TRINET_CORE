using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TRINET_CORE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("d122f9af-7af7-4e80-953d-5096d4d5ccc6"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("6b105e10-b554-4e59-aa0a-e1aa0be3e1de"));

            migrationBuilder.AddColumn<string>(
                name: "MacAddress",
                table: "Devices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("b20ec704-a3e3-493a-952e-9ee06eeffeb5"), "Default" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "LocationId", "Password", "PasswordResetRequired", "RefreshToken", "RefreshTokenExpiry", "UserAccessLevel", "Username" },
                values: new object[] { new Guid("cc83b1c3-20b2-4ae4-b81f-aeb2da3c0515"), new Guid("b20ec704-a3e3-493a-952e-9ee06eeffeb5"), "AQAAAAIAAYagAAAAEJlUNwBNFK7izjkdcqDFs7fnkJw54tpm38asXEA6+oqM+l6unRPwpA5o+FLwOfNbuw==", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("b20ec704-a3e3-493a-952e-9ee06eeffeb5"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cc83b1c3-20b2-4ae4-b81f-aeb2da3c0515"));

            migrationBuilder.DropColumn(
                name: "MacAddress",
                table: "Devices");

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("d122f9af-7af7-4e80-953d-5096d4d5ccc6"), "Default" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "LocationId", "Password", "PasswordResetRequired", "RefreshToken", "RefreshTokenExpiry", "UserAccessLevel", "Username" },
                values: new object[] { new Guid("6b105e10-b554-4e59-aa0a-e1aa0be3e1de"), new Guid("d122f9af-7af7-4e80-953d-5096d4d5ccc6"), "AQAAAAIAAYagAAAAEFMEaCOM/Gqro91lKAzhEYtBgGRVzN168/JPNFGjFkcblte62AA4zpBJiffzgRr9Cw==", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Admin" });
        }
    }
}
