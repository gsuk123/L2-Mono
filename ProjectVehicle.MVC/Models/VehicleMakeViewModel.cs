using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectVehicle.MVC.Models
{
    public class VehicleMakeViewModel
    {
        public int ID { get; set; }
        [Remote("IsNameAvailable", "VehicleMake", ErrorMessage = "Manufacturer name alredy in use.")]
        [Display(Name = "Manufacturer name")]
        public string ManufacturerName { get; set; }
        [Display(Name = "Made in")]
        public string MadeIn { get; set; }
        public virtual ICollection<VehicleModelViewModel> VehicleModels { get; set; }
    }
}