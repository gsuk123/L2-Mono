using PagedList;
using ProjectVehicle.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ProjectVehicle.Service.Common;
using AutoMapper;
using ProjectVehicle.Service.DAL;

namespace ProjectVehicle.Service.Services
{
    public class VehicleModelService : IVehicleModelService
    {
        readonly VehicleContext dbConnection;
        readonly IMapper mapper;
        public VehicleModelService(VehicleContext dbConnection, IMapper mapper)
        {
            this.dbConnection = dbConnection;
            this.mapper = mapper;
        }

        public Task<IPagedList<IVehicleModel>> FindVehicleModelsAsync(IVehicleSorting sort, IVehicleFiltering filter, IVehiclePaging page, int? makeId = null)
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
            
            var mappedList = mapper.Map<List<VehicleModelEntityModel>, List<IVehicleModel>>(vehicleModels.ToList());
            var pagedList = mappedList.ToPagedList(page.Page ?? 1, 3);
            return Task.FromResult(pagedList);

        }
        public Task<IEnumerable<IVehicleMake>> GetVehiclesMake()
        {
            var vehicleMake =  dbConnection.VehiclesMakes;
            var mappedList = mapper.Map<List<VehicleMakeEntityModel>, List<IVehicleMake>>(vehicleMake.ToList());
            return Task.FromResult<IEnumerable<IVehicleMake>>(mappedList);
        }

        public async Task<IVehicleModel> GetVehicleModelAsync(int? id)
        {
            var vehicleModels = dbConnection.VehiclesModels;
            var vehicleModel = await vehicleModels.FindAsync(id);
            IVehicleModel vehicle = mapper.Map<IVehicleModel>(vehicleModel);
            return vehicle;
        }

        public Task InsertOrUpdateVehicleModelAsync(IVehicleModel vehicleModel)
        {
            VehicleModelEntityModel vehicleModelEM = mapper.Map<VehicleModelEntityModel>(vehicleModel);
            if (vehicleModelEM.VehicleModelID == default)
            {
                dbConnection.VehiclesModels.Add(vehicleModelEM);
                return dbConnection.SaveChangesAsync();
            }
            else
            {
                dbConnection.Entry(vehicleModelEM).State = System.Data.Entity.EntityState.Modified;
                return dbConnection.SaveChangesAsync();
            }
        }

        public Task DeleteVehicleModelAsync(int id)
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
