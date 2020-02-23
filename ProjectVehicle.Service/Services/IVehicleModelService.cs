using PagedList;
using ProjectVehicle.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVehicle.Service.Services
{
    public interface IVehicleModelService : IDisposable
    {
        IPagedList<VehicleModel> AllModels(string sortOrder, string searchString, int? page, int? makeId = null);
        Task<VehicleModel> FindModelAsync(int? id);
        Task InsertOrUpdateModelAsync(VehicleModel vehicleModel);
        Task DeleteModelAsync(int id);
        IEnumerable<IVehicleMake> SelectAll();
    }
}
