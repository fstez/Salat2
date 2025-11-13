using Microsoft.EntityFrameworkCore;
using Salat.Models;

namespace Salat.Data
{
    public class SalatDbContext : DbContext
    {
        public SalatDbContext(DbContextOptions<SalatDbContext> options) : base(options) { }

        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<FoodComponent> FoodComponents { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<FoodItem>().HasIndex(x => x.Name).IsUnique();
            b.Entity<Food>().HasIndex(x => x.Name).IsUnique();

            b.Entity<FoodComponent>()
                .HasOne(c => c.Food)
                .WithMany(f => f.Components)
                .HasForeignKey(c => c.FoodId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<FoodComponent>()
                .HasOne(c => c.FoodItem)
                .WithMany()
                .HasForeignKey(c => c.FoodItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
