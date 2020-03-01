using ProjectVehicle.Service.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVehicle.Service.Services
{
    public class VehicleContext : DbContext
    {
        public DbSet<VehicleMakeEntityModel> VehiclesMakes { get; set; }
        public DbSet<VehicleModelEntityModel> VehiclesModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}
