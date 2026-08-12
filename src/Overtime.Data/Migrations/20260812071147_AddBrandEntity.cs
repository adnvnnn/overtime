using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Overtime.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "DisplayOrder", "LogoUrl", "Name", "Slug" },
                values: new object[,]
                {
                    { 1, 0, null, "اورتایم", "overtime" },
                    { 2, 1, "assets/images/brands/nike-logo.svg", "Nike", "nike" },
                    { 3, 2, "assets/images/brands/adidas-logo.svg", "Adidas", "adidas" },
                    { 4, 3, "assets/images/brands/airjordan-logo.svg", "Air Jordan", "airjordan" },
                    { 5, 4, "assets/images/brands/new-balance-logo.svg", "New Balance", "new-balance" },
                    { 6, 5, "assets/images/brands/puma-logo.svg", "Puma", "puma" },
                    { 7, 6, "assets/images/brands/asics-logo.svg", "Asics", "asics" },
                    { 8, 7, "assets/images/brands/reebok-logo.svg", "Reebok", "reebok" },
                    { 9, 8, "assets/images/brands/converse-logo.svg", "Converse", "converse" },
                    { 10, 9, "assets/images/brands/hoka-logo.svg", "Hoka", "hoka" },
                    { 11, 10, "assets/images/brands/salomon-logo.svg", "Salomon", "salomon" },
                    { 12, 11, "assets/images/brands/saucony-logo.svg", "Saucony", "saucony" },
                    { 13, 12, "assets/images/brands/skechers-logo.svg", "Skechers", "skechers" },
                    { 14, 13, "assets/images/brands/brooks-logo.svg", "Brooks", "brooks" },
                    { 15, 14, "assets/images/brands/mizuno-logo.svg", "Mizuno", "mizuno" },
                    { 16, 15, "assets/images/brands/under-armour-logo.svg", "Under Armour", "under-armour" },
                    { 17, 16, "assets/images/brands/fila-logo.svg", "Fila", "fila" },
                    { 18, 17, "assets/images/brands/timberland-logo.svg", "Timberland", "timberland" },
                    { 19, 18, "assets/images/brands/columbia-logo.svg", "Columbia", "columbia" },
                    { 20, 19, "assets/images/brands/lacoste-logo.svg", "Lacoste", "lacoste" },
                    { 21, 20, "assets/images/brands/diesel-logo.svg", "Diesel", "diesel" },
                    { 22, 21, "assets/images/brands/amiri-logo.svg", "Amiri", "amiri" },
                    { 23, 22, "assets/images/brands/balenciaga-logo.svg", "Balenciaga", "balenciaga" },
                    { 24, 23, "assets/images/brands/louis-vuitton-logo.svg", "Louis Vuitton", "louis-vuitton" },
                    { 25, 24, "assets/images/brands/alexander-macqueen-logo.svg", "Alexander McQueen", "alexander-macqueen" },
                    { 26, 25, "assets/images/brands/vico-logo.svg", "Vico", "vico" }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "air-pulse",
                column: "BrandId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "aria-knit",
                column: "BrandId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "highjump-x",
                column: "BrandId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "mini-dash",
                column: "BrandId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "overtime-06",
                column: "BrandId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "street-low",
                column: "BrandId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Slug",
                table: "Brands",
                column: "Slug",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "air-pulse",
                column: "Brand",
                value: "اورتایم");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "aria-knit",
                column: "Brand",
                value: "اورتایم");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "highjump-x",
                column: "Brand",
                value: "اورتایم");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "mini-dash",
                column: "Brand",
                value: "اورتایم");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "overtime-06",
                column: "Brand",
                value: "اورتایم");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "street-low",
                column: "Brand",
                value: "اورتایم");
        }
    }
}
