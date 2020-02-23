using ProjectVehicle.Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectVehicle.MVC.Models
{
    public class VehicleModelViewModel
    {
        public int VehicleModelID { get; set; }
        [Display(Name = "Model name")]
        public string ModelName { get; set; }
        [Display(Name = "Model year")]
        public int ModelYear { get; set; }        
        public string Colour { get; set; }
        public int VehicleMakeID { get; set; }
        public virtual VehicleMake VehicleMake { get; set; }
    }
}