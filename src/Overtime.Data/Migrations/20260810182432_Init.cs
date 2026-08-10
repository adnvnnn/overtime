using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Overtime.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtpCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConsumedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Attempts = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Usage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Swatch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Badge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    ReviewCount = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    StockQty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSizes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSpecs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSpecs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Badge", "Brand", "Category", "Description", "DisplayOrder", "Name", "Price", "Rating", "ReviewCount", "Swatch", "Usage" },
                values: new object[,]
                {
                    { "air-pulse", "پرفروش", "اورتایم", "مردانه", "کتانی رانینگ Air Pulse برای دویدن‌های روزمره و تمرین‌های سبک طراحی شده است. رویه‌ی مش تنفس‌پذیر همراه با میان‌کفش سبک، احساس راحتی را در هر قدم حفظ می‌کند.", 0, "Air Pulse", 2950000L, 4.7999999999999998, 124, "accent", "رانینگ" },
                    { "aria-knit", "تخفیف", "اورتایم", "زنانه", "Aria Knit با رویه‌ی نیت کشباف، فرمی نزدیک به فرم پا دارد و برای پوشیدن طولانی‌مدت در طول روز بسیار راحت است.", 3, "Aria Knit", 2650000L, 4.7999999999999998, 142, "accent-2", "نیت" },
                    { "highjump-x", "جدید", "اورتایم", "مردانه", "Highjump X با میان‌کفش ضربه‌گیر و ساق بلندتر، برای حرکات ناگهانی در زمین بسکتبال طراحی شده و پایداری مچ پا را در فرودها افزایش می‌دهد.", 1, "Highjump X", 3800000L, 4.7000000000000002, 98, "accent-2", "بسکتبال" },
                    { "mini-dash", null, "اورتایم", "بچگانه", "Mini Dash مدلی سبک و بادوام برای کودکان است؛ زیره‌ی ضدلغزش و بست چسبی، پوشیدن و درآوردن را برای کودکان آسان می‌کند.", 4, "Mini Dash", 1850000L, 4.9000000000000004, 86, "accent", "روزمره" },
                    { "overtime-06", "لیمیتد", "اورتایم", "لیمیتد ادیشن", "Overtime №06 نسخه‌ی محدود امضای برند اورتایم است؛ تیراژ کم، جزئیات ویژه و جعبه‌ی اختصاصی این مدل را به یک قطعه‌ی کالکشنی تبدیل می‌کند.", 5, "Overtime №06", 4500000L, 5.0, 41, "ink", "کالکشنی" },
                    { "street-low", null, "اورتایم", "یونیسکس", "یک کتانی روزمره‌ی ساده و همه‌کاره؛ Street Low با طراحی مینیمال و رنگ خنثی، با هر پوششی هم‌نشین می‌شود.", 2, "Street Low", 2200000L, 4.5999999999999996, 156, "ink", "روزمره" }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "ProductId", "SortOrder", "Url" },
                values: new object[,]
                {
                    { 1, "air-pulse", 0, "assets/images/products/product-1.jpg" },
                    { 2, "air-pulse", 1, "assets/images/products/product-1.jpg" },
                    { 3, "air-pulse", 2, "assets/images/products/product-1.jpg" },
                    { 4, "highjump-x", 0, "assets/images/products/product-2.jpg" },
                    { 5, "highjump-x", 1, "assets/images/products/product-2.jpg" },
                    { 6, "highjump-x", 2, "assets/images/products/product-2.jpg" },
                    { 7, "street-low", 0, "assets/images/products/product-3.jpg" },
                    { 8, "street-low", 1, "assets/images/products/product-3.jpg" },
                    { 9, "street-low", 2, "assets/images/products/product-3.jpg" },
                    { 10, "aria-knit", 0, "assets/images/products/product-4.jpg" },
                    { 11, "aria-knit", 1, "assets/images/products/product-4.jpg" },
                    { 12, "aria-knit", 2, "assets/images/products/product-4.jpg" },
                    { 13, "mini-dash", 0, "assets/images/products/product-5.jpg" },
                    { 14, "mini-dash", 1, "assets/images/products/product-5.jpg" },
                    { 15, "mini-dash", 2, "assets/images/products/product-5.jpg" },
                    { 16, "overtime-06", 0, "assets/images/products/product-6.jpg" },
                    { 17, "overtime-06", 1, "assets/images/products/product-6.jpg" },
                    { 18, "overtime-06", 2, "assets/images/products/product-6.jpg" }
                });

            migrationBuilder.InsertData(
                table: "ProductSizes",
                columns: new[] { "Id", "ProductId", "Size", "StockQty" },
                values: new object[,]
                {
                    { 1, "air-pulse", 40, 12 },
                    { 2, "air-pulse", 41, 14 },
                    { 3, "air-pulse", 42, 0 },
                    { 4, "air-pulse", 43, 6 },
                    { 5, "air-pulse", 44, 8 },
                    { 6, "air-pulse", 45, 10 },
                    { 7, "highjump-x", 41, 14 },
                    { 8, "highjump-x", 42, 4 },
                    { 9, "highjump-x", 43, 6 },
                    { 10, "highjump-x", 44, 8 },
                    { 11, "highjump-x", 45, 10 },
                    { 12, "highjump-x", 46, 12 },
                    { 13, "street-low", 38, 8 },
                    { 14, "street-low", 39, 10 },
                    { 15, "street-low", 40, 12 },
                    { 16, "street-low", 41, 14 },
                    { 17, "street-low", 42, 4 },
                    { 18, "street-low", 43, 6 },
                    { 19, "street-low", 44, 8 },
                    { 20, "aria-knit", 36, 4 },
                    { 21, "aria-knit", 37, 6 },
                    { 22, "aria-knit", 38, 8 },
                    { 23, "aria-knit", 39, 10 },
                    { 24, "aria-knit", 40, 12 },
                    { 25, "mini-dash", 28, 12 },
                    { 26, "mini-dash", 29, 14 },
                    { 27, "mini-dash", 30, 4 },
                    { 28, "mini-dash", 31, 6 },
                    { 29, "mini-dash", 32, 8 },
                    { 30, "mini-dash", 33, 10 },
                    { 31, "mini-dash", 34, 12 },
                    { 32, "overtime-06", 40, 12 },
                    { 33, "overtime-06", 41, 14 },
                    { 34, "overtime-06", 42, 4 },
                    { 35, "overtime-06", 43, 6 },
                    { 36, "overtime-06", 44, 8 }
                });

            migrationBuilder.InsertData(
                table: "ProductSpecs",
                columns: new[] { "Id", "Key", "ProductId", "SortOrder", "Value" },
                values: new object[,]
                {
                    { 1, "جنس رویه", "air-pulse", 0, "مش تنفس‌پذیر" },
                    { 2, "نوع بند", "air-pulse", 1, "بندی معمولی" },
                    { 3, "کاربرد", "air-pulse", 2, "دویدن و پیاده‌روی" },
                    { 4, "رنگ اصلی", "air-pulse", 3, "بنفش" },
                    { 5, "جنس رویه", "highjump-x", 0, "سینتتیک و مش" },
                    { 6, "نوع بند", "highjump-x", 1, "بندی بلند" },
                    { 7, "کاربرد", "highjump-x", 2, "بسکتبال" },
                    { 8, "رنگ اصلی", "highjump-x", 3, "بنفش تیره" },
                    { 9, "جنس رویه", "street-low", 0, "چرم مصنوعی" },
                    { 10, "نوع بند", "street-low", 1, "بندی معمولی" },
                    { 11, "کاربرد", "street-low", 2, "استفاده روزمره" },
                    { 12, "رنگ اصلی", "street-low", 3, "مشکی" },
                    { 13, "جنس رویه", "aria-knit", 0, "نیت کشباف" },
                    { 14, "نوع بند", "aria-knit", 1, "بندی کشی" },
                    { 15, "کاربرد", "aria-knit", 2, "استفاده روزمره" },
                    { 16, "رنگ اصلی", "aria-knit", 3, "بنفش تیره" },
                    { 17, "جنس رویه", "mini-dash", 0, "مش نرم" },
                    { 18, "نوع بند", "mini-dash", 1, "بست چسبی" },
                    { 19, "کاربرد", "mini-dash", 2, "استفاده روزمره" },
                    { 20, "رنگ اصلی", "mini-dash", 3, "بنفش" },
                    { 21, "جنس رویه", "overtime-06", 0, "چرم و سوئد" },
                    { 22, "نوع بند", "overtime-06", 1, "بندی معمولی" },
                    { 23, "کاربرد", "overtime-06", 2, "کالکشنی و روزمره" },
                    { 24, "رنگ اصلی", "overtime-06", 3, "مشکی و بنفش" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId_IsDefault",
                table: "Addresses",
                columns: new[] { "UserId", "IsDefault" });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUsers_Username",
                table: "AdminUsers",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId_ProductId_Size",
                table: "CartItems",
                columns: new[] { "CartId", "ProductId", "Size" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressId",
                table: "Orders",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OtpCodes_PhoneNumber_CreatedAt",
                table: "OtpCodes",
                columns: new[] { "PhoneNumber", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizes_ProductId_Size",
                table: "ProductSizes",
                columns: new[] { "ProductId", "Size" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecs_ProductId",
                table: "ProductSpecs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "OtpCodes");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductSizes");

            migrationBuilder.DropTable(
                name: "ProductSpecs");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
