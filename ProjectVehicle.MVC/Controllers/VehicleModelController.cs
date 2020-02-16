using ProjectVehicle.Data.Models;
using ProjectVehicle.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProjectVehicle.MVC.Controllers
{
    public class VehicleModelController : Controller
    {
        private readonly IVehicleData db;

        public VehicleModelController(IVehicleData db)
        {
            this.db = db;
        }



        // GET: VehicleModel
        public ActionResult Index(int? makeId = null)
        {

            var vehicleModels = db.AllModels(makeId);

            return View(vehicleModels.ToList());


        }

        //GET: VehicleModel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel = db.FindModel(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            return View(vehicleModel);
        }

        // GET: VehicleModel/Create 
        //Just getting the form that will allow user to create a VehicleModel
        public ActionResult Create()
        {
            //ViewBag.VehicleMakeID = new SelectList(db.VehicleMakes, "ID", "ManufacturerName");
            return View();            
        }

        // POST: VehicleModel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VehicleModel vehicleModel)
        {
            if (ModelState.IsValid)
            {
                db.InsertOrUpdateModel(vehicleModel);
                db.Save();
                return RedirectToAction("Index");
            }

            //ViewBag.VehicleMakeID = new SelectList(db.VehicleMakes, "ID", "ManufacturerName", vehicleModel.VehicleMakeID);
            return View(vehicleModel);
        }

        // GET: VehicleModel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel = db.FindModel(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            //ViewBag.VehicleMakeID = new SelectList(db.VehicleMakes, "ID", "ManufacturerName", vehicleModel.VehicleMakeID);
            return View(vehicleModel);
        }

        // POST: VehicleModel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VehicleModelID,ModelName,ModelYear,Colour,VehicleMakeID")] VehicleModel vehicleModel)
        {
            if (ModelState.IsValid)
            {
                db.InsertOrUpdateModel(vehicleModel);
                db.Save();
                return RedirectToAction("Index");
            }
            //ViewBag.VehicleMakeID = new SelectList(db.VehicleMakes, "ID", "ManufacturerName", vehicleModel.VehicleMakeID);
            return View(vehicleModel);
        }

        // GET: VehicleModel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel = db.FindModel(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            return View(vehicleModel);
        }

        // POST: VehicleModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.DeleteModel(id);
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