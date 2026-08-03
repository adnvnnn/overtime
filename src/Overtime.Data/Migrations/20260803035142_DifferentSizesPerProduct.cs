using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Overtime.Data.Migrations
{
    /// <inheritdoc />
    public partial class DifferentSizesPerProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // NOTE: this block is intentionally applied in descending Id order.
            // Sizes 40-45 shift up by one to 41-46, and (ProductId, Size) is unique,
            // so an ascending update order would collide with the next row's
            // still-unshifted value (e.g. Id 7 -> 41 while Id 8 is still 41).
            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 46, 12 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 45, 10 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 44, 8 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 43, 6 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 42, 4 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 41, 14 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 38, 8 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 39, 10 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 40, 12 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 41, 14 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 42, 4 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 43, 6 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "ProductId", "Size", "StockQty" },
                values: new object[] { "street-low", 44, 8 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 36, 4 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 37, 6 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 38, 8 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 39, 10 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 40, 12 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 25,
                column: "Size",
                value: 28);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 26,
                column: "Size",
                value: 29);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 27,
                column: "Size",
                value: 30);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 28,
                column: "Size",
                value: 31);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 29,
                column: "Size",
                value: 32);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 30,
                column: "Size",
                value: 33);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "ProductId", "Size" },
                values: new object[] { "mini-dash", 34 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 40, 12 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 41, 14 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 42, 4 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 43, 6 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 44, 8 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 40, 12 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 41, 14 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 42, 4 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 43, 6 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 44, 8 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 45, 10 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 40, 12 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 41, 14 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 42, 4 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 43, 6 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 44, 8 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 45, 10 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "ProductId", "Size", "StockQty" },
                values: new object[] { "aria-knit", 40, 12 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 41, 14 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 42, 4 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 43, 6 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 44, 8 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 45, 10 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 25,
                column: "Size",
                value: 40);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 26,
                column: "Size",
                value: 41);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 27,
                column: "Size",
                value: 42);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 28,
                column: "Size",
                value: 43);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 29,
                column: "Size",
                value: 44);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 30,
                column: "Size",
                value: 45);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "ProductId", "Size" },
                values: new object[] { "overtime-06", 40 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 41, 14 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 42, 4 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 43, 6 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 44, 8 });

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "Size", "StockQty" },
                values: new object[] { 45, 10 });
        }
    }
}
