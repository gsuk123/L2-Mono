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

        public bool ValidateName(string ManufacturerName)
        {
            var result = !dbConnection.VehiclesMakes.Any(name => name.ManufacturerName == ManufacturerName);
            return result;
        }

        //public IPagedList<VehicleMake> GetAll(string sortOrder, string searchString, int? page)
        public IPagedList<VehicleMake> GetAll(IVehicleSorting sort, IVehicleFiltering search, IVehiclePaging page)
        {
            var vehicles = dbConnection.VehiclesMakes.Select(v => v);

            var searchString = search.Search;
            var sortOrder = sort.Sort;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                vehicles = vehicles.Where(v => v.ManufacturerName.Contains(searchString)
                                       || v.MadeIn.Contains(searchString));
            }
            switch (sortOrder)
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
            int pageSize = 3;
            int pageNumber = (page.Page ?? 1);


            return vehicles.ToPagedList(pageNumber, pageSize);
            
        }

        //public Task<VehicleMake> FindAsync(int? id)
        //{
        //    VehicleMake vehicleMake = new VehicleMake();
        //    return dbConnection.VehiclesMakes.FindAsync(id);
        //}

        public async Task<IVehicleMake> FindAsync(int? id)
        {            
            var vehicleMake = dbConnection.VehiclesMakes;
            var make = await vehicleMake.FindAsync(id);
            return make;
        }

        //public IEnumerable<IVehicleMake> SelectAll()
        //{
        //    var vehicleMake = dbConnection.VehiclesMakes;
        //    return vehicleMake.ToList();
        //}

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
