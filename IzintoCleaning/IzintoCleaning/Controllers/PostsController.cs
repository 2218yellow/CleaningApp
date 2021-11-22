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
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            var post = db.Post.Include(p => p.Admin).Include(p => p.Service);
            return View(post.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.AdminID = new SelectList(db.admin, "AdminID", "AdminName");
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostCode,SerCode,Wage,PostDesc,PostClosingDate,AdminID")] Post post)
        {
            if (ModelState.IsValid)
            {
                string str = Request.Params["btn1"];
                if (str == "Examine")
                {
                    post.Wage = post.getWage();

                }
                else if (str == "Confirm")
                {
                    post.Wage = post.getWage();
                    db.Post.Add(post);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.AdminID = new SelectList(db.admin, "AdminID", "AdminName", post.AdminID);
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", post.SerCode);
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminID = new SelectList(db.admin, "AdminID", "AdminName", post.AdminID);
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", post.SerCode);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostCode,SerCode,Wage,PostDesc,PostClosingDate,AdminID")] Post post)
        {
            if (ModelState.IsValid)
            {
                string str = Request.Params["btn1"];
                if (str == "Examine")
                {
                    post.Wage = post.getWage();

                }
                else if (str == "Confirm")
                {
                    post.Wage = post.getWage();
                    db.Entry(post).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.AdminID = new SelectList(db.admin, "AdminID", "AdminName", post.AdminID);
            ViewBag.SerCode = new SelectList(db.Service, "SerCode", "SerName", post.SerCode);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Post.Find(id);
            db.Post.Remove(post);
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
