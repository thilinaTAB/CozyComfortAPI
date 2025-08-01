using Microsoft.EntityFrameworkCore;
using CozyComfortAPI.Model;

namespace CozyComfortAPI.Data
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext>opp):base(opp)
            { }
        //List down all the models as given below
        public DbSet<BlanketModel> BlanketModels { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<Material> Materials { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);
            //If you are happy to allow EF to select matching data types
            //Then use option 1
            //Option 1
            model.Entity<Distributor>();
            model.Entity<Material>();
            //Some cases you need to mention the data type
            //In such case use option 2
            //Option 2
            model.Entity<BlanketModel>().Property(p=>p.Price).HasColumnType("decimal(18,2)");
            model.Entity<Order>().Property(p => p.Discount).HasColumnType("decimal(18,2)");
            model.Entity<Order>().Property(p => p.Total).HasColumnType("decimal(18,2)");
            model.Entity<BlanketModel>().HasOne(b => b.Material).WithMany().HasForeignKey(b => b.MaterialID).OnDelete(DeleteBehavior.Cascade);

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
