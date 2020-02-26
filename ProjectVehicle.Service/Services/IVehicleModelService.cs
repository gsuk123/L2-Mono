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
    public interface IVehicleModelService : IDisposable
    {
        Task<IPagedList<VehicleModel>> GetAllModelsAsync(IVehicleSorting sort, IVehicleFiltering filter, IVehiclePaging page, int? makeId = null);
        Task<IVehicleModel> FindModelAsync(int? id);
        Task InsertOrUpdateModelAsync(VehicleModel vehicleModel);
        Task DeleteModelAsync(int id);
        IEnumerable<IVehicleMake> SelectAll();
    }
}
