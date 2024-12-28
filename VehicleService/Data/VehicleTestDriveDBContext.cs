using Microsoft.EntityFrameworkCore;
using VehicleServiceAPI.Models;

namespace VehicleServiceAPI.data
{

    public class VehicleTestDriveDBContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public object Vehicle { get; internal set; }

        public VehicleTestDriveDBContext() : base()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=VehicleTestDriveDB;");
        }

    }
}