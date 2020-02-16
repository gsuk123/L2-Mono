using PagedList;
using ProjectVehicle.Data.Models;
using ProjectVehicle.Data.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ProjectVehicle.MVC.Models;

namespace ProjectVehicle.MVC.Controllers
{
    public class VehicleMakeController : Controller
    {
        private readonly IVehicleData db;
        private readonly IMapper mapper;
        public VehicleMakeController(
            IVehicleData db,
            IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
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

            var vehicles = db.All(sortOrder, searchString, page);
            var vehiclesViewList = new List<VehicleMakeViewModel>();
            foreach (var v in vehicles)
            {
                VehicleMakeViewModel vehicleMakeVM = mapper.Map<VehicleMakeViewModel>(v);
                vehiclesViewList.Add(vehicleMakeVM);
            }
            var newPagedList = new StaticPagedList<VehicleMakeViewModel>(vehiclesViewList, vehicles.PageNumber, vehicles.PageSize, vehicles.TotalItemCount);
            return View(newPagedList);

        }


        // GET: VehicleMake/Details/5 Sync method
        public ActionResult Details(int? id)
        {            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            VehicleMake vehicleMake = db.Find(id);
            if (vehicleMake == null)
            {
                return HttpNotFound();
            }
            VehicleMakeViewModel vehicleMakeVM = mapper.Map<VehicleMake, VehicleMakeViewModel>(vehicleMake);
            return View(vehicleMakeVM);
        }

        // GET: VehicleMake/Create 
        //Just getting the form that will allow user to create a VehicleMake
        public ActionResult Create()
        {
            return View();
        }

        // POST: VehicleMake/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VehicleMakeViewModel vehicleMakeVM)
        {           
            if (String.IsNullOrEmpty(vehicleMakeVM.ManufacturerName))
            {
                ModelState.AddModelError(nameof(vehicleMakeVM.ManufacturerName), "The name is required");
            }


            if (ModelState.IsValid)
            {
                var vehicleMake = mapper.Map<VehicleMakeViewModel, VehicleMake>(vehicleMakeVM);
                db.InsertOrUpdate(vehicleMake);
                db.Save();
                return RedirectToAction("Index");
            }
            return View();

        }

        // GET: VehicleMake/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) //VehicleMake/Edit/
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VehicleMake vehicleMake = db.Find(id);
            if (vehicleMake == null) //VehicleMake/Edit/0
            {
                return HttpNotFound();
            }
            VehicleMakeViewModel vehicleMakeVM = mapper.Map<VehicleMake, VehicleMakeViewModel>(vehicleMake);
            return View(vehicleMakeVM);
        }

        // POST: VehicleMake/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VehicleMakeViewModel vehicleMakeVM)
        {
            if (String.IsNullOrEmpty(vehicleMakeVM.ManufacturerName))
            {
                ModelState.AddModelError(nameof(vehicleMakeVM.ManufacturerName), "The name is required");
            }

            if (ModelState.IsValid)
            {
                var vehicleMake = mapper.Map<VehicleMakeViewModel, VehicleMake>(vehicleMakeVM);
                db.InsertOrUpdate(vehicleMake);                   
                db.Save();                                     
                return RedirectToAction("Index");
            }
            return View();// if there is error we want to stay on same view to see what are model validations errors
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) //VehicleMake/Edit/
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VehicleMake vehicleMake = db.Find(id);
            if (vehicleMake == null) //VehicleMake/Edit/0
            {
                return HttpNotFound();
            }
            VehicleMakeViewModel vehicleMakeVM = mapper.Map<VehicleMake, VehicleMakeViewModel>(vehicleMake);
            return View(vehicleMakeVM);
        }

        // POST: VehicleMake/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Delete(id);
            db.Save();
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