﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebShopAPI.Data;

#nullable disable

namespace WebShopAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241012122039_Discount")]
    partial class Discount
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("WebShopAPI.Data.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Avt")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("PhoneNumber")
                        .IsUnique()
                        .HasFilter("[PhoneNumber] IS NOT NULL");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("WebShopAPI.Data.Banner", b =>
                {
                    b.Property<string>("IdBanner")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LinkImg")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("IdBanner");

                    b.ToTable("banners");
                });

            modelBuilder.Entity("WebShopAPI.Data.Category", b =>
                {
                    b.Property<string>("IdCate")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameCate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusCate")
                        .HasColumnType("int");

                    b.HasKey("IdCate");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("WebShopAPI.Data.ImgPro", b =>
                {
                    b.Property<string>("IdImg")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdPro")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LinkImg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdImg");

                    b.HasIndex("IdPro");

                    b.ToTable("imgPros");
                });

            modelBuilder.Entity("WebShopAPI.Data.Order", b =>
                {
                    b.Property<string>("IdOrder")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdAcc")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("OrderEnd")
                        .HasColumnType("int");

                    b.Property<int?>("OrderInProgress")
                        .HasColumnType("int");

                    b.Property<int?>("OrderStart")
                        .HasColumnType("int");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.Property<double>("OrderTotal")
                        .HasColumnType("float");

                    b.Property<double?>("OrderTotalDiscount")
                        .HasColumnType("float");

                    b.Property<string>("PaymentMethodId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("ShippingFee")
                        .HasColumnType("real");

                    b.HasKey("IdOrder");

                    b.HasIndex("IdAcc");

                    b.ToTable("orders");
                });

            modelBuilder.Entity("WebShopAPI.Data.OrderDetail", b =>
                {
                    b.Property<string>("IdOrderDetail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdOrder")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdProItem")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("OrderIdOrder")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("OrderTotal")
                        .HasColumnType("float");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ProductItemIdProItem")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int?>("Review")
                        .HasColumnType("int");

                    b.HasKey("IdOrderDetail");

                    b.HasIndex("IdOrder");

                    b.HasIndex("IdProItem");

                    b.HasIndex("OrderIdOrder");

                    b.HasIndex("ProductItemIdProItem");

                    b.ToTable("orderDetails");
                });

            modelBuilder.Entity("WebShopAPI.Data.Product", b =>
                {
                    b.Property<string>("IdPro")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("BestSeller")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdCate")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LongDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusProduct")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdPro");

                    b.HasIndex("IdCate");

                    b.ToTable("products");
                });

            modelBuilder.Entity("WebShopAPI.Data.ProductItem", b =>
                {
                    b.Property<string>("IdProItem")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdPro")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Size")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusProItem")
                        .HasColumnType("int");

                    b.HasKey("IdProItem");

                    b.HasIndex("IdPro");

                    b.ToTable("productItems");
                });

            modelBuilder.Entity("WebShopAPI.Data.Review", b =>
                {
                    b.Property<string>("IdReview")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Dislike")
                        .HasColumnType("int");

                    b.Property<string>("IdAcc")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdPro")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("Like")
                        .HasColumnType("int");

                    b.Property<int>("RatingValue")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ReviewDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("statusReview")
                        .HasColumnType("int");

                    b.HasKey("IdReview");

                    b.HasIndex("IdAcc");

                    b.HasIndex("IdPro");

                    b.ToTable("reviews");
                });

            modelBuilder.Entity("WebShopAPI.Data.ShoppingCart", b =>
                {
                    b.Property<string>("IdCart")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdAcc")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IdCart");

                    b.HasIndex("IdAcc");

                    b.ToTable("shoppingCarts");
                });

            modelBuilder.Entity("WebShopAPI.Data.ShoppingCartItem", b =>
                {
                    b.Property<string>("IdCartItem")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdCart")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdPro")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdProItem")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("IdCartItem");

                    b.HasIndex("IdCart");

                    b.HasIndex("IdPro");

                    b.HasIndex("IdProItem");

                    b.ToTable("shoppingCartItems");
                });

            modelBuilder.Entity("WebShopAPI.Data.Wishlist", b =>
                {
                    b.Property<string>("IdWishlist")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdAcc")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdPro")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IdWishlist");

                    b.HasIndex("IdAcc");

                    b.HasIndex("IdPro");

                    b.ToTable("wishlists");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WebShopAPI.Data.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WebShopAPI.Data.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebShopAPI.Data.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("WebShopAPI.Data.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebShopAPI.Data.ImgPro", b =>
                {
                    b.HasOne("WebShopAPI.Data.Product", "product")
                        .WithMany("ImgPros")
                        .HasForeignKey("IdPro")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ImgPro_Product");

                    b.Navigation("product");
                });

            modelBuilder.Entity("WebShopAPI.Data.Order", b =>
                {
                    b.HasOne("WebShopAPI.Data.AppUser", "User")
                        .WithMany("Orders")
                        .HasForeignKey("IdAcc")
                        .IsRequired()
                        .HasConstraintName("FK_order_user");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebShopAPI.Data.OrderDetail", b =>
                {
                    b.HasOne("WebShopAPI.Data.Order", null)
                        .WithMany("OrderDetails")
                        .HasForeignKey("IdOrder")
                        .IsRequired()
                        .HasConstraintName("FK_order_detail_order");

                    b.HasOne("WebShopAPI.Data.ProductItem", null)
                        .WithMany("OrderDetails")
                        .HasForeignKey("IdProItem")
                        .IsRequired()
                        .HasConstraintName("FK_order_detail_product_item");

                    b.HasOne("WebShopAPI.Data.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderIdOrder")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebShopAPI.Data.ProductItem", "ProductItem")
                        .WithMany()
                        .HasForeignKey("ProductItemIdProItem")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("ProductItem");
                });

            modelBuilder.Entity("WebShopAPI.Data.Product", b =>
                {
                    b.HasOne("WebShopAPI.Data.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("IdCate")
                        .IsRequired()
                        .HasConstraintName("FK_Product_Category");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("WebShopAPI.Data.ProductItem", b =>
                {
                    b.HasOne("WebShopAPI.Data.Product", "product")
                        .WithMany("ProductItems")
                        .HasForeignKey("IdPro")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Product_ProductItem");

                    b.Navigation("product");
                });

            modelBuilder.Entity("WebShopAPI.Data.Review", b =>
                {
                    b.HasOne("WebShopAPI.Data.AppUser", "user")
                        .WithMany("Reviews")
                        .HasForeignKey("IdAcc")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Review_User");

                    b.HasOne("WebShopAPI.Data.Product", "product")
                        .WithMany("Reviews")
                        .HasForeignKey("IdPro")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Review_Product");

                    b.Navigation("product");

                    b.Navigation("user");
                });

            modelBuilder.Entity("WebShopAPI.Data.ShoppingCart", b =>
                {
                    b.HasOne("WebShopAPI.Data.AppUser", "User")
                        .WithMany("ShoppingCarts")
                        .HasForeignKey("IdAcc")
                        .IsRequired()
                        .HasConstraintName("FK_shopping_cart_user");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebShopAPI.Data.ShoppingCartItem", b =>
                {
                    b.HasOne("WebShopAPI.Data.ShoppingCart", "ShoppingCart")
                        .WithMany("ShoppingCartItems")
                        .HasForeignKey("IdCart")
                        .IsRequired()
                        .HasConstraintName("FK_shopping_cart_item_shopping_cart");

                    b.HasOne("WebShopAPI.Data.Product", "Product")
                        .WithMany("ShoppingCartItems")
                        .HasForeignKey("IdPro")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_shopping_cart_item_product");

                    b.HasOne("WebShopAPI.Data.ProductItem", "ProductItem")
                        .WithMany("ShoppingCartItems")
                        .HasForeignKey("IdProItem")
                        .IsRequired()
                        .HasConstraintName("FK_shopping_cart_item_product_item");

                    b.Navigation("Product");

                    b.Navigation("ProductItem");

                    b.Navigation("ShoppingCart");
                });

            modelBuilder.Entity("WebShopAPI.Data.Wishlist", b =>
                {
                    b.HasOne("WebShopAPI.Data.AppUser", "user")
                        .WithMany("Wishlists")
                        .HasForeignKey("IdAcc")
                        .IsRequired()
                        .HasConstraintName("FK_wishlist_Users");

                    b.HasOne("WebShopAPI.Data.Product", "product")
                        .WithMany("Wishlists")
                        .HasForeignKey("IdPro")
                        .IsRequired()
                        .HasConstraintName("FK_wishlist_product");

                    b.Navigation("product");

                    b.Navigation("user");
                });

            modelBuilder.Entity("WebShopAPI.Data.AppUser", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Reviews");

                    b.Navigation("ShoppingCarts");

                    b.Navigation("Wishlists");
                });

            modelBuilder.Entity("WebShopAPI.Data.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("WebShopAPI.Data.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("WebShopAPI.Data.Product", b =>
                {
                    b.Navigation("ImgPros");

                    b.Navigation("ProductItems");

                    b.Navigation("Reviews");

                    b.Navigation("ShoppingCartItems");

                    b.Navigation("Wishlists");
                });

            modelBuilder.Entity("WebShopAPI.Data.ProductItem", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("ShoppingCartItems");
                });

            modelBuilder.Entity("WebShopAPI.Data.ShoppingCart", b =>
                {
                    b.Navigation("ShoppingCartItems");
                });
#pragma warning restore 612, 618
        }
    }
}
