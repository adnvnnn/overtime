using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Overtime.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSlugAndIntId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // EF's DeleteData generator mis-renders string literals for these composite-key (and
            // string-key) seed rows as unquoted SQL (e.g. "ProductId = air-pulse" instead of
            // "ProductId = N'air-pulse'"), which SQL Server rejects. Raw SQL sidesteps it — every row
            // in these three tables is the full known seed set being replaced below anyway.
            migrationBuilder.Sql("DELETE FROM [ProductCategory];");
            migrationBuilder.Sql("DELETE FROM [ProductUsage];");
            migrationBuilder.Sql("DELETE FROM [Products];");

            // These three tables aren't managed via HasData (Orders/OrderItems seed at runtime via
            // DemoDataSeeder, CartItems/Reviews are live user data), so EF has no way to generate a
            // fix-up for their ProductId values before the type change below. Since this project's
            // data is disposable demo/dev content, clear them instead of trying to convert values EF
            // never saw — Orders/OrderItems get reseeded automatically on next app startup, CartItems
            // and Reviews just start empty again.
            migrationBuilder.Sql("DELETE FROM [Orders];");
            migrationBuilder.Sql("DELETE FROM [CartItems];");
            migrationBuilder.Sql("DELETE FROM [Reviews];");

            // EF's AlterColumnOperation generator auto-drops/recreates a column's OWN dependent indexes
            // and default constraints, but not foreign keys that point AT it from other tables — SQL
            // Server refuses "ALTER COLUMN" while any such FK still references the column. Every one of
            // these 7 FKs points at Products.Id (via ProductId), so they're all dropped up front here
            // and rebuilt once, together, after every ProductId column (and Products.Id itself) is int.
            migrationBuilder.DropForeignKey(name: "FK_CartItems_Products_ProductId", table: "CartItems");
            migrationBuilder.DropForeignKey(name: "FK_ProductImages_Products_ProductId", table: "ProductImages");
            migrationBuilder.DropForeignKey(name: "FK_ProductSizes_Products_ProductId", table: "ProductSizes");
            migrationBuilder.DropForeignKey(name: "FK_ProductSpecs_Products_ProductId", table: "ProductSpecs");
            migrationBuilder.DropForeignKey(name: "FK_Reviews_Products_ProductId", table: "Reviews");
            migrationBuilder.DropForeignKey(name: "FK_ProductCategory_Products_ProductId", table: "ProductCategory");
            migrationBuilder.DropForeignKey(name: "FK_ProductUsage_Products_ProductId", table: "ProductUsage");

            // Same reasoning as the FKs above: ProductId is part of these two join tables' own
            // composite primary key, and SQL Server refuses to ALTER COLUMN a column that's part of any
            // PK, not just the Products.Id one. Drop and rebuild around the type change.
            migrationBuilder.DropPrimaryKey(name: "PK_ProductCategory", table: "ProductCategory");
            migrationBuilder.DropPrimaryKey(name: "PK_ProductUsage", table: "ProductUsage");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Reviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductUsage",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            // ProductSpecs/ProductSizes/ProductImages rows already exist (unlike Products above, EF
            // updates them in place rather than delete+reinsert). Their ProductId values are fixed up
            // here, WHILE the column is still nvarchar — an int value going into an nvarchar column is
            // always a safe implicit conversion. Only after that is the column type actually changed
            // to int, by which point every value is a clean numeric string ("1", "2", ...) rather than
            // the old slugs ("air-pulse", ...), which is what a naive string->int ALTER COLUMN (run
            // before these value fix-ups) would otherwise fail on.
            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProductId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProductId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProductId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProductId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 5,
                column: "ProductId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 6,
                column: "ProductId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 7,
                column: "ProductId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 8,
                column: "ProductId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 9,
                column: "ProductId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 10,
                column: "ProductId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 11,
                column: "ProductId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 12,
                column: "ProductId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 13,
                column: "ProductId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 14,
                column: "ProductId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 15,
                column: "ProductId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 16,
                column: "ProductId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 17,
                column: "ProductId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 18,
                column: "ProductId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 19,
                column: "ProductId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 20,
                column: "ProductId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 21,
                column: "ProductId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 22,
                column: "ProductId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 23,
                column: "ProductId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 24,
                column: "ProductId",
                value: 6);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductSpecs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProductId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProductId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProductId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProductId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 5,
                column: "ProductId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 6,
                column: "ProductId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 7,
                column: "ProductId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 8,
                column: "ProductId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 9,
                column: "ProductId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 10,
                column: "ProductId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 11,
                column: "ProductId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 12,
                column: "ProductId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 13,
                column: "ProductId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 14,
                column: "ProductId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 15,
                column: "ProductId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 16,
                column: "ProductId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 17,
                column: "ProductId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 18,
                column: "ProductId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 19,
                column: "ProductId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 20,
                column: "ProductId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 21,
                column: "ProductId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 22,
                column: "ProductId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 23,
                column: "ProductId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 24,
                column: "ProductId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 25,
                column: "ProductId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 26,
                column: "ProductId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 27,
                column: "ProductId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 28,
                column: "ProductId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 29,
                column: "ProductId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 30,
                column: "ProductId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 31,
                column: "ProductId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 32,
                column: "ProductId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 33,
                column: "ProductId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 34,
                column: "ProductId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 35,
                column: "ProductId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 36,
                column: "ProductId",
                value: 6);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductSizes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProductId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProductId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProductId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProductId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 5,
                column: "ProductId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 6,
                column: "ProductId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 7,
                column: "ProductId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 8,
                column: "ProductId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 9,
                column: "ProductId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 10,
                column: "ProductId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 11,
                column: "ProductId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 12,
                column: "ProductId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 13,
                column: "ProductId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 14,
                column: "ProductId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 15,
                column: "ProductId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 16,
                column: "ProductId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 17,
                column: "ProductId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 18,
                column: "ProductId",
                value: 6);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductCategory",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "CartItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            // Every FK-side ProductId column is int now, matching each other. Converting Products.Id
            // itself to an IDENTITY int column is the one operation EF's SQL Server migration
            // generator refuses to emit as a plain ALTER COLUMN ("the column needs to be dropped and
            // recreated"), so it's done here by hand: drop+recreate the PK column (the FKs pointing at
            // it were already dropped above), then rebuild the FKs (Products has already been emptied
            // by the DeleteData calls above, so there's no data to lose).
            migrationBuilder.DropPrimaryKey(name: "PK_Products", table: "Products");

            migrationBuilder.DropColumn(name: "Id", table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Products",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(name: "PK_Products", table: "Products", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_ProductCategory", table: "ProductCategory", columns: new[] { "ProductId", "CategoryId" });
            migrationBuilder.AddPrimaryKey(name: "PK_ProductUsage", table: "ProductUsage", columns: new[] { "ProductId", "UsageId" });

            migrationBuilder.AddForeignKey(name: "FK_CartItems_Products_ProductId", table: "CartItems", column: "ProductId", principalTable: "Products", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_ProductImages_Products_ProductId", table: "ProductImages", column: "ProductId", principalTable: "Products", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_ProductSizes_Products_ProductId", table: "ProductSizes", column: "ProductId", principalTable: "Products", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_ProductSpecs_Products_ProductId", table: "ProductSpecs", column: "ProductId", principalTable: "Products", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_Reviews_Products_ProductId", table: "Reviews", column: "ProductId", principalTable: "Products", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_ProductCategory_Products_ProductId", table: "ProductCategory", column: "ProductId", principalTable: "Products", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_ProductUsage_Products_ProductId", table: "ProductUsage", column: "ProductId", principalTable: "Products", principalColumn: "Id", onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Products",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Slug",
                value: "men");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Slug",
                value: "unisex");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Slug",
                value: "women");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Slug",
                value: "kids");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "Slug",
                value: "limited-edition");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Badge", "BrandId", "Description", "DisplayOrder", "Name", "Price", "Rating", "ReviewCount", "Slug", "Swatch" },
                values: new object[,]
                {
                    { 1, "پرفروش", 1, "کتانی رانینگ Air Pulse برای دویدن‌های روزمره و تمرین‌های سبک طراحی شده است. رویه‌ی مش تنفس‌پذیر همراه با میان‌کفش سبک، احساس راحتی را در هر قدم حفظ می‌کند.", 0, "Air Pulse", 2950000L, 4.7999999999999998, 124, "air-pulse", "accent" },
                    { 2, "جدید", 1, "Highjump X با میان‌کفش ضربه‌گیر و ساق بلندتر، برای حرکات ناگهانی در زمین بسکتبال طراحی شده و پایداری مچ پا را در فرودها افزایش می‌دهد.", 1, "Highjump X", 3800000L, 4.7000000000000002, 98, "highjump-x", "accent-2" },
                    { 3, null, 1, "یک کتانی روزمره‌ی ساده و همه‌کاره؛ Street Low با طراحی مینیمال و رنگ خنثی، با هر پوششی هم‌نشین می‌شود.", 2, "Street Low", 2200000L, 4.5999999999999996, 156, "street-low", "ink" },
                    { 4, "تخفیف", 1, "Aria Knit با رویه‌ی نیت کشباف، فرمی نزدیک به فرم پا دارد و برای پوشیدن طولانی‌مدت در طول روز بسیار راحت است.", 3, "Aria Knit", 2650000L, 4.7999999999999998, 142, "aria-knit", "accent-2" },
                    { 5, null, 1, "Mini Dash مدلی سبک و بادوام برای کودکان است؛ زیره‌ی ضدلغزش و بست چسبی، پوشیدن و درآوردن را برای کودکان آسان می‌کند.", 4, "Mini Dash", 1850000L, 4.9000000000000004, 86, "mini-dash", "accent" },
                    { 6, "لیمیتد", 1, "Overtime №06 نسخه‌ی محدود امضای برند اورتایم است؛ تیراژ کم، جزئیات ویژه و جعبه‌ی اختصاصی این مدل را به یک قطعه‌ی کالکشنی تبدیل می‌کند.", 5, "Overtime №06", 4500000L, 5.0, 41, "overtime-06", "ink" }
                });

            migrationBuilder.InsertData(
                table: "ProductCategory",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 3 },
                    { 3, 4 },
                    { 4, 5 },
                    { 5, 6 }
                });

            migrationBuilder.InsertData(
                table: "ProductUsage",
                columns: new[] { "ProductId", "UsageId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 5, 3 },
                    { 6, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Slug",
                table: "Products",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Slug",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Slug",
                table: "Categories");

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 4, 5 });

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 5, 6 });

            migrationBuilder.DeleteData(
                table: "ProductUsage",
                keyColumns: new[] { "ProductId", "UsageId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "ProductUsage",
                keyColumns: new[] { "ProductId", "UsageId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "ProductUsage",
                keyColumns: new[] { "ProductId", "UsageId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "ProductUsage",
                keyColumns: new[] { "ProductId", "UsageId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "ProductUsage",
                keyColumns: new[] { "ProductId", "UsageId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "ProductUsage",
                keyColumns: new[] { "ProductId", "UsageId" },
                keyValues: new object[] { 6, 5 });

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Categories");

            // Mirror of the Up() identity handling: SQL Server can't ALTER COLUMN a column out of
            // IDENTITY either, so drop the FKs and the column, recreate it as a plain nvarchar PK, then
            // rebuild the FKs (Products is empty at this point, having just had its seed rows deleted).
            migrationBuilder.DropForeignKey(name: "FK_CartItems_Products_ProductId", table: "CartItems");
            migrationBuilder.DropForeignKey(name: "FK_ProductImages_Products_ProductId", table: "ProductImages");
            migrationBuilder.DropForeignKey(name: "FK_ProductSizes_Products_ProductId", table: "ProductSizes");
            migrationBuilder.DropForeignKey(name: "FK_ProductSpecs_Products_ProductId", table: "ProductSpecs");
            migrationBuilder.DropForeignKey(name: "FK_Reviews_Products_ProductId", table: "Reviews");
            migrationBuilder.DropForeignKey(name: "FK_ProductCategory_Products_ProductId", table: "ProductCategory");
            migrationBuilder.DropForeignKey(name: "FK_ProductUsage_Products_ProductId", table: "ProductUsage");

            // ProductId is also part of these two join tables' own composite PK, which equally blocks
            // ALTER COLUMN on it — drop and rebuild around the type change, same as Up().
            migrationBuilder.DropPrimaryKey(name: "PK_ProductCategory", table: "ProductCategory");
            migrationBuilder.DropPrimaryKey(name: "PK_ProductUsage", table: "ProductUsage");

            migrationBuilder.DropPrimaryKey(name: "PK_Products", table: "Products");

            migrationBuilder.DropColumn(name: "Id", table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(name: "PK_Products", table: "Products", column: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "Reviews",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "ProductUsage",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "ProductSpecs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "ProductSizes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "ProductImages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "ProductCategory",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "CartItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(name: "PK_ProductCategory", table: "ProductCategory", columns: new[] { "ProductId", "CategoryId" });
            migrationBuilder.AddPrimaryKey(name: "PK_ProductUsage", table: "ProductUsage", columns: new[] { "ProductId", "UsageId" });

            migrationBuilder.AddForeignKey(name: "FK_CartItems_Products_ProductId", table: "CartItems", column: "ProductId", principalTable: "Products", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_ProductImages_Products_ProductId", table: "ProductImages", column: "ProductId", principalTable: "Products", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_ProductSizes_Products_ProductId", table: "ProductSizes", column: "ProductId", principalTable: "Products", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_ProductSpecs_Products_ProductId", table: "ProductSpecs", column: "ProductId", principalTable: "Products", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_Reviews_Products_ProductId", table: "Reviews", column: "ProductId", principalTable: "Products", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_ProductCategory_Products_ProductId", table: "ProductCategory", column: "ProductId", principalTable: "Products", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_ProductUsage_Products_ProductId", table: "ProductUsage", column: "ProductId", principalTable: "Products", principalColumn: "Id", onDelete: ReferentialAction.Cascade);

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProductId",
                value: "air-pulse");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProductId",
                value: "air-pulse");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProductId",
                value: "air-pulse");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProductId",
                value: "highjump-x");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 5,
                column: "ProductId",
                value: "highjump-x");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 6,
                column: "ProductId",
                value: "highjump-x");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 7,
                column: "ProductId",
                value: "street-low");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 8,
                column: "ProductId",
                value: "street-low");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 9,
                column: "ProductId",
                value: "street-low");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 10,
                column: "ProductId",
                value: "aria-knit");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 11,
                column: "ProductId",
                value: "aria-knit");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 12,
                column: "ProductId",
                value: "aria-knit");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 13,
                column: "ProductId",
                value: "mini-dash");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 14,
                column: "ProductId",
                value: "mini-dash");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 15,
                column: "ProductId",
                value: "mini-dash");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 16,
                column: "ProductId",
                value: "overtime-06");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 17,
                column: "ProductId",
                value: "overtime-06");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 18,
                column: "ProductId",
                value: "overtime-06");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProductId",
                value: "air-pulse");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProductId",
                value: "air-pulse");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProductId",
                value: "air-pulse");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProductId",
                value: "air-pulse");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 5,
                column: "ProductId",
                value: "air-pulse");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 6,
                column: "ProductId",
                value: "air-pulse");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 7,
                column: "ProductId",
                value: "highjump-x");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 8,
                column: "ProductId",
                value: "highjump-x");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 9,
                column: "ProductId",
                value: "highjump-x");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 10,
                column: "ProductId",
                value: "highjump-x");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 11,
                column: "ProductId",
                value: "highjump-x");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 12,
                column: "ProductId",
                value: "highjump-x");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 13,
                column: "ProductId",
                value: "street-low");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 14,
                column: "ProductId",
                value: "street-low");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 15,
                column: "ProductId",
                value: "street-low");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 16,
                column: "ProductId",
                value: "street-low");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 17,
                column: "ProductId",
                value: "street-low");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 18,
                column: "ProductId",
                value: "street-low");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 19,
                column: "ProductId",
                value: "street-low");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 20,
                column: "ProductId",
                value: "aria-knit");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 21,
                column: "ProductId",
                value: "aria-knit");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 22,
                column: "ProductId",
                value: "aria-knit");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 23,
                column: "ProductId",
                value: "aria-knit");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 24,
                column: "ProductId",
                value: "aria-knit");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 25,
                column: "ProductId",
                value: "mini-dash");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 26,
                column: "ProductId",
                value: "mini-dash");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 27,
                column: "ProductId",
                value: "mini-dash");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 28,
                column: "ProductId",
                value: "mini-dash");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 29,
                column: "ProductId",
                value: "mini-dash");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 30,
                column: "ProductId",
                value: "mini-dash");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 31,
                column: "ProductId",
                value: "mini-dash");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 32,
                column: "ProductId",
                value: "overtime-06");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 33,
                column: "ProductId",
                value: "overtime-06");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 34,
                column: "ProductId",
                value: "overtime-06");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 35,
                column: "ProductId",
                value: "overtime-06");

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 36,
                column: "ProductId",
                value: "overtime-06");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProductId",
                value: "air-pulse");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProductId",
                value: "air-pulse");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProductId",
                value: "air-pulse");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProductId",
                value: "air-pulse");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 5,
                column: "ProductId",
                value: "highjump-x");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 6,
                column: "ProductId",
                value: "highjump-x");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 7,
                column: "ProductId",
                value: "highjump-x");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 8,
                column: "ProductId",
                value: "highjump-x");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 9,
                column: "ProductId",
                value: "street-low");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 10,
                column: "ProductId",
                value: "street-low");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 11,
                column: "ProductId",
                value: "street-low");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 12,
                column: "ProductId",
                value: "street-low");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 13,
                column: "ProductId",
                value: "aria-knit");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 14,
                column: "ProductId",
                value: "aria-knit");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 15,
                column: "ProductId",
                value: "aria-knit");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 16,
                column: "ProductId",
                value: "aria-knit");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 17,
                column: "ProductId",
                value: "mini-dash");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 18,
                column: "ProductId",
                value: "mini-dash");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 19,
                column: "ProductId",
                value: "mini-dash");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 20,
                column: "ProductId",
                value: "mini-dash");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 21,
                column: "ProductId",
                value: "overtime-06");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 22,
                column: "ProductId",
                value: "overtime-06");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 23,
                column: "ProductId",
                value: "overtime-06");

            migrationBuilder.UpdateData(
                table: "ProductSpecs",
                keyColumn: "Id",
                keyValue: 24,
                column: "ProductId",
                value: "overtime-06");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Badge", "BrandId", "Description", "DisplayOrder", "Name", "Price", "Rating", "ReviewCount", "Swatch" },
                values: new object[,]
                {
                    { "air-pulse", "پرفروش", 1, "کتانی رانینگ Air Pulse برای دویدن‌های روزمره و تمرین‌های سبک طراحی شده است. رویه‌ی مش تنفس‌پذیر همراه با میان‌کفش سبک، احساس راحتی را در هر قدم حفظ می‌کند.", 0, "Air Pulse", 2950000L, 4.7999999999999998, 124, "accent" },
                    { "aria-knit", "تخفیف", 1, "Aria Knit با رویه‌ی نیت کشباف، فرمی نزدیک به فرم پا دارد و برای پوشیدن طولانی‌مدت در طول روز بسیار راحت است.", 3, "Aria Knit", 2650000L, 4.7999999999999998, 142, "accent-2" },
                    { "highjump-x", "جدید", 1, "Highjump X با میان‌کفش ضربه‌گیر و ساق بلندتر، برای حرکات ناگهانی در زمین بسکتبال طراحی شده و پایداری مچ پا را در فرودها افزایش می‌دهد.", 1, "Highjump X", 3800000L, 4.7000000000000002, 98, "accent-2" },
                    { "mini-dash", null, 1, "Mini Dash مدلی سبک و بادوام برای کودکان است؛ زیره‌ی ضدلغزش و بست چسبی، پوشیدن و درآوردن را برای کودکان آسان می‌کند.", 4, "Mini Dash", 1850000L, 4.9000000000000004, 86, "accent" },
                    { "overtime-06", "لیمیتد", 1, "Overtime №06 نسخه‌ی محدود امضای برند اورتایم است؛ تیراژ کم، جزئیات ویژه و جعبه‌ی اختصاصی این مدل را به یک قطعه‌ی کالکشنی تبدیل می‌کند.", 5, "Overtime №06", 4500000L, 5.0, 41, "ink" },
                    { "street-low", null, 1, "یک کتانی روزمره‌ی ساده و همه‌کاره؛ Street Low با طراحی مینیمال و رنگ خنثی، با هر پوششی هم‌نشین می‌شود.", 2, "Street Low", 2200000L, 4.5999999999999996, 156, "ink" }
                });

            migrationBuilder.InsertData(
                table: "ProductCategory",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { 1, "air-pulse" },
                    { 3, "aria-knit" },
                    { 1, "highjump-x" },
                    { 4, "mini-dash" },
                    { 5, "overtime-06" },
                    { 2, "street-low" }
                });

            migrationBuilder.InsertData(
                table: "ProductUsage",
                columns: new[] { "ProductId", "UsageId" },
                values: new object[,]
                {
                    { "air-pulse", 1 },
                    { "aria-knit", 4 },
                    { "highjump-x", 2 },
                    { "mini-dash", 3 },
                    { "overtime-06", 5 },
                    { "street-low", 3 }
                });
        }
    }
}
