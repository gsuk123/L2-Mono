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

        public JsonResult IsNameAvailable(string ManufacturerName)
        {
            var result = vehicleService.ValidateName(ManufacturerName);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: VehicleMake
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.MadeInSortParm = String.IsNullOrEmpty(sortOrder) ? "madein_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //var vehicles = vehicleService.GetAll(sortOrder, searchString, page);
            var filter = helperFactory.CreateVehicleFiltering();
            filter.Search = searchString;
            var sort = helperFactory.CreateVehicleSorting();
            sort.Sort = sortOrder;
            var paging = helperFactory.CreateVehiclePaging();
            paging.Page = page;


            var vehicles = vehicleService.GetAll(sort, filter, paging);
            var vehiclesViewList = new List<VehicleMakeViewModel>();
            foreach (var v in vehicles)
            {
                VehicleMakeViewModel vehicleMakeVM = mapper.Map<VehicleMakeViewModel>(v);
                vehiclesViewList.Add(vehicleMakeVM);
            }
            var newPagedList = new StaticPagedList<VehicleMakeViewModel>(vehiclesViewList, vehicles.PageNumber, vehicles.PageSize, vehicles.TotalItemCount);
            return View(newPagedList);

        }


        // GET: VehicleMake/Details/5 
        public async Task<ActionResult> Details(int? id)
        {            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var vehicleMake = await vehicleService.FindAsync(id);
            if (vehicleMake == null)
            {
                return HttpNotFound();
            }
            VehicleMakeViewModel vehicleMakeVM = mapper.Map<IVehicleMake, VehicleMakeViewModel>(vehicleMake);
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
            if (String.IsNullOrEmpty(vehicleMakeVM.ManufacturerName))
            {
                ModelState.AddModelError(nameof(vehicleMakeVM.ManufacturerName), "The name is required");
            }
            if (String.IsNullOrEmpty(vehicleMakeVM.MadeIn))
            {
                ModelState.AddModelError(nameof(vehicleMakeVM.MadeIn), "The place of production is required");
            }

            if (ModelState.IsValid)
            {
                var vehicleMake = mapper.Map<VehicleMakeViewModel, VehicleMake>(vehicleMakeVM);
                await vehicleService.InsertOrUpdateAsync(vehicleMake);               
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

            IVehicleMake vehicleMake = await vehicleService.FindAsync(id);
            if (vehicleMake == null) 
            {
                return HttpNotFound();
            }
            VehicleMakeViewModel vehicleMakeVM = mapper.Map<IVehicleMake, VehicleMakeViewModel>(vehicleMake);
            return View(vehicleMakeVM);
        }

        // POST: VehicleMake/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VehicleMakeViewModel vehicleMakeVM)
        {
            if (String.IsNullOrEmpty(vehicleMakeVM.ManufacturerName))
            {
                ModelState.AddModelError(nameof(vehicleMakeVM.ManufacturerName), "The name is required");
            }
            if (String.IsNullOrEmpty(vehicleMakeVM.MadeIn))
            {
                ModelState.AddModelError(nameof(vehicleMakeVM.MadeIn), "The place of production is required");
            }

            if (ModelState.IsValid)
            {
                var vehicleMake = mapper.Map<VehicleMakeViewModel, VehicleMake>(vehicleMakeVM);
                await vehicleService.InsertOrUpdateAsync(vehicleMake);            
                                                  
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

            IVehicleMake vehicleMake = await vehicleService.FindAsync(id);
            if (vehicleMake == null) 
            {
                return HttpNotFound();
            }
            VehicleMakeViewModel vehicleMakeVM = mapper.Map<IVehicleMake, VehicleMakeViewModel>(vehicleMake);
            return View(vehicleMakeVM);
        }

        // POST: VehicleMake/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await vehicleService.DeleteAsync(id);           
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