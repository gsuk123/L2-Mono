using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ProjectVehicle.MVC.Models;
using System.Threading.Tasks;
using ProjectVehicle.Service.Services;
using ProjectVehicle.Service.Models;
using ProjectVehicle.Service.Common;

namespace ProjectVehicle.MVC.Controllers
{
    public class VehicleMakeController : Controller
    {
        private readonly IVehicleMakeService vehicleService;
        private readonly IMapper mapper;
        private readonly IHelperFactory helperFactory;
        
        public VehicleMakeController(
            IVehicleMakeService vehicleService,
            IMapper mapper,
            IHelperFactory helperFactory)
            
        {
            this.vehicleService = vehicleService;
            this.mapper = mapper;
            this.helperFactory = helperFactory;            
        }

        public async Task<JsonResult> IsNameAvailable(string manufacturerName)
        {
            var result = await vehicleService.ValidateVehicleNameAsync(manufacturerName);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: VehicleMake
        public async Task<ActionResult> Index(string sortVehicle, string currentFilter, string searchVehicle, int? pageVehicle)
        {
            ViewBag.CurrentSort = sortVehicle;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortVehicle) ? "name_desc" : "";
            ViewBag.MadeInSortParm = String.IsNullOrEmpty(sortVehicle) ? "madein_desc" : "";
            if (searchVehicle != null)
            {
                pageVehicle = 1;
            }
            else
            {
                searchVehicle = currentFilter;
            }

            ViewBag.CurrentFilter = searchVehicle;

            
            var filter = helperFactory.CreateVehicleFiltering();
            filter.Filter = searchVehicle;
            var sort = helperFactory.CreateVehicleSorting();
            sort.Sort = sortVehicle;
            var page = helperFactory.CreateVehiclePaging();
            page.Page = pageVehicle;


            
            var vehicles = await vehicleService.FindVehicleMakeAsync(sort, filter, page);
            var vehiclesList = new List<VehicleMakeViewModel>();
            foreach (var v in vehicles)
            {
                VehicleMakeViewModel vehicleMakeVM = mapper.Map<VehicleMakeViewModel>(v);
                vehiclesList.Add(vehicleMakeVM);
            }
            var vehiclePagedList = new StaticPagedList<VehicleMakeViewModel>(vehiclesList, vehicles.PageNumber, vehicles.PageSize, vehicles.TotalItemCount);
            return View(vehiclePagedList);

        }


        // GET: VehicleMake/Details/5 
        public async Task<ActionResult> Details(int? id)
        {            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var vehicleMake = await vehicleService.GetVehicleMakeAsync(id);
            if (vehicleMake == null)
            {
                return HttpNotFound();
            }

            VehicleMakeViewModel vehicleMakeVM = mapper.Map<VehicleMakeViewModel>(vehicleMake);
            return View(vehicleMakeVM);
        }

        // GET: VehicleMake/Create         
        public ActionResult Create()
        {
            return View();
        }

        // POST: VehicleMake/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VehicleMakeViewModel vehicleMakeVM)
        {           


            if (ModelState.IsValid)
            {
                var vehicleMake = mapper.Map<IVehicleMake>(vehicleMakeVM);
                await vehicleService.InsertOrUpdateVehicleMakeAsync(vehicleMake);               
                return RedirectToAction("Index");
            }
            return View();

        }

        // GET: VehicleMake/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IVehicleMake vehicleMake = await vehicleService.GetVehicleMakeAsync(id);

            if (vehicleMake == null) 
            {
                return HttpNotFound();
            }
            VehicleMakeViewModel vehicleMakeVM = mapper.Map<VehicleMakeViewModel>(vehicleMake);
            return View(vehicleMakeVM);
        }

        // POST: VehicleMake/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VehicleMakeViewModel vehicleMakeVM)
        {

            if (ModelState.IsValid)
            {
                var vehicleMake = mapper.Map<IVehicleMake>(vehicleMakeVM);
                await vehicleService.InsertOrUpdateVehicleMakeAsync(vehicleMake);            
                                                  
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IVehicleMake vehicleMake = await vehicleService.GetVehicleMakeAsync(id);
            if (vehicleMake == null) 
            {
                return HttpNotFound();
            }
            VehicleMakeViewModel vehicleMakeVM = mapper.Map<VehicleMakeViewModel>(vehicleMake);
            return View(vehicleMakeVM);
        }

        // POST: VehicleMake/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await vehicleService.DeleteVehicleMakeAsync(id);           
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                vehicleService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}