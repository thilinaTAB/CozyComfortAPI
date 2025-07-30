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
            model.Entity<BlanketModel>().HasOne(b => b.Materials).WithMany().HasForeignKey(b => b.MaterialID).OnDelete(DeleteBehavior.Cascade);
        }

    }
}
