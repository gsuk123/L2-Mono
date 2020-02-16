using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectVehicle.MVC.Models
{
    public class VehicleMakeViewModel
    {
        public int ID { get; set; }
        public string ManufacturerName { get; set; }
        public string MadeIn { get; set; }
        public virtual ICollection<VehicleModelViewModel> VehicleModels { get; set; }
    }
}