using PagedList;
using ProjectVehicle.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ProjectVehicle.Data.Services
{
    public class SqlVehicleData : IVehicleData
    {
        readonly VehicleContext db;

        public SqlVehicleData(VehicleContext db)
        {
            this.db = db;
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
            int pageNumber = (page ?? 1); //if page is null then value is 1


            return vehicles.ToPagedList(pageNumber, pageSize);
        }

        public List<VehicleModel> AllModels(int? makeId = null)
        {
            var vehicleModels = db.VehiclesModels.Include(v => v.VehicleMake);


            if (makeId.HasValue)
            {
                vehicleModels = vehicleModels.Where(m => m.VehicleMakeID == makeId);

            }
            return vehicleModels.ToList();
        }

        public VehicleMake Find(int? id)
        {
            VehicleMake vehicleMake = new VehicleMake();
            vehicleMake = db.VehiclesMakes.Where(v => v.ID == id).FirstOrDefault();
            return vehicleMake;
        }
        public VehicleModel FindModel(int? id)
        {
            VehicleModel vehicleModel = new VehicleModel();
            vehicleModel = db.VehiclesModels.Where(m => m.VehicleModelID == id).FirstOrDefault();
            return vehicleModel;
        }

        public void InsertOrUpdate(VehicleMake vehicleMake)
        {
            if(vehicleMake.ID == default)
            {
                db.VehiclesMakes.Add(vehicleMake);
            }
            else
            {
                db.Entry(vehicleMake).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void InsertOrUpdateModel(VehicleModel vehicleModel)
        {
            if(vehicleModel.VehicleModelID == default)
            {
                db.VehiclesModels.Add(vehicleModel);
            }
            else
            {
                db.Entry(vehicleModel).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var vehicleMake = db.VehiclesMakes.Find(id);
            db.VehiclesMakes.Remove(vehicleMake);
        }
        public void DeleteModel(int id)
        {
            var vehicleModel = db.VehiclesModels.Find(id);
            db.VehiclesModels.Remove(vehicleModel);
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
