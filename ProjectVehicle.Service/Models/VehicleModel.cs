using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVehicle.Service.Models
{
    public interface IVehicleModel 
    {
        int VehicleModelID { get; set; }
        string ModelName { get; set; }
        int ModelYear { get; set; }
        string Colour { get; set; }
        int VehicleMakeID { get; set; }
        IVehicleMake VehicleMake { get; set; }
    }

    public class VehicleModel : IVehicleModel
    {
        public int VehicleModelID { get; set; }
        public string ModelName { get; set; }
        public int ModelYear { get; set; }
        public string Colour { get; set; }
        public int VehicleMakeID { get; set; }
        public IVehicleMake VehicleMake { get; set; }
        
    }
}
