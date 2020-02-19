using PagedList;
using ProjectVehicle.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProjectVehicle.Data.Services
{
    public interface IVehicleData : IDisposable
    {
        IPagedList<VehicleMake> All(string sortOrder, string searchString, int? page);

        bool ValidateName(string ManufacturerName);
        
        IPagedList<VehicleModel> AllModels(string sortOrder, string searchString, int? page, int? makeId = null);

        List<VehicleMake> SelectAll();

        Task<VehicleMake> FindAsync(int? id);
        Task<VehicleModel> FindModelAsync(int? id);

        Task InsertOrUpdateAsync(VehicleMake vehicleMake);
        Task InsertOrUpdateModelAsync(VehicleModel vehicleModel);

        Task DeleteAsync(int id);
        Task DeleteModelAsync(int id);
    }
}
