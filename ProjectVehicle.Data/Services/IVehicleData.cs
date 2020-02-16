using PagedList;
using ProjectVehicle.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVehicle.Data.Services
{
    public interface IVehicleData : IDisposable
    {
        IPagedList<VehicleMake> All(string sortOrder, string searchString, int? page);

        List<VehicleModel> AllModels(int? makeId = null);


        VehicleMake Find(int? id);
        VehicleModel FindModel(int? id);

        void InsertOrUpdate(VehicleMake vehicleMake);
        void InsertOrUpdateModel(VehicleModel vehicleModel);

        void Save();

        void Delete(int id);
        void DeleteModel(int id);
    }
}
