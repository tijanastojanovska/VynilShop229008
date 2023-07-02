using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text;
using VynilShop.Domain.DomainModels;
using VynilShop.Domain.Identity;

namespace VynilShop.Repository.Data
{
    public class ApplicationDbContext : IdentityDbContext<VynilShopUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Vynil> Vynils { get; set; }
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<VynilInShoppingCart> VynilInShoppingCarts { get; set; }
        public virtual DbSet<VynilInOrder> VynilInOrders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Vynil>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Artist>()
               .Property(z => z.Id)
               .ValueGeneratedOnAdd();

            builder.Entity<VynilInShoppingCart>()
                .HasOne(z => z.Vynil)
                .WithMany(z => z.VynilInShoppingCarts)
                .HasForeignKey(z => z.ShoppingCartId);

            builder.Entity<VynilInShoppingCart>()
                .HasOne(z => z.ShoppingCart)
                .WithMany(z => z.VynilInShoppingCarts)
                .HasForeignKey(z => z.VynilId);


            builder.Entity<ShoppingCart>()
                .HasOne<VynilShopUser>(z => z.Owner)
                .WithOne(z => z.UserCart)
                .HasForeignKey<ShoppingCart>(z => z.OwnerId);


            //builder.Entity<ProductInOrder>()
            //   .HasKey(z => new { z.ProductId, z.OrderId });

            builder.Entity<VynilInOrder>()
                .HasOne(z => z.OrderedVynil)
                .WithMany(z => z.VynilInOrders)
                .HasForeignKey(z => z.OrderId);

            builder.Entity<VynilInOrder>()
                .HasOne(z => z.UserOrder)
                .WithMany(z => z.VynilInOrders)
                .HasForeignKey(z => z.VynilId);


            builder.Entity<Vynil>()
              .HasOne(z => z.Artist)
              .WithMany(z => z.Vynils)
              .HasForeignKey(z => z.ArtistId);
        }
    }
}
