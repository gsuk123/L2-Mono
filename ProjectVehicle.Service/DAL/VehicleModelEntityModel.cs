using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVehicle.Service.DAL
{
    [Table("VehicleModel")]
    public class VehicleModelEntityModel
    {
        [Key]
        public int VehicleModelID { get; set; }
        public string ModelName { get; set; }
        public int ModelYear { get; set; }
        public string Colour { get; set; }         
        public int VehicleMakeID { get; set; }   
        [ForeignKey("VehicleMakeID")]
        public virtual VehicleMakeEntityModel VehicleMake { get; set; }
    }
}
