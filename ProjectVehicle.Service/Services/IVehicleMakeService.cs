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
    public interface IVehicleMakeService : IDisposable
    {
        Task<IPagedList<IVehicleMake>> FindVehicleMakeAsync(IVehicleSorting sort, IVehicleFiltering filter, IVehiclePaging page);      
        Task<bool> ValidateVehicleNameAsync(string manufacturerName);
        Task<IVehicleMake> GetVehicleMakeAsync(int? id);
        Task InsertOrUpdateVehicleMakeAsync(IVehicleMake vehicleMake);
        Task DeleteVehicleMakeAsync(int id);

    }
}
