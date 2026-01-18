using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastFood.Migrations
{
    /// <inheritdoc />
    public partial class hihihaha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Combos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Foods_FoodCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "FoodCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComboItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComboId = table.Column<int>(type: "int", nullable: false),
                    FoodId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComboItems_Combos_ComboId",
                        column: x => x.ComboId,
                        principalTable: "Combos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboItems_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    FoodId = table.Column<int>(type: "int", nullable: true),
                    ComboId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Combos_ComboId",
                        column: x => x.ComboId,
                        principalTable: "Combos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderDetails_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Combos",
                columns: new[] { "Id", "Description", "ImageUrl", "IsActive", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Burger bò phô mai, khoai tây chiên và Coca Cola", "https://burgerking.vn/media/catalog/product/cache/1/image/1800x/040ec09b1e35df139433887a97daa66f/1/7/17-whopper-b_-t_m-ph_-mai-c_-l_n.jpg", true, "Combo Burger Bò Đặc Biệt", 95000m },
                    { 2, "4 miếng gà rán, khoai tây chiên lớn và 2 Pepsi", "https://images.unsplash.com/photo-1562967914-608f82629710?w=500", true, "Combo Gà Rán Gia Đình", 180000m },
                    { 3, "Burger gà giòn, khoai tây chiên và nước cam ép", "https://images.unsplash.com/photo-1603064752734-4c48eff53d05?w=500", true, "Combo Burger Gà Tiết Kiệm", 100000m },
                    { 4, "Burger bò nhỏ, cánh gà chiên và trà sữa", "https://images.unsplash.com/photo-1565299715199-866c917206bb?w=500", true, "Combo Học Sinh", 120000m },
                    { 5, "Kem vani, bánh táo và trà sữa trân châu", "https://images.unsplash.com/photo-1551024601-bec78aea704b?w=500", true, "Combo Tráng Miệng Ngọt Ngào", 85000m },
                    { 6, "Burger bò bacon, khoai tây chiên và cà phê sữa", "https://images.unsplash.com/photo-1572802419224-296b0aeee0d9?w=500", true, "Combo Burger Bacon Đặc Biệt", 128000m },
                    { 7, "Burger gà cay Hàn Quốc, cánh gà buffalo và trà sữa matcha", "https://images.unsplash.com/photo-1594212699903-ec8a3eca50f5?w=500", true, "Combo Gà Cay Hàn Quốc", 145000m },
                    { 8, "Salad caesar và sinh tố bơ", "https://images.unsplash.com/photo-1546793665-c74683f339c1?w=500", true, "Combo Healthy", 115000m },
                    { 9, "Gà nuggets, khoai tây chiên và nước cam ép", "https://images.unsplash.com/photo-1562967914-608f82629710?w=500", true, "Combo Trẻ Em Vui Vẻ", 98000m },
                    { 10, "Burger phô mai nướng, hành tây chiên giòn và soda chanh dây", "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=500", true, "Combo Tiệc Nướng", 110000m },
                    { 11, "Burger gà giòn, bánh donut socola và cà phê đen", "https://images.unsplash.com/photo-1606755962773-d324e0a13086?w=500", true, "Combo Cà Phê Sáng", 95000m },
                    { 12, "Burger tôm, gỏi cuốn tôm", "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=500", true, "Combo Hải Sản Cao Cấp", 135000m },
                    { 13, "Gà rán phô mai, phô mai que chiên và trà sữa", "https://images.unsplash.com/photo-1626082927389-6cd097cdc6ec?w=500", true, "Combo Phô Mai Đặc Biệt", 130000m },
                    { 14, "Tiramisu, cheesecake dâu và trà đào cam sả", "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?w=500", true, "Combo Tráng Miệng Cao Cấp", 125000m },
                    { 15, "2 burger bò phô mai, 2 khoai tây chiên và 2 pepsi", "https://images.unsplash.com/photo-1553979459-d2229ba7433b?w=500", true, "Combo Bạn Bè", 225000m }
                });

            migrationBuilder.InsertData(
                table: "FoodCategories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Các loại burger bò, gà, cá", "Burger" },
                    { 2, "Các món gà rán giòn, cay", "Gà rán" },
                    { 3, "Nước ngọt, trà sữa, nước ép", "Đồ uống" },
                    { 4, "Khoai tây, salad, snack", "Món phụ" },
                    { 5, "Kem, bánh ngọt", "Tráng miệng" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Quản trị hệ thống", "Admin" },
                    { 2, "Khách hàng", "Customer" }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Address", "DateOfBirth", "Email", "FullName", "Password", "PhoneNumber", "RoleId" },
                values: new object[,]
                {
                    { 1, "Hà Nội", null, "admin@fastfood.local", "Admin", "123456", "0900000000", 1 },
                    { 2, "TP. Hồ Chí Minh", null, "customer@fastfood.local", "Nguyễn Văn A", "123456", "0911111111", 2 }
                });

            migrationBuilder.InsertData(
                table: "Foods",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "IsActive", "Name", "Price", "Tag" },
                values: new object[,]
                {
                    { 1, 1, "Burger bò 100% thịt thật với phô mai cheddar tan chảy, rau xà lách tươi và sốt đặc biệt", "https://burgerking.vn/media/catalog/product/cache/1/image/1800x/040ec09b1e35df139433887a97daa66f/1/7/17-whopper-b_-t_m-ph_-mai-c_-l_n.jpg", true, "Burger Bò Phô Mai", 55000m, "bò, phô mai, burger" },
                    { 2, 1, "Burger gà rán giòn với sốt mayonnaise và rau tươi", "https://images.unsplash.com/photo-1606755962773-d324e0a13086?w=500", true, "Burger Gà Giòn", 50000m, "gà, burger, giòn" },
                    { 4, 2, "Miếng gà rán giòn rụm với công thức bí mật 11 loại gia vị", "https://images.unsplash.com/photo-1626082927389-6cd097cdc6ec?w=500", true, "Gà Rán Giòn Truyền Thống", 45000m, "gà, rán, giòn" },
                    { 5, 2, "Gà rán cay nồng với ớt và tiêu đen", "https://images.unsplash.com/photo-1594221708779-94832f4320d1?w=500", true, "Gà Rán Cay", 47000m, "gà, cay, rán" },
                    { 6, 2, "Cánh gà chiên giòn sốt nước mắm đậm đà", "https://images.unsplash.com/photo-1567620832903-9fc6debc209f?w=500", true, "Cánh Gà Chiên Nước Mắm", 40000m, "gà, cánh, chiên" },
                    { 7, 3, "Nước ngọt có gas Coca Cola classic", "https://images.unsplash.com/photo-1554866585-cd94860890b7?w=500", true, "Coca Cola", 20000m, "nước ngọt, coca, gas" },
                    { 8, 3, "Nước ngọt có gas Pepsi", "https://images.unsplash.com/photo-1629203851122-3726ecdf080e?w=500", true, "Pepsi", 20000m, "nước ngọt, pepsi, gas" },
                    { 9, 3, "Trà sữa ngọt ngào với trân châu đen dai", "https://images.unsplash.com/photo-1525385133512-2f3bdd039054?w=500", true, "Trà Sữa Trân Châu", 35000m, "trà sữa, trân châu, ngọt" },
                    { 10, 3, "Nước cam tươi ép 100%", "https://images.unsplash.com/photo-1600271886742-f049cd451bba?w=500", true, "Nước Cam Ép", 30000m, "nước ép, cam, tươi" },
                    { 11, 4, "Khoai tây chiên giòn vàng ươm", "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?w=500", true, "Khoai Tây Chiên", 30000m, "khoai tây, chiên, snack" },
                    { 12, 4, "Salad rau củ tươi với sốt mayonnaise", "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?w=500", true, "Salad Rau Củ", 35000m, "salad, rau, healthy" },
                    { 13, 5, "Kem vani béo ngậy mát lạnh", "https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=500", true, "Kem Vani", 25000m, "kem, vani, ngọt" },
                    { 14, 5, "Bánh táo nướng thơm giòn", "https://images.unsplash.com/photo-1562007908-17c67e878c88?w=500", true, "Bánh Táo", 28000m, "bánh, táo, nướng" },
                    { 15, 1, "Burger tôm tươi giòn tan với sốt cocktail đặc biệt", "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=500", true, "Burger Tôm", 65000m, "tôm, burger, hải sản" },
                    { 16, 1, "Burger gà với sốt teriyaki Nhật Bản, dứa nướng", "https://images.unsplash.com/photo-1550547660-d9450f859349?w=500", true, "Burger Gà Teriyaki", 58000m, "gà, teriyaki, burger" },
                    { 17, 1, "Hai lớp thịt bò 100% với phô mai cheddar tan chảy", "https://images.unsplash.com/photo-1553979459-d2229ba7433b?w=500", true, "Burger Bò Phô Mai Đôi", 75000m, "bò, phô mai, burger, double" },
                    { 18, 1, "Burger chay từ đậu và rau củ, phù hợp người ăn chay", "https://images.unsplash.com/photo-1520072959219-c595dc870360?w=500", true, "Burger Chay", 52000m, "chay, burger, healthy" },
                    { 19, 2, "Gà rán giòn phủ sốt BBQ ngọt cay đậm đà", "https://images.unsplash.com/photo-1598103442097-8b74394b95c6?w=500", true, "Gà Rán Sốt BBQ", 48000m, "gà, bbq, rán" },
                    { 21, 2, "Phi lê gà rán không xương, dễ ăn", "https://images.unsplash.com/photo-1562967914-608f82629710?w=500", true, "Gà Phi Lê", 46000m, "gà, phi lê, rán" },
                    { 22, 3, "Nước ngọt có gas Sprite chanh mát lạnh", "https://images.unsplash.com/photo-1625772452859-1c03d5bf1137?w=500", true, "Sprite", 20000m, "nước ngọt, sprite, gas" },
                    { 23, 3, "Trà đào kết hợp cam sả thơm mát", "https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=500", true, "Trà Đào Cam Sả", 38000m, "trà, đào, cam sả" },
                    { 24, 3, "Sinh tố bơ béo ngậy, bổ dưỡng", "https://images.unsplash.com/photo-1623065422902-30a2d299bbe4?w=500", true, "Sinh Tố Bơ", 40000m, "sinh tố, bơ, healthy" },
                    { 25, 3, "Cà phê đen đá truyền thống Việt Nam", "https://images.unsplash.com/photo-1514432324607-a09d9b4aefdd?w=500", true, "Cà Phê Đen", 25000m, "cà phê, đen, việt nam" },
                    { 26, 3, "Cà phê sữa đá ngọt ngào", "https://images.unsplash.com/photo-1461023058943-07fcbe16d735?w=500", true, "Cà Phê Sữa", 28000m, "cà phê, sữa, việt nam" },
                    { 29, 4, "Gỏi cuốn tôm tươi với rau sống và bún", "https://images.unsplash.com/photo-1559314809-0d155014e29e?w=500", true, "Gỏi Cuốn Tôm", 32000m, "gỏi cuốn, tôm, healthy" },
                    { 30, 4, "Salad caesar với gà nướng và sốt đặc biệt", "https://images.unsplash.com/photo-1546793665-c74683f339c1?w=500", true, "Salad Caesar", 42000m, "salad, caesar, gà" },
                    { 31, 5, "Kem chocolate đậm đà, ngọt ngào", "https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=500", true, "Kem Chocolate", 25000m, "kem, chocolate, ngọt" },
                    { 32, 5, "Kem dâu tươi mát lạnh", "https://images.unsplash.com/photo-1501443762994-82bd5dace89a?w=500", true, "Kem Dâu", 25000m, "kem, dâu, ngọt" },
                    { 33, 5, "Bánh flan caramel mềm mịn", "https://images.unsplash.com/photo-1587486913049-53fc88980cfc?w=500", true, "Bánh Flan", 22000m, "flan, caramel, ngọt" },
                    { 34, 5, "Brownie chocolate đắng ngọt, nóng giòn", "https://images.unsplash.com/photo-1607920591413-4ec007e70023?w=500", true, "Brownie Chocolate", 32000m, "brownie, chocolate, bánh" },
                    { 35, 5, "Kem vani phủ sốt caramel và hạt", "https://images.unsplash.com/photo-1563729784474-d77dbb933a9e?w=500", true, "Sundae Caramel", 30000m, "kem, sundae, caramel" },
                    { 36, 1, "Burger bò với bacon giòn, phô mai và sốt BBQ", "https://images.unsplash.com/photo-1572802419224-296b0aeee0d9?w=500", true, "Burger Bò Bacon", 68000m, "bò, bacon, burger" },
                    { 37, 1, "Burger gà rán cay kiểu Hàn với sốt gochujang", "https://images.unsplash.com/photo-1594212699903-ec8a3eca50f5?w=500", true, "Burger Gà Cay Hàn Quốc", 62000m, "gà, cay, hàn quốc" },
                    { 38, 1, "Burger phô mai nướng giòn tan với nhiều loại phô mai", "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=500", true, "Burger Phô Mai Nướng", 55000m, "phô mai, burger, nướng" },
                    { 39, 2, "Gà rán phủ lớp phô mai tan chảy bên trong", "https://images.unsplash.com/photo-1626082927389-6cd097cdc6ec?w=500", true, "Gà Rán Phô Mai", 52000m, "gà, phô mai, rán" },
                    { 40, 2, "Cánh gà cay kiểu Mỹ với sốt buffalo", "https://images.unsplash.com/photo-1608039829572-78524f79c4c7?w=500", true, "Cánh Gà Buffalo", 45000m, "gà, cánh, buffalo" },
                    { 41, 2, "Gà viên chiên giòn dạng nuggets cho trẻ em", "https://images.unsplash.com/photo-1562967914-608f82629710?w=500", true, "Gà Nuggets", 38000m, "gà, nuggets, trẻ em" },
                    { 42, 3, "Trà sữa matcha Nhật Bản với trân châu trắng", "https://images.unsplash.com/photo-1515823064-d6e0c04616a7?w=500", true, "Trà Sữa Matcha", 42000m, "trà sữa, matcha, nhật bản" },
                    { 43, 3, "Sinh tố xoài tươi ngọt mát", "https://images.unsplash.com/photo-1546173159-315724a31696?w=500", true, "Sinh Tố Xoài", 38000m, "sinh tố, xoài, tươi" },
                    { 44, 3, "Soda chanh dây tươi mát giải nhiệt", "https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=500", true, "Soda Chanh Dây", 32000m, "soda, chanh dây, tươi" },
                    { 45, 3, "Trà chanh tươi mát lạnh giải khát", "https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=500", true, "Trà Chanh", 28000m, "trà, chanh, giải khát" },
                    { 48, 4, "Hành tây tẩm bột chiên giòn giòn với sốt", "https://images.unsplash.com/photo-1639024471283-03518883512d?w=500", true, "Hành Tây Chiên Giòn", 35000m, "hành tây, chiên, snack" },
                    { 49, 4, "Phô mai que tẩm bột chiên giòn tan", "https://images.unsplash.com/photo-1548340748-6d2b7d7da280?w=500", true, "Phô Mai Que Chiên", 40000m, "phô mai, chiên, que" },
                    { 50, 4, "Salad rau củ tươi trộn dầu giấm balsamic", "https://images.unsplash.com/photo-1540189549336-e6e99c3679fe?w=500", true, "Salad Trộn Dầu Giấm", 38000m, "salad, rau, healthy" },
                    { 51, 5, "Bánh donut phủ socola và rắc hạt", "https://images.unsplash.com/photo-1551024601-bec78aea704b?w=500", true, "Bánh Donut Socola", 26000m, "donut, socola, bánh" },
                    { 52, 5, "Tiramisu Ý truyền thống với cà phê và mascarpone", "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?w=500", true, "Tiramisu", 45000m, "tiramisu, cà phê, ý" },
                    { 53, 5, "Kem matcha Nhật Bản đắng ngọt hài hòa", "https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=500", true, "Kem Matcha", 28000m, "kem, matcha, nhật bản" },
                    { 54, 5, "Pudding xoài mềm mịn béo ngậy", "https://images.unsplash.com/photo-1488477181946-6428a0291777?w=500", true, "Pudding Xoài", 30000m, "pudding, xoài, ngọt" }
                });

            migrationBuilder.InsertData(
                table: "ComboItems",
                columns: new[] { "Id", "ComboId", "FoodId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 1, 11, 1 },
                    { 3, 1, 7, 1 },
                    { 4, 2, 4, 4 },
                    { 5, 2, 11, 2 },
                    { 6, 2, 8, 2 },
                    { 7, 3, 2, 1 },
                    { 8, 3, 11, 1 },
                    { 9, 3, 10, 1 },
                    { 10, 4, 1, 1 },
                    { 11, 4, 6, 2 },
                    { 12, 4, 9, 1 },
                    { 13, 5, 13, 1 },
                    { 14, 5, 14, 1 },
                    { 15, 5, 9, 1 },
                    { 16, 6, 36, 1 },
                    { 17, 6, 11, 1 },
                    { 18, 6, 26, 1 },
                    { 19, 7, 37, 1 },
                    { 20, 7, 40, 2 },
                    { 21, 7, 42, 1 },
                    { 22, 8, 30, 1 },
                    { 24, 8, 24, 1 },
                    { 25, 9, 41, 1 },
                    { 26, 9, 11, 1 },
                    { 27, 9, 10, 1 },
                    { 28, 10, 38, 1 },
                    { 29, 10, 48, 1 },
                    { 30, 10, 44, 1 },
                    { 31, 11, 2, 1 },
                    { 32, 11, 51, 1 },
                    { 33, 11, 25, 1 },
                    { 34, 12, 15, 1 },
                    { 35, 12, 29, 2 },
                    { 37, 13, 39, 2 },
                    { 38, 13, 49, 1 },
                    { 39, 13, 9, 1 },
                    { 40, 14, 52, 1 },
                    { 42, 14, 23, 1 },
                    { 43, 15, 1, 2 },
                    { 45, 15, 11, 2 },
                    { 46, 15, 8, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_RoleId",
                table: "Accounts",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboItems_ComboId",
                table: "ComboItems",
                column: "ComboId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboItems_FoodId",
                table: "ComboItems",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_CategoryId",
                table: "Foods",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ComboId",
                table: "OrderDetails",
                column: "ComboId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_FoodId",
                table: "OrderDetails",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AccountId",
                table: "Orders",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComboItems");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Combos");

            migrationBuilder.DropTable(
                name: "Foods");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "FoodCategories");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
