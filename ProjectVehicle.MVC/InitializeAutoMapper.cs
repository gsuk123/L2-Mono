using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ProjectVehicle.Data.Models;
using ProjectVehicle.MVC.Models;

namespace ProjectVehicle.MVC
{
    public class InitializeAutoMapper
    {
        public static void Initialize()
        {
            CreateModelsToViewModels(); 
        }
        private static void CreateModelsToViewModels()
        {
            //Create a map
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<VehicleMake, VehicleMakeViewModel>();
            });
            IMapper mapper = config.CreateMapper();
        }
    }
}