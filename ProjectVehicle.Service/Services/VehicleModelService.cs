using PagedList;
using ProjectVehicle.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ProjectVehicle.Service.Services
{
    public class VehicleModelService : IVehicleModelService
    {
        readonly VehicleContext dbConnection;
        public VehicleModelService(VehicleContext dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public IPagedList<VehicleModel> AllModels(string sortOrder, string searchString, int? page, int? makeId = null)
        {
            var vehicleModels = dbConnection.VehiclesModels.Include(v => v.VehicleMake);

            if (makeId.HasValue)
            {
                vehicleModels = vehicleModels.Where(m => m.VehicleMakeID == makeId);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                vehicleModels = vehicleModels.Where(v => v.ModelName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "model_desc":
                    vehicleModels = vehicleModels.OrderByDescending(v => v.ModelName);
                    break;
                default:
                    vehicleModels = vehicleModels.OrderBy(v => v.ModelName);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return vehicleModels.ToPagedList(pageNumber, pageSize);

        }
        public IEnumerable<IVehicleMake> SelectAll()
        {
            var vehicleMake = dbConnection.VehiclesMakes;
            return vehicleMake.ToList();
        }

        public Task<VehicleModel> FindModelAsync(int? id)
        {
            VehicleModel vehicleModel = new VehicleModel();
            return dbConnection.VehiclesModels.FindAsync(id);
        }

        public Task InsertOrUpdateModelAsync(VehicleModel vehicleModel)
        {
            if (vehicleModel.VehicleModelID == default)
            {
                dbConnection.VehiclesModels.Add(vehicleModel);
                return dbConnection.SaveChangesAsync();
            }
            else
            {
                dbConnection.Entry(vehicleModel).State = System.Data.Entity.EntityState.Modified;
                return dbConnection.SaveChangesAsync();
            }
        }

        public Task DeleteModelAsync(int id)
        {
            var vehicleModel = dbConnection.VehiclesModels.Find(id);
            dbConnection.VehiclesModels.Remove(vehicleModel);
            return dbConnection.SaveChangesAsync();
        }

        public void Dispose()
        {
            dbConnection.Dispose();
        }
    }
}
