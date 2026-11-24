using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.Concrete
{
    public class SignalrContext : IdentityDbContext<AppUser,AppRole,int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=LAPTOP-5Q1ARH5E;initial catalog=SignalRDb;integrated security=true;trust server certificate=true;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.RestaurantTable)
                .WithMany(t => t.Orders)
                .HasForeignKey(o => o.RestaurantTableId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Order>().ToTable(tb => tb.UseSqlOutputClause(false));
            modelBuilder.Entity<OrderDetail>().ToTable(tb => tb.UseSqlOutputClause(false));
        }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MoneyCase> MoneyCases { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<RestaurantTable> RestaurantTables { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
    }
}