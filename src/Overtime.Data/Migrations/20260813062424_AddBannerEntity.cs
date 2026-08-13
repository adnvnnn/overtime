using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Overtime.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBannerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Badge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<long>(type: "bigint", nullable: true),
                    LinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Banners",
                columns: new[] { "Id", "Badge", "DisplayOrder", "ImageUrl", "IsActive", "Kind", "LinkUrl", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "№06", 0, "assets/images/products/product-9.jpg", true, 0, null, null, null },
                    { 2, "№02", 1, "assets/images/products/product-3.jpg", true, 0, null, null, null },
                    { 3, "№04", 2, "assets/images/products/product-6.jpg", true, 0, null, null, null },
                    { 4, "№08", 3, "assets/images/products/product-12.jpg", true, 0, null, null, null },
                    { 5, "پرفروش", 0, "assets/images/products/product-1.jpg", true, 1, "/product/air-pulse", 2950000L, "Air Pulse" },
                    { 6, "جدید", 1, "assets/images/products/product-2.jpg", true, 1, "/product/highjump-x", 3800000L, "Highjump X" },
                    { 7, "تخفیف", 2, "assets/images/products/product-4.jpg", true, 1, "/product/aria-knit", 2650000L, "Aria Knit" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");
        }
    }
}
