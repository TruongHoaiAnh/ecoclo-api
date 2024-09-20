using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebShopAPI.Models;

namespace WebShopAPI.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> products { get; set; }
        public DbSet<ProductItem> productItems { get; set; }
        public DbSet<ImgPro> imgPros { get; set; }
        public DbSet<Review> reviews { get; set; }
        public DbSet<Wishlist> wishlists { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<ShoppingCart> shoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> shoppingCartItems { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           /* modelBuilder.Entity<ProductModel>()
                .HasKey(p => p.IdCate);

*/

            // Tạo chỉ mục duy nhất cho PhoneNumber
            modelBuilder.Entity<AppUser>()
                   .HasIndex(u => u.PhoneNumber)
                   .IsUnique();

            //Product
            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductItems) // Product có nhiều ProductItems
                .WithOne(pi => pi.product)    // Mỗi ProductItem thuộc về một Product
                .HasForeignKey(pi => pi.IdPro) // Thiết lập khóa ngoại trong ProductItem
                .HasConstraintName("FK_Product_ProductItem");

            //Review
            modelBuilder.Entity<Review>()
               .HasOne(r => r.product)  // Đối tượng Product liên kết với Review
               .WithMany(p => p.Reviews)  // Một Product có nhiều Review
               .HasForeignKey(r => r.IdPro)  // Khóa ngoại trong Review
               .HasConstraintName("FK_Review_Product");
            modelBuilder.Entity<Review>()
                .HasOne(r => r.user)  // Đối tượng AppUser liên kết với Review
                .WithMany(r => r.Reviews)  // Một AppUser có thể có nhiều Review (nếu muốn cấu hình thêm thì thêm thuộc tính)
                .HasForeignKey(r => r.IdAcc)  // Khóa ngoại trong Review
                .HasConstraintName("FK_Review_User");
            //Img
            modelBuilder.Entity<ImgPro>()
                .HasOne(i => i.product)        
                .WithMany(p => p.ImgPros)     
                .HasForeignKey(i => i.IdPro)    
                .HasConstraintName("FK_ImgPro_Product");

            //Wishlist
            modelBuilder.Entity<Wishlist>()
                    .HasOne(d => d.product)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.IdPro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_wishlist_product");
            modelBuilder.Entity<Wishlist>()
            .HasOne(d => d.user)
               .WithMany(p => p.Wishlists)
               .HasForeignKey(d => d.IdPro)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_wishlist_Users");
            //Category
            modelBuilder.Entity<Product>()
            .HasOne(e => e.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(d => d.IdCate)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_Product_Category");

            //Shopping cart item
            modelBuilder.Entity<ShoppingCartItem>(entity =>
            {
                entity.HasOne(d => d.ShoppingCart)
                    .WithMany(p => p.ShoppingCartItems)
                    .HasForeignKey(d => d.IdCart)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_shopping_cart_item_shopping_cart");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ShoppingCartItems)
                    .HasForeignKey(d => d.IdPro)
                    .HasConstraintName("FK_shopping_cart_item_product");

                entity.HasOne(d => d.ProductItem)
                    .WithMany(p => p.ShoppingCartItems)
                    .HasForeignKey(d => d.IdProItem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_shopping_cart_item_product_item");
            });

            //Shopping cart
            modelBuilder.Entity<ShoppingCart>()
            .HasOne(e => e.User)
               .WithMany(c => c.ShoppingCarts)
               .HasForeignKey(d => d.IdCart)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_shopping_cart_user");

           



            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }


        }

    }
}
