using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IzintoCleaning.Models;

namespace IzintoCleaning.Controllers
{
    public class EquipmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Equipments
        public ActionResult Index()
        {
            var equipment = db.Equipment.Include(e => e.admin).Include(e => e.supplier);
            return View(equipment.ToList());
        }

        // GET: Equipments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Equipment.Find(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // GET: Equipments/Create
        public ActionResult Create()
        {
            ViewBag.AdminID = new SelectList(db.admin, "AdminID", "AdminName");
            ViewBag.SuppID = new SelectList(db.Supplier, "SuppID", "SuppName");
            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EquipID,EquipName,EquipType,EquipCost,SuppID,AdminID,QuantityInStock,Picture")] Equipment equipment, HttpPostedFileBase img_upload)
        {
            byte[] data = null;
            data = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(data, 0, img_upload.ContentLength);
            equipment.Picture = data;

            if (ModelState.IsValid)
            {
               

                db.Equipment.Add(equipment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdminID = new SelectList(db.admin, "AdminID", "AdminName", equipment.AdminID);
            ViewBag.SuppID = new SelectList(db.Supplier, "SuppID", "SuppName", equipment.SuppID);
            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Equipment.Find(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminID = new SelectList(db.admin, "AdminID", "AdminName", equipment.AdminID);
            ViewBag.SuppID = new SelectList(db.Supplier, "SuppID", "SuppName", equipment.SuppID);
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EquipID,EquipName,EquipType,EquipCost,SuppID,AdminID,QuantityInStock,Picture")] Equipment equipment, HttpPostedFileBase img_upload)
        {
            byte[] data = null;
            data = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(data, 0, img_upload.ContentLength);
            equipment.Picture = data;

            if (ModelState.IsValid)
            {
                db.Entry(equipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdminID = new SelectList(db.admin, "AdminID", "AdminName", equipment.AdminID);
            ViewBag.SuppID = new SelectList(db.Supplier, "SuppID", "SuppName", equipment.SuppID);
            return View(equipment);
        }

        // GET: Equipments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Equipment.Find(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Equipment equipment = db.Equipment.Find(id);
            db.Equipment.Remove(equipment);
            db.SaveChanges();
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
