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
    public class ServiceDescsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ServiceDescs
        public ActionResult Index()
        {
            var serviceDescs = db.ServiceDescs.Include(s => s.Service);
            return View(serviceDescs.ToList());
        }

        // GET: ServiceDescs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceDesc serviceDesc = db.ServiceDescs.Find(id);
            if (serviceDesc == null)
            {
                return HttpNotFound();
            }
            return View(serviceDesc);
        }

        // GET: ServiceDescs/Create
        public ActionResult Create()
        {
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName");
            return View();
        }

        // POST: ServiceDescs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DescID,CritName,CritCost,SerCode,SerBasic,employees")] ServiceDesc serviceDesc)
        {
            Service sr = new Service();
            if (ModelState.IsValid)
            {
                string str = Request.Params["btn1"];
                if (str == "Review")
                {
                    serviceDesc.SerBasic = serviceDesc.getSerCost();

                }
                else if (str == "Confirm")
                {
                    serviceDesc.SerBasic = serviceDesc.getSerCost();
                    db.ServiceDescs.Add(serviceDesc);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", serviceDesc.SerCode);
            return View(serviceDesc);
        }

        // GET: ServiceDescs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceDesc serviceDesc = db.ServiceDescs.Find(id);
            if (serviceDesc == null)
            {
                return HttpNotFound();
            }
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", serviceDesc.SerCode);
            return View(serviceDesc);
        }

        // POST: ServiceDescs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DescID,CritName,CritCost,SerCode,SerBasic")] ServiceDesc serviceDesc)
        {
            if (ModelState.IsValid)
            {
                string str = Request.Params["btn1"];
                if (str == "Review")
                {
                    serviceDesc.SerBasic = serviceDesc.getSerCost();

                }
                else if (str == "Confirm")
                {
                    db.Entry(serviceDesc).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", serviceDesc.SerCode);
            return View(serviceDesc);
        }

        // GET: ServiceDescs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceDesc serviceDesc = db.ServiceDescs.Find(id);
            if (serviceDesc == null)
            {
                return HttpNotFound();
            }
            return View(serviceDesc);
        }

        // POST: ServiceDescs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceDesc serviceDesc = db.ServiceDescs.Find(id);
            db.ServiceDescs.Remove(serviceDesc);
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
