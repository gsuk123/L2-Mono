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
        Task<IPagedList<IVehicleModel>> FindVehicleModelsAsync(IVehicleSorting sort, IVehicleFiltering filter, IVehiclePaging page, int? makeId = null);
        Task<IVehicleModel> GetVehicleModelAsync(int? id);
        Task InsertOrUpdateVehicleModelAsync(IVehicleModel vehicleModel);
        Task DeleteVehicleModelAsync(int id);
        Task<IEnumerable<IVehicleMake>> GetVehiclesMake();
    }
}
