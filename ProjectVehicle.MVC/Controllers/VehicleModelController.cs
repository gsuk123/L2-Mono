using AutoMapper;
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
        public ActionResult Index(int? makeId = null)
        {

            var vehicleModels = db.AllModels(makeId);
            var vehicleViewModelList = new List<VehicleModelViewModel>();
            foreach(var v in vehicleModels)
            {
                VehicleModelViewModel vehicleModelVM = mapper.Map<VehicleModelViewModel>(v);
                vehicleViewModelList.Add(vehicleModelVM);
            }
            
            
            return View(vehicleViewModelList.ToList());

        }

        //GET: VehicleModel/Details/5
        public async Task<ActionResult> Details(int? id)
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
                //db.Save();
                return RedirectToAction("Index");
            }

            //ViewBag.VehicleMakeID = new SelectList(db.VehicleMakes, "ID", "ManufacturerName", vehicleModel.VehicleMakeID);
            
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
            //ViewBag.VehicleMakeID = new SelectList(db.VehicleMakes, "ID", "ManufacturerName", vehicleModel.VehicleMakeID);
            return View(vehicleModelVM);
        }

        // POST: VehicleModel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VehicleModelViewModel vehicleModelVM)
        {
            if (ModelState.IsValid)
            {
                var vehicleModel = mapper.Map<VehicleModelViewModel, VehicleModel>(vehicleModelVM);
               await db.InsertOrUpdateModelAsync(vehicleModel);
                //db.Save();
                return RedirectToAction("Index");
            }
            //ViewBag.VehicleMakeID = new SelectList(db.VehicleMakes, "ID", "ManufacturerName", vehicleModel.VehicleMakeID);
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
            //db.Save();
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