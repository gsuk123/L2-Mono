using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVehicle.Service.DAL
{
    public class VehicleModelEntityModel
    {
        public int VehicleModelID { get; set; }
        public string ModelName { get; set; }
        public int ModelYear { get; set; }
        public string Colour { get; set; }
        public int VehicleMakeID { get; set; }
        public virtual VehicleMakeEntityModel VehicleMakeEntityModel { get; set; }
    }
}
