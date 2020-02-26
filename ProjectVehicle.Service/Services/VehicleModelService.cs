using PagedList;
using ProjectVehicle.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ProjectVehicle.Service.Common;

namespace ProjectVehicle.Service.Services
{
    public class VehicleModelService : IVehicleModelService
    {
        readonly VehicleContext dbConnection;
        public VehicleModelService(VehicleContext dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public Task<IPagedList<VehicleModel>> GetAllModelsAsync(IVehicleSorting sort, IVehicleFiltering filter, IVehiclePaging page, int? makeId = null)
        {
            var vehicleModels = dbConnection.VehiclesModels.Include(v => v.VehicleMake);

            var searchVehicle = filter.Filter;
            var sortVehicle = sort.Sort;

            if (makeId.HasValue)
            {
                vehicleModels = vehicleModels.Where(m => m.VehicleMakeID == makeId);
            }
            if (!String.IsNullOrEmpty(searchVehicle))
            {
                vehicleModels = vehicleModels.Where(v => v.ModelName.Contains(searchVehicle));
            }
            switch (sortVehicle)
            {
                case "model_desc":
                    vehicleModels = vehicleModels.OrderByDescending(v => v.ModelName);
                    break;
                case "Year":
                    vehicleModels = vehicleModels.OrderBy(v => v.ModelYear);
                    break;
                case "year_desc":
                    vehicleModels = vehicleModels.OrderByDescending(v => v.ModelYear);
                    break;
                default:
                    vehicleModels = vehicleModels.OrderBy(v => v.ModelName);
                    break;
            }
            var pagedList = vehicleModels.ToPagedList(page.Page ?? 1, 3);

            return Task.FromResult(pagedList);

        }
        public IEnumerable<IVehicleMake> SelectAll()
        {
            var vehicleMake =  dbConnection.VehiclesMakes;
            return vehicleMake.ToList();
        }

        //public Task<VehicleModel> FindModelAsync(int? id)
        //{
        //    VehicleModel vehicleModel = new VehicleModel();
        //    return dbConnection.VehiclesModels.FindAsync(id);
        //}
        public async Task<IVehicleModel> FindModelAsync(int? id)
        {
            var vehicleModel = dbConnection.VehiclesModels;
            var make = await vehicleModel.FindAsync(id);
            return make;
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
