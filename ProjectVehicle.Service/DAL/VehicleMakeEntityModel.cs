using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVehicle.Service.DAL
{
    [Table("VehicleMake")]
    public class VehicleMakeEntityModel
    {        
        [Key]
        public int ID { get; set; }
        public string ManufacturerName { get; set; }
        public string MadeIn { get; set; }
        public virtual ICollection<VehicleModelEntityModel> VehicleModels { get; set; }
    }
}
