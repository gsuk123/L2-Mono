using AutoMapper;
using PagedList;
using ProjectVehicle.Service.Common;
using ProjectVehicle.Service.DAL;
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
        readonly IMapper mapper;
        public VehicleMakeService(VehicleContext dbConnection, IMapper mapper)
        {
            this.dbConnection = dbConnection;
            this.mapper = mapper;
        }

        public Task<bool> ValidateVehicleNameAsync(string manufacturerName)
        {
            var result = !dbConnection.VehiclesMakes.Any(name => name.ManufacturerName == manufacturerName);
            return Task.FromResult(result);
            
        }

        
        public Task<IPagedList<IVehicleMake>> FindVehicleMakeAsync(IVehicleSorting sort, IVehicleFiltering filter, IVehiclePaging page)
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

            var mappedList = mapper.Map<List<VehicleMakeEntityModel>, List<IVehicleMake>>(vehicles.ToList());

            var pagedList = mappedList.ToPagedList(page.Page ?? 1, 3);            

            return Task.FromResult(pagedList);
            
        }


        public async Task<IVehicleMake> GetVehicleMakeAsync(int? id)
        {            
            var vehicleMakes = dbConnection.VehiclesMakes;
            var vehicleMake = await vehicleMakes.FindAsync(id);            
            IVehicleMake vehicle = mapper.Map<IVehicleMake>(vehicleMake);            
            return vehicle;
        }

        public Task InsertOrUpdateVehicleMakeAsync(IVehicleMake vehicleMake)
        {
            VehicleMakeEntityModel vehicleMakeEM = mapper.Map<VehicleMakeEntityModel>(vehicleMake);
            if (vehicleMakeEM.ID == default)
            {
                dbConnection.VehiclesMakes.Add(vehicleMakeEM);
                return dbConnection.SaveChangesAsync();
            }
            else
            {
                dbConnection.Entry(vehicleMakeEM).State = System.Data.Entity.EntityState.Modified;
                return dbConnection.SaveChangesAsync();
            }
        }

        public Task DeleteVehicleMakeAsync(int id)
        {
            var vehicleMakeEM = dbConnection.VehiclesMakes.Find(id);
            dbConnection.VehiclesMakes.Remove(vehicleMakeEM);
            return dbConnection.SaveChangesAsync();
        }

        public void Dispose()
        {
            dbConnection.Dispose();
        }
    }
}
