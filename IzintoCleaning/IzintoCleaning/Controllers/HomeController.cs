using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IzintoCleaning.Models;

namespace IzintoCleaning.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Success()
        {
            ViewBag.Message = "Your Payment Was Completed Successfully";

            return View();
        }

        public ActionResult Error()
        {
            ViewBag.Message = "Your Payment Failed";

            return View();
        }

        [ChildActionOnly]
        public ActionResult Cleaning()
        {
            ViewBag.CustID = new SelectList(db.customer, "CustID", "CustName");
            //ViewBag.DescID = new SelectList(db.ServiceDescs, "DescID", "CritName");
            return PartialView("_Cleaning");
        }

    }
}