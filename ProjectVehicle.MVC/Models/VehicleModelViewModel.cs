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
        [Required]
        public string ModelName { get; set; }
        [Display(Name = "Model year")]
        [Required]
        public int ModelYear { get; set; }
        [Required]
        public string Colour { get; set; }
        [Required]
        public int VehicleMakeID { get; set; }
        public virtual VehicleMake VehicleMake { get; set; }
    }
}