using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectVehicle.MVC.Models
{
    public interface IVehicleMakeViewModel
    {
        int ID { get; set; }
        string ManufacturerName { get; set; }
        string MadeIn { get; set; }
    }

    public class VehicleMakeViewModel : IVehicleMakeViewModel
    {
        public int ID { get; set; }
        [Remote("IsNameAvailable", "VehicleMake", ErrorMessage = "Manufacturer name alredy in use.")]
        [Required]
        [Display(Name = "Manufacturer name")]
        public string ManufacturerName { get; set; }
        [Display(Name = "Made in")]
        [Required]
        public string MadeIn { get; set; }
        public virtual ICollection<VehicleModelViewModel> VehicleModels { get; set; }
    }
}