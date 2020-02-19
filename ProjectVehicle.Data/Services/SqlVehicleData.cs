using PagedList;
using ProjectVehicle.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web.Mvc;


namespace ProjectVehicle.Data.Services
{
    public class SqlVehicleData : IVehicleData
    {
        readonly VehicleContext db;

        public SqlVehicleData(VehicleContext db)
        {
            this.db = db;
        }

        public bool ValidateName(string ManufacturerName)
        {            
            var result = !db.VehiclesMakes.Any(name => name.ManufacturerName == ManufacturerName);
            return result;
        }

        public IPagedList<VehicleMake> All(string sortOrder, string searchString, int? page)
        {
            var vehicles = db.VehiclesMakes.Select(v => v);

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
            int pageNumber = (page ?? 1); 


            return vehicles.ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<VehicleModel> AllModels(string sortOrder, string searchString, int? page, int? makeId = null)
        {
            var vehicleModels = db.VehiclesModels.Include(v => v.VehicleMake);

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



        public Task<VehicleMake> FindAsync(int? id)
        {
            VehicleMake vehicleMake = new VehicleMake();            
            return db.VehiclesMakes.FindAsync(id);
        }

        public List<VehicleMake> SelectAll()
        {
            var vehicleMake = db.VehiclesMakes;
            return vehicleMake.ToList();
        }

        public Task<VehicleModel> FindModelAsync(int? id)
        {
            VehicleModel vehicleModel = new VehicleModel();
            return db.VehiclesModels.FindAsync(id);
        }

        public Task InsertOrUpdateAsync(VehicleMake vehicleMake)
        {
            if(vehicleMake.ID == default)
            {
                db.VehiclesMakes.Add(vehicleMake);
                return db.SaveChangesAsync();
            }
            else
            {
                db.Entry(vehicleMake).State = System.Data.Entity.EntityState.Modified;
                return db.SaveChangesAsync();
            }
        }

        public Task InsertOrUpdateModelAsync(VehicleModel vehicleModel)
        {
            if(vehicleModel.VehicleModelID == default)
            {
                db.VehiclesModels.Add(vehicleModel);
                return db.SaveChangesAsync();
            }
            else
            {
                db.Entry(vehicleModel).State = System.Data.Entity.EntityState.Modified;
                return db.SaveChangesAsync();
            }
        }



        public Task DeleteAsync(int id)
        {
            var vehicleMake = db.VehiclesMakes.Find(id);
            db.VehiclesMakes.Remove(vehicleMake);
            return db.SaveChangesAsync();
        }
        public Task DeleteModelAsync(int id)
        {
            var vehicleModel = db.VehiclesModels.Find(id);
            db.VehiclesModels.Remove(vehicleModel);
            return db.SaveChangesAsync();
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
