using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Overtime.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductBrandAndUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Usage",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "air-pulse",
                columns: new[] { "Brand", "Category", "Usage" },
                values: new object[] { "اورتایم", "مردانه", "رانینگ" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "aria-knit",
                columns: new[] { "Brand", "Category", "Usage" },
                values: new object[] { "اورتایم", "زنانه", "نیت" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "highjump-x",
                columns: new[] { "Brand", "Category", "Usage" },
                values: new object[] { "اورتایم", "مردانه", "بسکتبال" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "mini-dash",
                columns: new[] { "Brand", "Category", "Usage" },
                values: new object[] { "اورتایم", "بچگانه", "روزمره" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "overtime-06",
                columns: new[] { "Brand", "Usage" },
                values: new object[] { "اورتایم", "کالکشنی" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "street-low",
                columns: new[] { "Brand", "Category", "Usage" },
                values: new object[] { "اورتایم", "یونیسکس", "روزمره" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Usage",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "air-pulse",
                column: "Category",
                value: "مردانه · رانینگ");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "aria-knit",
                column: "Category",
                value: "زنانه · نیت");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "highjump-x",
                column: "Category",
                value: "مردانه · بسکتبال");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "mini-dash",
                column: "Category",
                value: "بچگانه · روزمره");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "street-low",
                column: "Category",
                value: "یونیسکس · روزمره");
        }
    }
}
