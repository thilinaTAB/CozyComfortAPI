using Microsoft.EntityFrameworkCore;
using CozyComfortAPI.Model;

namespace CozyComfortAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opp) : base(opp)
        { }
        public DbSet<BlanketModel> BlanketModels { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<DistributorStock> DistributorStocks { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);

            model.Entity<Distributor>();
            model.Entity<Material>();

            model.Entity<BlanketModel>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            model.Entity<Order>().Property(p => p.Total).HasColumnType("decimal(18,2)");

            model.Entity<BlanketModel>()
                .HasOne(b => b.Material)
                .WithMany()
                .HasForeignKey(b => b.MaterialID)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<DistributorStock>()
                .HasOne(ds => ds.Distributor)
                .WithMany(d => d.DistributorStocks)
                .HasForeignKey(ds => ds.DistributorID);

            model.Entity<DistributorStock>()
                .HasOne(ds => ds.BlanketModel)
                .WithMany(bm => bm.DistributorStocks)
                .HasForeignKey(ds => ds.ModelID);

            model.Entity<Material>().HasData(
                new Material { MaterialID = 1, MaterialName = "Cotton", Description = "100% organic cotton" },
                new Material { MaterialID = 2, MaterialName = "Wool", Description = "Soft merino wool" },
                new Material { MaterialID = 3, MaterialName = "Fleece", Description = "Warm polyester fleece" },
                new Material { MaterialID = 4, MaterialName = "Bamboo", Description = "Eco-friendly bamboo fiber" },
                new Material { MaterialID = 5, MaterialName = "Cashmere", Description = "Luxurious cashmere wool" },
                new Material { MaterialID = 6, MaterialName = "Chenille", Description = "Soft chenille fabric" }
            );
        }
    }
}
