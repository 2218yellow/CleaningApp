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
    public class ApplicantsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Applicants
        public ActionResult Index()
        {
            return View(db.Applicant.ToList());
        }

        // GET: Applicants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicant = db.Applicant.Include(s => s.Files).SingleOrDefault(s => s.AppID == id);
            if (applicant == null)
            {
                return HttpNotFound();
            }
            return View(applicant);
        }

        // GET: Applicants/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Applicants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppID,AppName,AppSurname,AppEmail,AppPhone,AppAdd,Picture")] Applicant applicant, HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var avatar = new Models.File
                {
                    FileName = System.IO.Path.GetFileName(upload.FileName),
                    FileType = FileType.Avatar,
                    ContentType = upload.ContentType
                };
                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    avatar.Content = reader.ReadBytes(upload.ContentLength);
                }
                applicant.Files = new List<Models.File> { avatar };
            }
            if (ModelState.IsValid)
            {
                db.Applicant.Add(applicant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicant);
        }

        // GET: Applicants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicant = db.Applicant.Include(s => s.Files).SingleOrDefault(s => s.AppID == id);
            if (applicant == null)
            {
                return HttpNotFound();
            }
            return View(applicant);
        }

        // POST: Applicants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppID,AppName,AppSurname,AppEmail,AppPhone,AppAdd,Picture,data")] Applicant applicant, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    if (applicant.Files.Any(f => f.FileType == FileType.Avatar))
                    {
                        db.Files.Remove(applicant.Files.First(f => f.FileType == FileType.Avatar));
                    }
                    var avatar = new Models.File
                    {
                        FileName = System.IO.Path.GetFileName(upload.FileName),
                        FileType = FileType.Avatar,
                        ContentType = upload.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        avatar.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    applicant.Files = new List<Models.File> { avatar };
                }
            }
            return View(applicant);
        }

        // GET: Applicants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicant = db.Applicant.Find(id);
            if (applicant == null)
            {
                return HttpNotFound();
            }
            return View(applicant);
        }

        // POST: Applicants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Applicant applicant = db.Applicant.Find(id);
            db.Applicant.Remove(applicant);
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
