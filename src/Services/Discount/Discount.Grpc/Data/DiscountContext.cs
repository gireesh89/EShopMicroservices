using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public class DiscountContext:DbContext
    {
        public DbSet<Coupons> Coupons { get; set; }

        public DiscountContext(DbContextOptions<DiscountContext> options)
            :base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupons>().HasData(
                new Coupons { Id = 1, ProductName = "IPhone X", Description = "IPhone Discount", Amount = 150 },
                new Coupons { Id = 2, ProductName = "Samsung 10", Description="Samsung Discount", Amount= 100}
                );
        }
    }
}
