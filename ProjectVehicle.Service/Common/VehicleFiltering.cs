using ProjectVehicle.Service.Models;
using ProjectVehicle.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVehicle.Service.Common
{

    public class VehicleFiltering : IVehicleFiltering
    {        
        public string Filter { get; set; }
    }
}

