using FastFoodOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace FastFoodOnline.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<FoodCategory> FoodCategories => Set<FoodCategory>();
        public DbSet<Food> Foods => Set<Food>();
        public DbSet<Combo> Combos => Set<Combo>();
        public DbSet<ComboItem> ComboItems => Set<ComboItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API configuration
            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Role)
                .WithMany(r => r.Accounts)
                .HasForeignKey(a => a.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Food>()
                .Property(f => f.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Combo>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.UnitPrice)
                .HasColumnType("decimal(18,2)");

            // Relationships for combo items
            modelBuilder.Entity<ComboItem>()
                .HasOne(ci => ci.Combo)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.ComboId);

            modelBuilder.Entity<ComboItem>()
                .HasOne(ci => ci.Food)
                .WithMany(f => f.ComboItems)
                .HasForeignKey(ci => ci.FoodId);

            // Relationships for order details
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(od => od.OrderId);

            // Seed data
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", Description = "Quản trị hệ thống" },
                new Role { Id = 2, Name = "Customer", Description = "Khách hàng" }
            );

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = 1,
                    FullName = "Admin",
                    Email = "admin@fastfood.local",
                    Password = "123456",
                    PhoneNumber = "0900000000",
                    Address = "Hà Nội",
                    RoleId = 1
                },
                new Account
                {
                    Id = 2,
                    FullName = "Nguyễn Văn A",
                    Email = "customer@fastfood.local",
                    Password = "123456",
                    PhoneNumber = "0911111111",
                    Address = "TP. Hồ Chí Minh",
                    RoleId = 2
                }
            );

            modelBuilder.Entity<FoodCategory>().HasData(
                new FoodCategory { Id = 1, Name = "Burger", Description = "Các loại burger bò, gà, cá" },
                new FoodCategory { Id = 2, Name = "Gà rán", Description = "Các món gà rán giòn, cay" },
                new FoodCategory { Id = 3, Name = "Đồ uống", Description = "Nước ngọt, trà sữa, nước ép" },
                new FoodCategory { Id = 4, Name = "Món phụ", Description = "Khoai tây, salad, snack" },
                new FoodCategory { Id = 5, Name = "Tráng miệng", Description = "Kem, bánh ngọt" }
            );

            modelBuilder.Entity<Food>().HasData(
                // Burgers
                new Food
                {
                    Id = 1,
                    Name = "Burger Bò Phô Mai",
                    Description = "Burger bò 100% thịt thật với phô mai cheddar tan chảy, rau xà lách tươi và sốt đặc biệt",
                    Price = 55000,
                    ImageUrl = "https://burgerking.vn/media/catalog/product/cache/1/image/1800x/040ec09b1e35df139433887a97daa66f/1/7/17-whopper-b_-t_m-ph_-mai-c_-l_n.jpg",
                    CategoryId = 1,
                    Tag = "bò, phô mai, burger",
                    IsActive = true
                },
                new Food
                {
                    Id = 2,
                    Name = "Burger Gà Giòn",
                    Description = "Burger gà rán giòn với sốt mayonnaise và rau tươi",
                    Price = 50000,
                    ImageUrl = "https://images.unsplash.com/photo-1606755962773-d324e0a13086?w=500",
                    CategoryId = 1,
                    Tag = "gà, burger, giòn",
                    IsActive = true
                },
                // Gà rán
                new Food
                {
                    Id = 4,
                    Name = "Gà Rán Giòn Truyền Thống",
                    Description = "Miếng gà rán giòn rụm với công thức bí mật 11 loại gia vị",
                    Price = 45000,
                    ImageUrl = "https://images.unsplash.com/photo-1626082927389-6cd097cdc6ec?w=500",
                    CategoryId = 2,
                    Tag = "gà, rán, giòn",
                    IsActive = true
                },
                new Food
                {
                    Id = 5,
                    Name = "Gà Rán Cay",
                    Description = "Gà rán cay nồng với ớt và tiêu đen",
                    Price = 47000,
                    ImageUrl = "https://images.unsplash.com/photo-1594221708779-94832f4320d1?w=500",
                    CategoryId = 2,
                    Tag = "gà, cay, rán",
                    IsActive = true
                },
                new Food
                {
                    Id = 6,
                    Name = "Cánh Gà Chiên Nước Mắm",
                    Description = "Cánh gà chiên giòn sốt nước mắm đậm đà",
                    Price = 40000,
                    ImageUrl = "https://images.unsplash.com/photo-1567620832903-9fc6debc209f?w=500",
                    CategoryId = 2,
                    Tag = "gà, cánh, chiên",
                    IsActive = true
                },

                // Đồ uống
                new Food
                {
                    Id = 7,
                    Name = "Coca Cola",
                    Description = "Nước ngọt có gas Coca Cola classic",
                    Price = 20000,
                    ImageUrl = "https://images.unsplash.com/photo-1554866585-cd94860890b7?w=500",
                    CategoryId = 3,
                    Tag = "nước ngọt, coca, gas",
                    IsActive = true
                },
                new Food
                {
                    Id = 8,
                    Name = "Pepsi",
                    Description = "Nước ngọt có gas Pepsi",
                    Price = 20000,
                    ImageUrl = "https://images.unsplash.com/photo-1629203851122-3726ecdf080e?w=500",
                    CategoryId = 3,
                    Tag = "nước ngọt, pepsi, gas",
                    IsActive = true
                },
                new Food
                {
                    Id = 9,
                    Name = "Trà Sữa Trân Châu",
                    Description = "Trà sữa ngọt ngào với trân châu đen dai",
                    Price = 35000,
                    ImageUrl = "https://images.unsplash.com/photo-1525385133512-2f3bdd039054?w=500",
                    CategoryId = 3,
                    Tag = "trà sữa, trân châu, ngọt",
                    IsActive = true
                },
                new Food
                {
                    Id = 10,
                    Name = "Nước Cam Ép",
                    Description = "Nước cam tươi ép 100%",
                    Price = 30000,
                    ImageUrl = "https://images.unsplash.com/photo-1600271886742-f049cd451bba?w=500",
                    CategoryId = 3,
                    Tag = "nước ép, cam, tươi",
                    IsActive = true
                },

                // Món phụ
                new Food
                {
                    Id = 11,
                    Name = "Khoai Tây Chiên",
                    Description = "Khoai tây chiên giòn vàng ươm",
                    Price = 30000,
                    ImageUrl = "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?w=500",
                    CategoryId = 4,
                    Tag = "khoai tây, chiên, snack",
                    IsActive = true
                },
                new Food
                {
                    Id = 12,
                    Name = "Salad Rau Củ",
                    Description = "Salad rau củ tươi với sốt mayonnaise",
                    Price = 35000,
                    ImageUrl = "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?w=500",
                    CategoryId = 4,
                    Tag = "salad, rau, healthy",
                    IsActive = true
                },

                // Tráng miệng
                new Food
                {
                    Id = 13,
                    Name = "Kem Vani",
                    Description = "Kem vani béo ngậy mát lạnh",
                    Price = 25000,
                    ImageUrl = "https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=500",
                    CategoryId = 5,
                    Tag = "kem, vani, ngọt",
                    IsActive = true
                },
                new Food
                {
                    Id = 14,
                    Name = "Bánh Táo",
                    Description = "Bánh táo nướng thơm giòn",
                    Price = 28000,
                    ImageUrl = "https://images.unsplash.com/photo-1562007908-17c67e878c88?w=500",
                    CategoryId = 5,
                    Tag = "bánh, táo, nướng",
                    IsActive = true
                },

                // Thêm Burgers
                new Food
                {
                    Id = 15,
                    Name = "Burger Tôm",
                    Description = "Burger tôm tươi giòn tan với sốt cocktail đặc biệt",
                    Price = 65000,
                    ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=500",
                    CategoryId = 1,
                    Tag = "tôm, burger, hải sản",
                    IsActive = true
                },
                new Food
                {
                    Id = 16,
                    Name = "Burger Gà Teriyaki",
                    Description = "Burger gà với sốt teriyaki Nhật Bản, dứa nướng",
                    Price = 58000,
                    ImageUrl = "https://images.unsplash.com/photo-1550547660-d9450f859349?w=500",
                    CategoryId = 1,
                    Tag = "gà, teriyaki, burger",
                    IsActive = true
                },
                new Food
                {
                    Id = 17,
                    Name = "Burger Bò Phô Mai Đôi",
                    Description = "Hai lớp thịt bò 100% với phô mai cheddar tan chảy",
                    Price = 75000,
                    ImageUrl = "https://images.unsplash.com/photo-1553979459-d2229ba7433b?w=500",
                    CategoryId = 1,
                    Tag = "bò, phô mai, burger, double",
                    IsActive = true
                },
                new Food
                {
                    Id = 18,
                    Name = "Burger Chay",
                    Description = "Burger chay từ đậu và rau củ, phù hợp người ăn chay",
                    Price = 52000,
                    ImageUrl = "https://images.unsplash.com/photo-1520072959219-c595dc870360?w=500",
                    CategoryId = 1,
                    Tag = "chay, burger, healthy",
                    IsActive = true
                },

                // Thêm Gà rán
                new Food
                {
                    Id = 19,
                    Name = "Gà Rán Sốt BBQ",
                    Description = "Gà rán giòn phủ sốt BBQ ngọt cay đậm đà",
                    Price = 48000,
                    ImageUrl = "https://images.unsplash.com/photo-1598103442097-8b74394b95c6?w=500",
                    CategoryId = 2,
                    Tag = "gà, bbq, rán",
                    IsActive = true
                },
                new Food
                {
                    Id = 21,
                    Name = "Gà Phi Lê",
                    Description = "Phi lê gà rán không xương, dễ ăn",
                    Price = 46000,
                    ImageUrl = "https://images.unsplash.com/photo-1562967914-608f82629710?w=500",
                    CategoryId = 2,
                    Tag = "gà, phi lê, rán",
                    IsActive = true
                },

                // Thêm Đồ uống
                new Food
                {
                    Id = 22,
                    Name = "Sprite",
                    Description = "Nước ngọt có gas Sprite chanh mát lạnh",
                    Price = 20000,
                    ImageUrl = "https://images.unsplash.com/photo-1625772452859-1c03d5bf1137?w=500",
                    CategoryId = 3,
                    Tag = "nước ngọt, sprite, gas",
                    IsActive = true
                },
                new Food
                {
                    Id = 23,
                    Name = "Trà Đào Cam Sả",
                    Description = "Trà đào kết hợp cam sả thơm mát",
                    Price = 38000,
                    ImageUrl = "https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=500",
                    CategoryId = 3,
                    Tag = "trà, đào, cam sả",
                    IsActive = true
                },
                new Food
                {
                    Id = 24,
                    Name = "Sinh Tố Bơ",
                    Description = "Sinh tố bơ béo ngậy, bổ dưỡng",
                    Price = 40000,
                    ImageUrl = "https://images.unsplash.com/photo-1623065422902-30a2d299bbe4?w=500",
                    CategoryId = 3,
                    Tag = "sinh tố, bơ, healthy",
                    IsActive = true
                },
                new Food
                {
                    Id = 25,
                    Name = "Cà Phê Đen",
                    Description = "Cà phê đen đá truyền thống Việt Nam",
                    Price = 25000,
                    ImageUrl = "https://images.unsplash.com/photo-1514432324607-a09d9b4aefdd?w=500",
                    CategoryId = 3,
                    Tag = "cà phê, đen, việt nam",
                    IsActive = true
                },
                new Food
                {
                    Id = 26,
                    Name = "Cà Phê Sữa",
                    Description = "Cà phê sữa đá ngọt ngào",
                    Price = 28000,
                    ImageUrl = "https://images.unsplash.com/photo-1461023058943-07fcbe16d735?w=500",
                    CategoryId = 3,
                    Tag = "cà phê, sữa, việt nam",
                    IsActive = true
                },

                // Thêm Món phụ
                new Food
                {
                    Id = 29,
                    Name = "Gỏi Cuốn Tôm",
                    Description = "Gỏi cuốn tôm tươi với rau sống và bún",
                    Price = 32000,
                    ImageUrl = "https://images.unsplash.com/photo-1559314809-0d155014e29e?w=500",
                    CategoryId = 4,
                    Tag = "gỏi cuốn, tôm, healthy",
                    IsActive = true
                },
                new Food
                {
                    Id = 30,
                    Name = "Salad Caesar",
                    Description = "Salad caesar với gà nướng và sốt đặc biệt",
                    Price = 42000,
                    ImageUrl = "https://images.unsplash.com/photo-1546793665-c74683f339c1?w=500",
                    CategoryId = 4,
                    Tag = "salad, caesar, gà",
                    IsActive = true
                },

                // Thêm Tráng miệng
                new Food
                {
                    Id = 31,
                    Name = "Kem Chocolate",
                    Description = "Kem chocolate đậm đà, ngọt ngào",
                    Price = 25000,
                    ImageUrl = "https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=500",
                    CategoryId = 5,
                    Tag = "kem, chocolate, ngọt",
                    IsActive = true
                },
                new Food
                {
                    Id = 32,
                    Name = "Kem Dâu",
                    Description = "Kem dâu tươi mát lạnh",
                    Price = 25000,
                    ImageUrl = "https://images.unsplash.com/photo-1501443762994-82bd5dace89a?w=500",
                    CategoryId = 5,
                    Tag = "kem, dâu, ngọt",
                    IsActive = true
                },
                new Food
                {
                    Id = 33,
                    Name = "Bánh Flan",
                    Description = "Bánh flan caramel mềm mịn",
                    Price = 22000,
                    ImageUrl = "https://images.unsplash.com/photo-1587486913049-53fc88980cfc?w=500",
                    CategoryId = 5,
                    Tag = "flan, caramel, ngọt",
                    IsActive = true
                },
                new Food
                {
                    Id = 34,
                    Name = "Brownie Chocolate",
                    Description = "Brownie chocolate đắng ngọt, nóng giòn",
                    Price = 32000,
                    ImageUrl = "https://images.unsplash.com/photo-1607920591413-4ec007e70023?w=500",
                    CategoryId = 5,
                    Tag = "brownie, chocolate, bánh",
                    IsActive = true
                },
                new Food
                {
                    Id = 35,
                    Name = "Sundae Caramel",
                    Description = "Kem vani phủ sốt caramel và hạt",
                    Price = 30000,
                    ImageUrl = "https://images.unsplash.com/photo-1563729784474-d77dbb933a9e?w=500",
                    CategoryId = 5,
                    Tag = "kem, sundae, caramel",
                    IsActive = true
                },

                // Thêm Burgers mới
                new Food
                {
                    Id = 36,
                    Name = "Burger Bò Bacon",
                    Description = "Burger bò với bacon giòn, phô mai và sốt BBQ",
                    Price = 68000,
                    ImageUrl = "https://images.unsplash.com/photo-1572802419224-296b0aeee0d9?w=500",
                    CategoryId = 1,
                    Tag = "bò, bacon, burger",
                    IsActive = true
                },
                new Food
                {
                    Id = 37,
                    Name = "Burger Gà Cay Hàn Quốc",
                    Description = "Burger gà rán cay kiểu Hàn với sốt gochujang",
                    Price = 62000,
                    ImageUrl = "https://images.unsplash.com/photo-1594212699903-ec8a3eca50f5?w=500",
                    CategoryId = 1,
                    Tag = "gà, cay, hàn quốc",
                    IsActive = true
                },
                new Food
                {
                    Id = 38,
                    Name = "Burger Phô Mai Nướng",
                    Description = "Burger phô mai nướng giòn tan với nhiều loại phô mai",
                    Price = 55000,
                    ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=500",
                    CategoryId = 1,
                    Tag = "phô mai, burger, nướng",
                    IsActive = true
                },

                // Thêm Gà rán mới
                new Food
                {
                    Id = 39,
                    Name = "Gà Rán Phô Mai",
                    Description = "Gà rán phủ lớp phô mai tan chảy bên trong",
                    Price = 52000,
                    ImageUrl = "https://images.unsplash.com/photo-1626082927389-6cd097cdc6ec?w=500",
                    CategoryId = 2,
                    Tag = "gà, phô mai, rán",
                    IsActive = true
                },
                new Food
                {
                    Id = 40,
                    Name = "Cánh Gà Buffalo",
                    Description = "Cánh gà cay kiểu Mỹ với sốt buffalo",
                    Price = 45000,
                    ImageUrl = "https://images.unsplash.com/photo-1608039829572-78524f79c4c7?w=500",
                    CategoryId = 2,
                    Tag = "gà, cánh, buffalo",
                    IsActive = true
                },
                new Food
                {
                    Id = 41,
                    Name = "Gà Nuggets",
                    Description = "Gà viên chiên giòn dạng nuggets cho trẻ em",
                    Price = 38000,
                    ImageUrl = "https://images.unsplash.com/photo-1562967914-608f82629710?w=500",
                    CategoryId = 2,
                    Tag = "gà, nuggets, trẻ em",
                    IsActive = true
                },

                // Thêm Đồ uống mới
                new Food
                {
                    Id = 42,
                    Name = "Trà Sữa Matcha",
                    Description = "Trà sữa matcha Nhật Bản với trân châu trắng",
                    Price = 42000,
                    ImageUrl = "https://images.unsplash.com/photo-1515823064-d6e0c04616a7?w=500",
                    CategoryId = 3,
                    Tag = "trà sữa, matcha, nhật bản",
                    IsActive = true
                },
                new Food
                {
                    Id = 43,
                    Name = "Sinh Tố Xoài",
                    Description = "Sinh tố xoài tươi ngọt mát",
                    Price = 38000,
                    ImageUrl = "https://images.unsplash.com/photo-1546173159-315724a31696?w=500",
                    CategoryId = 3,
                    Tag = "sinh tố, xoài, tươi",
                    IsActive = true
                },
                new Food
                {
                    Id = 44,
                    Name = "Soda Chanh Dây",
                    Description = "Soda chanh dây tươi mát giải nhiệt",
                    Price = 32000,
                    ImageUrl = "https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=500",
                    CategoryId = 3,
                    Tag = "soda, chanh dây, tươi",
                    IsActive = true
                },
                new Food
                {
                    Id = 45,
                    Name = "Trà Chanh",
                    Description = "Trà chanh tươi mát lạnh giải khát",
                    Price = 28000,
                    ImageUrl = "https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=500",
                    CategoryId = 3,
                    Tag = "trà, chanh, giải khát",
                    IsActive = true
                },
                // Thêm Món phụ mới
                new Food
                {
                    Id = 48,
                    Name = "Hành Tây Chiên Giòn",
                    Description = "Hành tây tẩm bột chiên giòn giòn với sốt",
                    Price = 35000,
                    ImageUrl = "https://images.unsplash.com/photo-1639024471283-03518883512d?w=500",
                    CategoryId = 4,
                    Tag = "hành tây, chiên, snack",
                    IsActive = true
                },
                new Food
                {
                    Id = 49,
                    Name = "Phô Mai Que Chiên",
                    Description = "Phô mai que tẩm bột chiên giòn tan",
                    Price = 40000,
                    ImageUrl = "https://images.unsplash.com/photo-1548340748-6d2b7d7da280?w=500",
                    CategoryId = 4,
                    Tag = "phô mai, chiên, que",
                    IsActive = true
                },
                new Food
                {
                    Id = 50,
                    Name = "Salad Trộn Dầu Giấm",
                    Description = "Salad rau củ tươi trộn dầu giấm balsamic",
                    Price = 38000,
                    ImageUrl = "https://images.unsplash.com/photo-1540189549336-e6e99c3679fe?w=500",
                    CategoryId = 4,
                    Tag = "salad, rau, healthy",
                    IsActive = true
                },

                // Thêm Tráng miệng mới
                new Food
                {
                    Id = 51,
                    Name = "Bánh Donut Socola",
                    Description = "Bánh donut phủ socola và rắc hạt",
                    Price = 26000,
                    ImageUrl = "https://images.unsplash.com/photo-1551024601-bec78aea704b?w=500",
                    CategoryId = 5,
                    Tag = "donut, socola, bánh",
                    IsActive = true
                },
                new Food
                {
                    Id = 52,
                    Name = "Tiramisu",
                    Description = "Tiramisu Ý truyền thống với cà phê và mascarpone",
                    Price = 45000,
                    ImageUrl = "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?w=500",
                    CategoryId = 5,
                    Tag = "tiramisu, cà phê, ý",
                    IsActive = true
                },
                new Food
                {
                    Id = 53,
                    Name = "Kem Matcha",
                    Description = "Kem matcha Nhật Bản đắng ngọt hài hòa",
                    Price = 28000,
                    ImageUrl = "https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=500",
                    CategoryId = 5,
                    Tag = "kem, matcha, nhật bản",
                    IsActive = true
                },
                new Food
                {
                    Id = 54,
                    Name = "Pudding Xoài",
                    Description = "Pudding xoài mềm mịn béo ngậy",
                    Price = 30000,
                    ImageUrl = "https://images.unsplash.com/photo-1488477181946-6428a0291777?w=500",
                    CategoryId = 5,
                    Tag = "pudding, xoài, ngọt",
                    IsActive = true
                }
            );

            modelBuilder.Entity<Combo>().HasData(
                new Combo
                {
                    Id = 1,
                    Name = "Combo Burger Bò Đặc Biệt",
                    Description = "Burger bò phô mai, khoai tây chiên và Coca Cola",
                    Price = 95000,
                    ImageUrl = "https://burgerking.vn/media/catalog/product/cache/1/image/1800x/040ec09b1e35df139433887a97daa66f/1/7/17-whopper-b_-t_m-ph_-mai-c_-l_n.jpg",
                    IsActive = true
                },
                new Combo
                {
                    Id = 2,
                    Name = "Combo Gà Rán Gia Đình",
                    Description = "4 miếng gà rán, khoai tây chiên lớn và 2 Pepsi",
                    Price = 180000,
                    ImageUrl = "https://images.unsplash.com/photo-1562967914-608f82629710?w=500",
                    IsActive = true
                },
                new Combo
                {
                    Id = 3,
                    Name = "Combo Burger Gà Tiết Kiệm",
                    Description = "Burger gà giòn, khoai tây chiên và nước cam ép",
                    Price = 100000,
                    ImageUrl = "https://images.unsplash.com/photo-1603064752734-4c48eff53d05?w=500",
                    IsActive = true
                },
                new Combo
                {
                    Id = 4,
                    Name = "Combo Học Sinh",
                    Description = "Burger bò nhỏ, cánh gà chiên và trà sữa",
                    Price = 120000,
                    ImageUrl = "https://images.unsplash.com/photo-1565299715199-866c917206bb?w=500",
                    IsActive = true
                },
                new Combo
                {
                    Id = 5,
                    Name = "Combo Tráng Miệng Ngọt Ngào",
                    Description = "Kem vani, bánh táo và trà sữa trân châu",
                    Price = 85000,
                    ImageUrl = "https://images.unsplash.com/photo-1551024601-bec78aea704b?w=500",
                    IsActive = true
                },
                new Combo
                {
                    Id = 6,
                    Name = "Combo Burger Bacon Đặc Biệt",
                    Description = "Burger bò bacon, khoai tây chiên và cà phê sữa",
                    Price = 128000,
                    ImageUrl = "https://images.unsplash.com/photo-1572802419224-296b0aeee0d9?w=500",
                    IsActive = true
                },
                new Combo
                {
                    Id = 7,
                    Name = "Combo Gà Cay Hàn Quốc",
                    Description = "Burger gà cay Hàn Quốc, cánh gà buffalo và trà sữa matcha",
                    Price = 145000,
                    ImageUrl = "https://images.unsplash.com/photo-1594212699903-ec8a3eca50f5?w=500",
                    IsActive = true
                },
                new Combo
                {
                    Id = 8,
                    Name = "Combo Healthy",
                    Description = "Salad caesar và sinh tố bơ",
                    Price = 115000,
                    ImageUrl = "https://images.unsplash.com/photo-1546793665-c74683f339c1?w=500",
                    IsActive = true
                },
                new Combo
                {
                    Id = 9,
                    Name = "Combo Trẻ Em Vui Vẻ",
                    Description = "Gà nuggets, khoai tây chiên và nước cam ép",
                    Price = 98000,
                    ImageUrl = "https://images.unsplash.com/photo-1562967914-608f82629710?w=500",
                    IsActive = true
                },
                new Combo
                {
                    Id = 10,
                    Name = "Combo Tiệc Nướng",
                    Description = "Burger phô mai nướng, hành tây chiên giòn và soda chanh dây",
                    Price = 110000,
                    ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=500",
                    IsActive = true
                },
                new Combo
                {
                    Id = 11,
                    Name = "Combo Cà Phê Sáng",
                    Description = "Burger gà giòn, bánh donut socola và cà phê đen",
                    Price = 95000,
                    ImageUrl = "https://images.unsplash.com/photo-1606755962773-d324e0a13086?w=500",
                    IsActive = true
                },
                new Combo
                {
                    Id = 12,
                    Name = "Combo Hải Sản Cao Cấp",
                    Description = "Burger tôm, gỏi cuốn tôm",
                    Price = 135000,
                    ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=500",
                    IsActive = true
                },
                new Combo
                {
                    Id = 13,
                    Name = "Combo Phô Mai Đặc Biệt",
                    Description = "Gà rán phô mai, phô mai que chiên và trà sữa",
                    Price = 130000,
                    ImageUrl = "https://images.unsplash.com/photo-1626082927389-6cd097cdc6ec?w=500",
                    IsActive = true
                },
                new Combo
                {
                    Id = 14,
                    Name = "Combo Tráng Miệng Cao Cấp",
                    Description = "Tiramisu, cheesecake dâu và trà đào cam sả",
                    Price = 125000,
                    ImageUrl = "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?w=500",
                    IsActive = true
                },
                new Combo
                {
                    Id = 15,
                    Name = "Combo Bạn Bè",
                    Description = "2 burger bò phô mai, 2 khoai tây chiên và 2 pepsi",
                    Price = 225000,
                    ImageUrl = "https://images.unsplash.com/photo-1553979459-d2229ba7433b?w=500",
                    IsActive = true
                }
            );

            modelBuilder.Entity<ComboItem>().HasData(
                // Combo 1: Burger Bò Đặc Biệt
                new ComboItem { Id = 1, ComboId = 1, FoodId = 1, Quantity = 1 },  // Burger Bò
                new ComboItem { Id = 2, ComboId = 1, FoodId = 11, Quantity = 1 }, // Khoai Tây
                new ComboItem { Id = 3, ComboId = 1, FoodId = 7, Quantity = 1 },  // Coca

                // Combo 2: Gà Rán Gia Đình
                new ComboItem { Id = 4, ComboId = 2, FoodId = 4, Quantity = 4 },  // 4 Gà Rán
                new ComboItem { Id = 5, ComboId = 2, FoodId = 11, Quantity = 2 }, // 2 Khoai Tây
                new ComboItem { Id = 6, ComboId = 2, FoodId = 8, Quantity = 2 },  // 2 Pepsi

                // Combo 3: Burger Gà Tiết Kiệm
                new ComboItem { Id = 7, ComboId = 3, FoodId = 2, Quantity = 1 },  // Burger Gà
                new ComboItem { Id = 8, ComboId = 3, FoodId = 11, Quantity = 1 }, // Khoai Tây
                new ComboItem { Id = 9, ComboId = 3, FoodId = 10, Quantity = 1 }, // Nước Cam

                // Combo 4: Học Sinh
                new ComboItem { Id = 10, ComboId = 4, FoodId = 1, Quantity = 1 }, // Burger Bò
                new ComboItem { Id = 11, ComboId = 4, FoodId = 6, Quantity = 2 }, // 2 Cánh Gà
                new ComboItem { Id = 12, ComboId = 4, FoodId = 9, Quantity = 1 }, // Trà Sữa

                // Combo 5: Tráng Miệng
                new ComboItem { Id = 13, ComboId = 5, FoodId = 13, Quantity = 1 }, // Kem Vani
                new ComboItem { Id = 14, ComboId = 5, FoodId = 14, Quantity = 1 }, // Bánh Táo
                new ComboItem { Id = 15, ComboId = 5, FoodId = 9, Quantity = 1 },  // Trà Sữa

                // Combo 6: Burger Bacon Đặc Biệt
                new ComboItem { Id = 16, ComboId = 6, FoodId = 36, Quantity = 1 }, // Burger Bò Bacon
                new ComboItem { Id = 17, ComboId = 6, FoodId = 11, Quantity = 1 }, // Khoai Tây Chiên
                new ComboItem { Id = 18, ComboId = 6, FoodId = 26, Quantity = 1 }, // Cà Phê Sữa

                // Combo 7: Gà Cay Hàn Quốc
                new ComboItem { Id = 19, ComboId = 7, FoodId = 37, Quantity = 1 }, // Burger Gà Cay Hàn Quốc
                new ComboItem { Id = 20, ComboId = 7, FoodId = 40, Quantity = 2 }, // Cánh Gà Buffalo
                new ComboItem { Id = 21, ComboId = 7, FoodId = 42, Quantity = 1 }, // Trà Sữa Matcha

                // Combo 8: Healthy
                new ComboItem { Id = 22, ComboId = 8, FoodId = 30, Quantity = 1 }, // Salad Caesar
                new ComboItem { Id = 24, ComboId = 8, FoodId = 24, Quantity = 1 }, // Sinh Tố Bơ

                // Combo 9: Trẻ Em Vui Vẻ
                new ComboItem { Id = 25, ComboId = 9, FoodId = 41, Quantity = 1 }, // Gà Nuggets
                new ComboItem { Id = 26, ComboId = 9, FoodId = 11, Quantity = 1 }, // Khoai Tây Chiên
                new ComboItem { Id = 27, ComboId = 9, FoodId = 10, Quantity = 1 }, // Nước Cam Ép

                // Combo 10: Tiệc Nướng
                new ComboItem { Id = 28, ComboId = 10, FoodId = 38, Quantity = 1 }, // Burger Phô Mai Nướng
                new ComboItem { Id = 29, ComboId = 10, FoodId = 48, Quantity = 1 }, // Hành Tây Chiên Giòn
                new ComboItem { Id = 30, ComboId = 10, FoodId = 44, Quantity = 1 }, // Soda Chanh Dây

                // Combo 11: Cà Phê Sáng
                new ComboItem { Id = 31, ComboId = 11, FoodId = 2, Quantity = 1 },  // Burger Gà Giòn
                new ComboItem { Id = 32, ComboId = 11, FoodId = 51, Quantity = 1 }, // Bánh Donut Socola
                new ComboItem { Id = 33, ComboId = 11, FoodId = 25, Quantity = 1 }, // Cà Phê Đen

                // Combo 12: Hải Sản Cao Cấp
                new ComboItem { Id = 34, ComboId = 12, FoodId = 15, Quantity = 1 }, // Burger Tôm
                new ComboItem { Id = 35, ComboId = 12, FoodId = 29, Quantity = 2 }, // Gỏi Cuốn Tôm

                // Combo 13: Phô Mai Đặc Biệt
                new ComboItem { Id = 37, ComboId = 13, FoodId = 39, Quantity = 2 }, // Gà Rán Phô Mai
                new ComboItem { Id = 38, ComboId = 13, FoodId = 49, Quantity = 1 }, // Phô Mai Que Chiên
                new ComboItem { Id = 39, ComboId = 13, FoodId = 9, Quantity = 1 },  // Trà Sữa

                // Combo 14: Tráng Miệng Cao Cấp
                new ComboItem { Id = 40, ComboId = 14, FoodId = 52, Quantity = 1 }, // Tiramisu
                new ComboItem { Id = 42, ComboId = 14, FoodId = 23, Quantity = 1 }, // Trà Đào Cam Sả

                // Combo 15: Bạn Bè
                new ComboItem { Id = 43, ComboId = 15, FoodId = 1, Quantity = 2 },  // 2 Burger Bò Phô Mai
                new ComboItem { Id = 45, ComboId = 15, FoodId = 11, Quantity = 2 }, // 2 Khoai Tây Chiên
                new ComboItem { Id = 46, ComboId = 15, FoodId = 8, Quantity = 2 }   // 2 Pepsi
            );
        }
    }
}
