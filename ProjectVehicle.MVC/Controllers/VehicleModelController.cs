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


namespace ProjectVehicle.MVC.Controllers
{
    public class VehicleModelController : Controller
    {
        private readonly IVehicleModelService vehicleService;
        private readonly IMapper mapper;

        public VehicleModelController(
            IVehicleModelService vehicleService,
            IMapper mapper)
        {
            this.vehicleService = vehicleService;
            this.mapper = mapper;
        }

        // GET: VehicleModel
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? makeId = null)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "model_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;



            var vehicleModels = vehicleService.AllModels(sortOrder, searchString, page, makeId);
            var vehicleViewModelList = new List<VehicleModelViewModel>();
            foreach (var v in vehicleModels)
            {
                VehicleModelViewModel vehicleModelVM = mapper.Map<VehicleModelViewModel>(v);
                vehicleViewModelList.Add(vehicleModelVM);
            }

            var newPagedList = new StaticPagedList<VehicleModelViewModel>(vehicleViewModelList, vehicleModels.PageNumber, vehicleModels.PageSize, vehicleModels.TotalItemCount);
            return View(newPagedList);


        }

        //GET: VehicleModel/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel = await vehicleService.FindModelAsync(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            VehicleModelViewModel vehicleModelVM = mapper.Map<VehicleModel, VehicleModelViewModel>(vehicleModel);
            return View(vehicleModelVM);
        }

        // GET: VehicleModel/Create 
        public ActionResult Create()
        {            
            ViewBag.VehicleMakeID = new SelectList(vehicleService.SelectAll(), "ID", "ManufacturerName");
            return View();            
        }

        // POST: VehicleModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VehicleModelViewModel vehicleModelVM)
        {
            if (ModelState.IsValid)
            {
                var vehicleModel = mapper.Map<VehicleModelViewModel, VehicleModel>(vehicleModelVM);
                await vehicleService.InsertOrUpdateModelAsync(vehicleModel);
                return RedirectToAction("Index");
            }            
            
            return View();
        }

        // GET: VehicleModel/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel =await vehicleService.FindModelAsync(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            VehicleModelViewModel vehicleModelVM = mapper.Map<VehicleModel, VehicleModelViewModel>(vehicleModel);            
            return View(vehicleModelVM);
        }

        // POST: VehicleModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VehicleModelViewModel vehicleModelVM)
        {
            if (ModelState.IsValid)
            {
                var vehicleModel = mapper.Map<VehicleModelViewModel, VehicleModel>(vehicleModelVM);
                await vehicleService.InsertOrUpdateModelAsync(vehicleModel);
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
            VehicleModel vehicleModel =await vehicleService.FindModelAsync(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            VehicleModelViewModel vehicleModelVM = mapper.Map<VehicleModel, VehicleModelViewModel>(vehicleModel);
            return View(vehicleModelVM);
        }

        // POST: VehicleModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await vehicleService.DeleteModelAsync(id);            
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