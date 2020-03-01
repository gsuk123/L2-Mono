using AutoMapper;
using PagedList;
using ProjectVehicle.Service.Models;
using ProjectVehicle.Service.Services;
using ProjectVehicle.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProjectVehicle.Service.Common;

namespace ProjectVehicle.MVC.Controllers
{
    public class VehicleModelController : Controller
    {
        private readonly IVehicleModelService vehicleService;
        private readonly IMapper mapper;
        private readonly IHelperFactory helperFactory;

        public VehicleModelController(
            IVehicleModelService vehicleService,
            IMapper mapper,
            IHelperFactory helperFactory)
        {
            this.vehicleService = vehicleService;
            this.mapper = mapper;
            this.helperFactory = helperFactory;
        }

        // GET: VehicleModel
        public async Task<ActionResult> Index(string sortVehicle, string currentFilter, string searchVehicle, int? pageVehicle, int? makeId = null)
        {
            ViewBag.CurrentSort = sortVehicle;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortVehicle) ? "model_desc" : "";
            ViewBag.YearSortParm = sortVehicle == "Year" ? "year_desc" : "Year";
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


            var vehicleModels = await vehicleService.FindVehicleModelsAsync(sort, filter, page, makeId);
            var vehicleList = new List<VehicleModelViewModel>();
            foreach (var v in vehicleModels)
            {
                VehicleModelViewModel vehicleModelVM = mapper.Map<VehicleModelViewModel>(v);                
                vehicleList.Add(vehicleModelVM);
            }

            var vehiclePagedList = new StaticPagedList<VehicleModelViewModel>(vehicleList, vehicleModels.PageNumber, vehicleModels.PageSize, vehicleModels.TotalItemCount);
            return View(vehiclePagedList);
        }

        //GET: VehicleModel/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var vehicleModel = await vehicleService.GetVehicleModelAsync(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            VehicleModelViewModel vehicleModelVM = mapper.Map<VehicleModelViewModel>(vehicleModel);
            return View(vehicleModelVM);
        }

        // GET: VehicleModel/Create 
        public async Task<ActionResult>Create()
        {            
            ViewBag.VehicleMakeID =  new SelectList( await vehicleService.GetVehiclesMake(), "ID", "ManufacturerName");
            return View();            
        }

        // POST: VehicleModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VehicleModelViewModel vehicleModelVM)
        {
            if (ModelState.IsValid)
            {
                var vehicleModel = mapper.Map<IVehicleModel>(vehicleModelVM);
                await vehicleService.InsertOrUpdateVehicleModelAsync(vehicleModel);
                return RedirectToAction("Index");
            }

            ViewBag.VehicleMakeID = new SelectList(await vehicleService.GetVehiclesMake(), "ID", "ManufacturerName");
            return View();
        }

        // GET: VehicleModel/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var vehicleModel = await vehicleService.GetVehicleModelAsync(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            VehicleModelViewModel vehicleModelVM = mapper.Map<VehicleModelViewModel>(vehicleModel);            
            return View(vehicleModelVM);
        }

        // POST: VehicleModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VehicleModelViewModel vehicleModelVM)
        {
            if (ModelState.IsValid)
            {
                var vehicleModel = mapper.Map<IVehicleModel>(vehicleModelVM);
                await vehicleService.InsertOrUpdateVehicleModelAsync(vehicleModel);
                return RedirectToAction("Index");
            }            
            return View();
        }

        // GET: VehicleModel/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var vehicleModel = await vehicleService.GetVehicleModelAsync(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            VehicleModelViewModel vehicleModelVM = mapper.Map<VehicleModelViewModel>(vehicleModel);
            return View(vehicleModelVM);
        }

        // POST: VehicleModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await vehicleService.DeleteVehicleModelAsync(id);            
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