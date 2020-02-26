using PagedList;
using ProjectVehicle.Service.Common;
using ProjectVehicle.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVehicle.Service.Services
{
    public class VehicleMakeService : IVehicleMakeService
    {
        readonly VehicleContext dbConnection;
        public VehicleMakeService(VehicleContext dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public Task<bool> ValidateVehicleNameAsync(string manufacturerName)
        {
            var result = !dbConnection.VehiclesMakes.Any(name => name.ManufacturerName == manufacturerName);
            return Task.FromResult(result);
            
        }

        
        public Task<IPagedList<VehicleMake>> GetAllAsync(IVehicleSorting sort, IVehicleFiltering filter, IVehiclePaging page)
        {
            var vehicles = dbConnection.VehiclesMakes.Select(v => v);

            var searchVehicle = filter.Filter;
            var sortVehicle = sort.Sort;
            
            if (!String.IsNullOrEmpty(searchVehicle))
            {
                vehicles = vehicles.Where(v => v.ManufacturerName.Contains(searchVehicle)
                                       || v.MadeIn.Contains(searchVehicle));
            }
            switch (sortVehicle)
            {
                case "name_desc":
                    vehicles = vehicles.OrderByDescending(v => v.ManufacturerName);
                    break;
                case "madein_desc":
                    vehicles = vehicles.OrderByDescending(v => v.MadeIn);
                    break;
                default:
                    vehicles = vehicles.OrderBy(v => v.ManufacturerName);
                    break;
            }

            var pagedList = vehicles.ToPagedList(page.Page ?? 1, 3);

            return Task.FromResult(pagedList);
            
        }


        public async Task<IVehicleMake> FindAsync(int? id)
        {            
            var vehicleMakes = dbConnection.VehiclesMakes;
            var vehicleMake = await vehicleMakes.FindAsync(id);
            return vehicleMake;
        }

        public Task InsertOrUpdateAsync(VehicleMake vehicleMake)
        {
            if (vehicleMake.ID == default)
            {
                dbConnection.VehiclesMakes.Add(vehicleMake);
                return dbConnection.SaveChangesAsync();
            }
            else
            {
                dbConnection.Entry(vehicleMake).State = System.Data.Entity.EntityState.Modified;
                return dbConnection.SaveChangesAsync();
            }
        }

        public Task DeleteAsync(int id)
        {
            var vehicleMake = dbConnection.VehiclesMakes.Find(id);
            dbConnection.VehiclesMakes.Remove(vehicleMake);
            return dbConnection.SaveChangesAsync();
        }

        public void Dispose()
        {
            dbConnection.Dispose();
        }
    }
}
