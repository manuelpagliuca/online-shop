using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ubique.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingHierarchyToFaucets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubType",
                table: "Faucets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Faucets",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubType",
                value: "Monoleva");

            migrationBuilder.UpdateData(
                table: "Faucets",
                keyColumn: "Id",
                keyValue: 2,
                column: "SubType",
                value: "Due maniglie");

            migrationBuilder.UpdateData(
                table: "Faucets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "SubType", "Type" },
                values: new object[] { "Termostatici", "Lavabo" });

            migrationBuilder.InsertData(
                table: "Faucets",
                columns: new[] { "Id", "Brand", "Color", "DisplayOrder", "Name", "SpoutLength", "SubType", "Type" },
                values: new object[,]
                {
                    { 4, "NOBILI", "Metal", 4, "ATOMIX 4.5", "30mm", "Monoleva", "Lavabo" },
                    { 5, "NOBILI", "Metal", 5, "HyperX", "25mm", "", "Bidet" },
                    { 6, "Paini", "Metal", 6, "JUMPER", "30mm", "", "Cascata" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Faucets",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Faucets",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Faucets",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "SubType",
                table: "Faucets");

            migrationBuilder.UpdateData(
                table: "Faucets",
                keyColumn: "Id",
                keyValue: 3,
                column: "Type",
                value: "Rubinetteria Lavabo");
        }
    }
}
