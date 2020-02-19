using AutoMapper;
using PagedList;
using ProjectVehicle.Data.Models;
using ProjectVehicle.Data.Services;
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
        private readonly IVehicleData db;
        private readonly IMapper mapper;

        public VehicleModelController(
            IVehicleData db,
            IMapper mapper)
        {
            this.db = db;
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


            //var vehicleModels = db.AllModels(makeId);
            var vehicleModels = db.AllModels(sortOrder, searchString, page, makeId);
            var vehicleViewModelList = new List<VehicleModelViewModel>();
            foreach(var v in vehicleModels)
            {
                VehicleModelViewModel vehicleModelVM = mapper.Map<VehicleModelViewModel>(v);
                vehicleViewModelList.Add(vehicleModelVM);
            }
            
            var newPagedList = new StaticPagedList<VehicleModelViewModel>(vehicleViewModelList, vehicleModels.PageNumber, vehicleModels.PageSize, vehicleModels.TotalItemCount);
            return View(newPagedList);
            //return View(vehicleViewModelList.ToList());

        }

        //GET: VehicleModel/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel = await db.FindModelAsync(id);
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
            ViewBag.VehicleMakeID = new SelectList(db.SelectAll(), "ID", "ManufacturerName");
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
                await db.InsertOrUpdateModelAsync(vehicleModel);
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
            VehicleModel vehicleModel =await db.FindModelAsync(id);
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
                await db.InsertOrUpdateModelAsync(vehicleModel);
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
            VehicleModel vehicleModel =await db.FindModelAsync(id);
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
            await db.DeleteModelAsync(id);            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}