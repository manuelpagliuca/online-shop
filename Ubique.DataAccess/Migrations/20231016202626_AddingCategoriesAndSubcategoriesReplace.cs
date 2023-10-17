using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ubique.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingCategoriesAndSubcategoriesReplace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Faucets");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Lavabo" },
                    { 2, 2, "Bidet" },
                    { 3, 3, "Lavabo" },
                    { 4, 4, "Lavabo" },
                    { 5, 5, "Bidet" },
                    { 6, 6, "Cascata" }
                });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "Id", "CategoryId", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { 1, 1, 1, "Monoleva" },
                    { 2, 1, 2, "Due maniglie" },
                    { 3, 6, 3, "Termostatico" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.CreateTable(
                name: "Faucets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpoutLength = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faucets", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Faucets",
                columns: new[] { "Id", "Brand", "Color", "DisplayOrder", "Name", "SpoutLength", "SubType", "Type" },
                values: new object[,]
                {
                    { 1, "Paini", "Metal", 1, "ATOMIX 3.0", "30mm", "Monoleva", "Lavabo" },
                    { 2, "NOBILI", "Metal", 2, "Svastun", "25mm", "Due maniglie", "Bidet" },
                    { 3, "Paini", "Metal", 3, "ATOMIX 11.0", "30mm", "Termostatici", "Lavabo" },
                    { 4, "NOBILI", "Metal", 4, "ATOMIX 4.5", "30mm", "Monoleva", "Lavabo" },
                    { 5, "NOBILI", "Metal", 5, "HyperX", "25mm", "", "Bidet" },
                    { 6, "Paini", "Metal", 6, "JUMPER", "30mm", "", "Cascata" }
                });
        }
    }
}
