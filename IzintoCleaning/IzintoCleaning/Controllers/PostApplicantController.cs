using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IzintoCleaning.Models;

namespace IzintoCleaning.Controllers
{
    public class PostApplicantController : Controller
    {
         // GET: PostApplicant
        public ActionResult ApplicationsList()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<ApplicationsVM> ApplicationList = new List<ApplicationsVM>();

            var applist = (from app in db.Applicant join po in db.PostApplication on app.AppID equals po.AppID select new {app.AppID, app.AppName, app.AppSurname, app.AppEmail, app.AppAdd, po.post.SerCode, po.post.PostDesc, po.post.PostClosingDate }).ToList();
            foreach(var item in applist)
            {
                ApplicationsVM applications = new ApplicationsVM();
                applications.AppID = item.AppID;
                applications.AppName = item.AppName;
                applications.AppSurname = item.AppSurname;
                applications.AppEmail = item.AppEmail;
                applications.AppAdd = item.AppAdd;
                applications.SerCode = item.SerCode;
                applications.PostDesc = item.PostDesc;
                applications.PostClosingDate = item.PostClosingDate;
                ApplicationList.Add(applications);
            }
            return View(ApplicationList);
        }
    }
}