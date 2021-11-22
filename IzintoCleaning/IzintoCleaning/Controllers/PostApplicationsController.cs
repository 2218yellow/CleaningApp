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
    public class PostApplicationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PostApplications
        public ActionResult Index()
        {
            var postApplication = db.PostApplication.Include(p => p.applicant).Include(p => p.post);
            return View(postApplication.ToList());
        }

        // GET: PostApplications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostApplication postApplication = db.PostApplication.Find(id);
            if (postApplication == null)
            {
                return HttpNotFound();
            }
            return View(postApplication);
        }

        // GET: PostApplications/Create
        public ActionResult Create()
        {
            ViewBag.AppID = new SelectList(db.Applicant, "AppID", "AppEmail");
            ViewBag.PostCode = new SelectList(db.Post, "PostCode", "PostDesc");
            return View();
        }

        // POST: PostApplications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostAppID,AppID,PostCode,postDescription,wage,ClosingDate")] PostApplication postApplication)
        {
            if (ModelState.IsValid)
            {
                string str = Request.Params["btn1"];
                if (str == "Examine")
                {
                    postApplication.postDescription = postApplication.getDescription();
                    postApplication.wage = postApplication.getWage();
                    postApplication.ClosingDate = postApplication.getDate();
                }
                else if (str == "Confirm")
                {
                    postApplication.postDescription = postApplication.getDescription();
                    postApplication.wage = postApplication.getWage();
                    postApplication.ClosingDate = postApplication.getDate();
                    db.PostApplication.Add(postApplication);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.AppID = new SelectList(db.Applicant, "AppID", "AppEmail", postApplication.AppID);
            ViewBag.PostCode = new SelectList(db.Post, "PostCode", "PostDesc", postApplication.PostCode);
            return View(postApplication);
        }

        // GET: PostApplications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostApplication postApplication = db.PostApplication.Find(id);
            if (postApplication == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppID = new SelectList(db.Applicant, "AppID", "AppEmail", postApplication.AppID);
            ViewBag.PostCode = new SelectList(db.Post, "PostCode", "PostDesc", postApplication.PostCode);
            return View(postApplication);
        }

        // POST: PostApplications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostAppID,AppID,PostCode,postDescription,wage,ClosingDate")] PostApplication postApplication)
        {
            if (ModelState.IsValid)
            {
                string str = Request.Params["btn1"];
                if (str == "Confirm")
                {
                    postApplication.postDescription = postApplication.getDescription();
                    postApplication.wage = postApplication.getWage();
                    postApplication.ClosingDate = postApplication.getDate();
                    db.Entry(postApplication).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.AppID = new SelectList(db.Applicant, "AppID", "AppEmail", postApplication.AppID);
            ViewBag.PostCode = new SelectList(db.Post, "PostCode", "PostDesc", postApplication.PostCode);
            return View(postApplication);
        }

        // GET: PostApplications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostApplication postApplication = db.PostApplication.Find(id);
            if (postApplication == null)
            {
                return HttpNotFound();
            }
            return View(postApplication);
        }

        // POST: PostApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostApplication postApplication = db.PostApplication.Find(id);
            db.PostApplication.Remove(postApplication);
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
