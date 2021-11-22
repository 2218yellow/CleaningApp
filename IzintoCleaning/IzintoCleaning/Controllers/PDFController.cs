using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IzintoCleaning.Models;

namespace IzintoCleaning.Controllers
{
    public class PDFController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: PDF
        public ActionResult Index(int id)
        {
            var fileToRetrieve = db.PDFs.Find(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
    }
}