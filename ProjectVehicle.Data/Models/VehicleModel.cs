using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVehicle.Data.Models
{
    public class VehicleModel
    {
        public int VehicleModelID { get; set; }
        public string ModelName { get; set; }
        public int ModelYear { get; set; }
        public string Colour { get; set; }
        public int VehicleMakeID { get; set; }
        public virtual VehicleMake VehicleMake { get; set; }
        
    }
}
