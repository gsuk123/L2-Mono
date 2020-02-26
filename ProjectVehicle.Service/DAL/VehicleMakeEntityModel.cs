using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVehicle.Service.DAL
{
    public class VehicleMakeEntityModel
    {
        public int ID { get; set; }
        public string ManufacturerName { get; set; }
        public string MadeIn { get; set; }
        public virtual ICollection<VehicleModelEntityModel> VehicleModels { get; set; }
    }
}
