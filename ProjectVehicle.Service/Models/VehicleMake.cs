using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVehicle.Service.Models
{
    
    public interface IVehicleMake 
    { 
        int ID { get; set; }
        string ManufacturerName { get; set; }
        string MadeIn { get; set; }        

    }

    public class VehicleMake : IVehicleMake
    {
        public int ID { get; set; }      
        public string ManufacturerName { get; set; }
        public string MadeIn { get; set; }
        public IEnumerable<IVehicleModel> VehicleModels { get; set; }
    }
}
