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
        IPagedList<VehicleMake> GetAll(IVehicleSorting sort, IVehicleFiltering search, IVehiclePaging page);
        //IPagedList<VehicleMake> GetAll(string sortOrder, string searchString, int? page);
        bool ValidateName(string ManufacturerName);
        Task<IVehicleMake> FindAsync(int? id);
        Task InsertOrUpdateAsync(VehicleMake vehicleMake);
        Task DeleteAsync(int id);

    }
}
