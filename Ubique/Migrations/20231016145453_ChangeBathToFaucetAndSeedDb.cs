using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ubique.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBathToFaucetAndSeedDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BathComponents");

            migrationBuilder.CreateTable(
                name: "Faucets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpoutLength = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faucets", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Faucets",
                columns: new[] { "Id", "Brand", "Color", "DisplayOrder", "Name", "SpoutLength", "Type" },
                values: new object[,]
                {
                    { 1, "Paini", "Metal", 1, "ATOMIX 3.0", "30mm", "Lavabo" },
                    { 2, "NOBILI", "Metal", 2, "Svastun", "25mm", "Bidet" },
                    { 3, "Paini", "Metal", 3, "ATOMIX 11.0", "30mm", "Rubinetteria Lavabo" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Faucets");

            migrationBuilder.CreateTable(
                name: "BathComponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BathComponents", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BathComponents",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Rubinetteria Lavabo" },
                    { 2, 2, "Rubinetteria Bidet" },
                    { 3, 3, "Rubinetti Giardino" },
                    { 4, 4, "Rubinetti per Camper e Barche" }
                });
        }
    }
}
