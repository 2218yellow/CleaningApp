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
    public class EquipmentOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EquipmentOrders
        public ActionResult Index()
        {
            return View(db.EquipmentOrders.ToList());
        }

        // GET: EquipmentOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentOrder equipmentOrder = db.EquipmentOrders.Find(id);
            if (equipmentOrder == null)
            {
                return HttpNotFound();
            }
            return View(equipmentOrder);
        }

        // GET: EquipmentOrders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EquipmentOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EquipmentOrderId,qty,amt,creator")] EquipmentOrder equipmentOrder)
        {
            if (ModelState.IsValid)
            {
                db.EquipmentOrders.Add(equipmentOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(equipmentOrder);
        }

        // GET: EquipmentOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentOrder equipmentOrder = db.EquipmentOrders.Find(id);
            if (equipmentOrder == null)
            {
                return HttpNotFound();
            }
            return View(equipmentOrder);
        }

        // POST: EquipmentOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EquipmentOrderId,qty,amt,creator")] EquipmentOrder equipmentOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipmentOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(equipmentOrder);
        }

        // GET: EquipmentOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentOrder equipmentOrder = db.EquipmentOrders.Find(id);
            if (equipmentOrder == null)
            {
                return HttpNotFound();
            }
            return View(equipmentOrder);
        }

        // POST: EquipmentOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EquipmentOrder equipmentOrder = db.EquipmentOrders.Find(id);
            db.EquipmentOrders.Remove(equipmentOrder);
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
