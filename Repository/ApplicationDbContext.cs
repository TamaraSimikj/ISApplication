using Domain.DomainModels;
using Domain.Identity;
using Domain.Relations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Repository
{

    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<IdentityUserClaim<string>> IdentityUser { get; set; }
        public DbSet<CinemaApplicationUser> ApplicationUser { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductInOrder> ProductInOrders { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ProductInShoppingCart> ProductInShoppingCarts { get; set; }
        public DbSet<EmailMessage> EmailMessages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>().Property(z => z.Id).ValueGeneratedOnAdd();

            builder.Entity<ProductInShoppingCart>().HasOne(z => z.CurrentProduct).WithMany(z => z.ProductInShoppingCarts)
                .HasForeignKey(z => z.ShoppingCartId);

            builder.Entity<ProductInShoppingCart>().HasOne(z => z.OwnersCard).WithMany(z => z.ProductInShoppingCard)
                .HasForeignKey(z => z.ProductId);

            builder.Entity<ShoppingCart>().HasOne(z => z.Owner).WithOne(z => z.OwnersCard)
                .HasForeignKey<ShoppingCart>(z => z.OwnerId);

            builder.Entity<ProductInOrder>()
                .HasOne(z => z.Product)
                .WithMany(z => z.ProductInOrders)
                .HasForeignKey(z => z.ProductId);

            builder.Entity<ProductInOrder>()
                .HasOne(z => z.Order)
                .WithMany(z => z.ProductInOrders)
                .HasForeignKey(z => z.OrderId);


        }

    }
}


