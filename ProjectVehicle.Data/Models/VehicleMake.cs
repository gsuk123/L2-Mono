using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVehicle.Data.Models
{
    public class VehicleMake
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(255)]        
        public string ManufacturerName { get; set; }
        public string MadeIn { get; set; }
        public virtual ICollection<VehicleModel> VehicleModels { get; set; }
    }
}
