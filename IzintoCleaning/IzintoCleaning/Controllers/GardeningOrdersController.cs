using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IzintoCleaning.Models;
using Microsoft.AspNet.Identity;
using PayFast;
using PayFast.AspNet;
using System.Configuration;
using System.Threading.Tasks;

namespace IzintoCleaning.Controllers
{
    public class GardeningOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GardeningOrders
        public GardeningOrdersController()
        {
            this.payFastSettings = new PayFastSettings();
            this.payFastSettings.MerchantId = ConfigurationManager.AppSettings["MerchantId"];
            this.payFastSettings.MerchantKey = ConfigurationManager.AppSettings["MerchantKey"];
            this.payFastSettings.PassPhrase = ConfigurationManager.AppSettings["PassPhrase"];
            this.payFastSettings.ProcessUrl = ConfigurationManager.AppSettings["ProcessUrl"];
            this.payFastSettings.ValidateUrl = ConfigurationManager.AppSettings["ValidateUrl"];
            this.payFastSettings.ReturnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            this.payFastSettings.CancelUrl = ConfigurationManager.AppSettings["CancelUrl"];
            this.payFastSettings.NotifyUrl = ConfigurationManager.AppSettings["NotifyUrl"];
        }
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var id = User.Identity.GetUserId();
                var gardeningOrder1 = db.GardeningOrders.Include(g => g.Customer).Include(g => g.Gardening);
                return View(gardeningOrder1.ToList());//.Where(x => x.CustID == id));
                //var gardeningOrders = db.GardeningOrders.Include(g => g.Customer).Include(g => g.Gardening);
                //return View(gardeningOrders.ToList());
            }
            var userid = User.Identity.GetUserId();
            var gardeningOrder = db.GardeningOrders.Include(g => g.Customer).Include(g => g.Gardening).Where(x => x.CustID == userid).Where(x => x.paymentstatus == false); 

            return View(gardeningOrder.ToList());
        }

        public ActionResult PrevOrders()
        {
            var userid = User.Identity.GetUserId();
            var gardeningorders = db.GardeningOrders.Include(c => c.Gardening).Include(c => c.Customer).Where(x => x.CustID == userid).Where(x => x.paymentstatus == true);
            return View(gardeningorders.ToList());
        }

        // GET: GardeningOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GardeningOrder gardeningOrder = db.GardeningOrders.Find(id);
            if (gardeningOrder == null)
            {
                return HttpNotFound();
            }
            return View(gardeningOrder);
        }

        // GET: GardeningOrders/Create
        public ActionResult Create()
        {
            var userid = User.Identity.GetUserId();
            ViewBag.CustID = new SelectList(db.customer.Where(c => c.CustID == userid), "CustID", "CustName");
            ViewBag.EmpId = new SelectList(db.Employees, "EmpID", "EmpName");
            ViewBag.Package_ID = new SelectList(db.Gardenings, "Package_ID", "PackageName");
           

            return View();
        }

        // POST: GardeningOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Order_ID,Package_ID,CustID,area,cost,DateOfService,EmpmloyeeName,address")] GardeningOrder gardeningOrder)
        {
            if (ModelState.IsValid)
            {
                string str = Request.Params["btn1"];
                if (str == "View Final Amount")
                {
                    gardeningOrder.cost = gardeningOrder.calcFinal();
                    gardeningOrder.areacost = gardeningOrder.getCostArea();
                    
                    // cleaningOrder.showStatus = cleaningOrder.getAssign();
                }
                else if (str == "Confirm Order")
                {
                    gardeningOrder.cost = gardeningOrder.calcFinal();
                    gardeningOrder.areacost = gardeningOrder.getCostArea();
                    
                    db.GardeningOrders.Add(gardeningOrder);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.CustID = new SelectList(db.customer, "CustID", "CustName", gardeningOrder.CustID);
           // ViewBag.EmpId = new SelectList(db.Employees, "EmpID", "EmpName", gardeningOrder.EmpId);
            ViewBag.Package_ID = new SelectList(db.Gardenings, "Package_ID", "PackageName", gardeningOrder.Package_ID);
            return View(gardeningOrder);
        }

        // GET: GardeningOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GardeningOrder gardeningOrder = db.GardeningOrders.Find(id);
            if (gardeningOrder == null)
            {
                return HttpNotFound();
            }
            var userid = User.Identity.GetUserId();
            ViewBag.CustID = new SelectList(db.customer.Where(c => c.CustID == userid), "CustID", "CustName", gardeningOrder.CustID);
          //  ViewBag.EmpId = new SelectList(db.Employees, "EmpID", "EmpName", gardeningOrder.EmpId);
            ViewBag.Package_ID = new SelectList(db.Gardenings, "Package_ID", "PackageName", gardeningOrder.Package_ID);
            return View(gardeningOrder);
        }

        // POST: GardeningOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Order_ID,Package_ID,CustID,area,cost,DateOfService,EmpmloyeeName,address")] GardeningOrder gardeningOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gardeningOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustID = new SelectList(db.customer, "CustID", "CustName", gardeningOrder.CustID);
         //   ViewBag.EmpId = new SelectList(db.Employees, "EmpID", "EmpName", gardeningOrder.EmpId);
            ViewBag.Package_ID = new SelectList(db.Gardenings, "Package_ID", "PackageName", gardeningOrder.Package_ID);
            return View(gardeningOrder);
        }

        // GET: GardeningOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GardeningOrder gardeningOrder = db.GardeningOrders.Find(id);
            if (gardeningOrder == null)
            {
                return HttpNotFound();
            }
            return View(gardeningOrder);
        }

        // POST: GardeningOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GardeningOrder gardeningOrder = db.GardeningOrders.Find(id);
            db.GardeningOrders.Remove(gardeningOrder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteSelected(int id)
        {
            FumigationOrder fumigationOrder = db.FumigationOrders.Find(id);
            db.FumigationOrders.Remove(fumigationOrder);
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
        public ActionResult Paynow()
        {


            return RedirectToAction("Success");
        }
        public ActionResult Success()
        {
            return View();

        }

        #region Fields

        private readonly PayFastSettings payFastSettings;

        #endregion Fields

        #region Constructor


        #endregion Constructor

        #region Methods



        public ActionResult Recurring()
        {
            var recurringRequest = new PayFastRequest(this.payFastSettings.PassPhrase);
            // Merchant Details
            recurringRequest.merchant_id = this.payFastSettings.MerchantId;
            recurringRequest.merchant_key = this.payFastSettings.MerchantKey;
            recurringRequest.return_url = this.payFastSettings.ReturnUrl;
            recurringRequest.cancel_url = this.payFastSettings.CancelUrl;
            recurringRequest.notify_url = this.payFastSettings.NotifyUrl;
            // Buyer Details
            recurringRequest.email_address = "nkosi@finalstride.com";
            // Transaction Details
            recurringRequest.m_payment_id = "8d00bf49-e979-4004-228c-08d452b86380";
            recurringRequest.amount = 20;
            recurringRequest.item_name = "Recurring Option";
            recurringRequest.item_description = "Some details about the recurring option";
            // Transaction Options
            recurringRequest.email_confirmation = true;
            recurringRequest.confirmation_address = "drnendwandwe@gmail.com";
            // Recurring Billing Details
            recurringRequest.subscription_type = SubscriptionType.Subscription;
            recurringRequest.billing_date = DateTime.Now;
            recurringRequest.recurring_amount = 20;
            recurringRequest.frequency = BillingFrequency.Monthly;
            recurringRequest.cycles = 0;
            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{recurringRequest.ToString()}";
            return Redirect(redirectUrl);
        }


        public ActionResult OnceOff(int id)
        {

            //var uid = User.Identity.GetUserId();
            //var appointments = db.Appointments.Include(a => a.Client).Where(x => x.ClientId == uid).Where(a => a.paymentstatus == false).Where(a => a.status == false);
            GardeningOrder gardeningOrder = db.GardeningOrders.Find(id);

            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);
            // Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;
            // Buyer Details
            onceOffRequest.email_address = "sbtu01@payfast.co.za";
            decimal amount = gardeningOrder.calcFinal();
            //var products = db.Items.Select(x => x.Item_Name).ToList();
            // Transaction Details
            onceOffRequest.m_payment_id = "";
            onceOffRequest.amount = (double)amount;
            onceOffRequest.item_name = "Your appointment Number is " + id;
            onceOffRequest.item_description = "You are now paying your rental fee";
            // Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";

            gardeningOrder.paymentstatus = true;
            db.Entry(gardeningOrder).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect(redirectUrl);
        }


        public ActionResult AdHoc()
        {
            var adHocRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            adHocRequest.merchant_id = this.payFastSettings.MerchantId;
            adHocRequest.merchant_key = this.payFastSettings.MerchantKey;
            adHocRequest.return_url = this.payFastSettings.ReturnUrl;
            adHocRequest.cancel_url = this.payFastSettings.CancelUrl;
            adHocRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            adHocRequest.email_address = "sbtu01@payfast.co.za";

            // Transaction Details
            adHocRequest.m_payment_id = "";
            adHocRequest.amount = 70;
            adHocRequest.item_name = "Adhoc Agreement";
            adHocRequest.item_description = "Some details about the adhoc agreement";

            // Transaction Options
            adHocRequest.email_confirmation = true;
            adHocRequest.confirmation_address = "sbtu01@payfast.co.za";

            // Recurring Billing Details
            adHocRequest.subscription_type = SubscriptionType.AdHoc;

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{adHocRequest.ToString()}";

            return Redirect(redirectUrl);
        }

        public ActionResult Return()
        {
            return View();
        }

        public ActionResult Cancel()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Notify([ModelBinder(typeof(PayFastNotifyModelBinder))]PayFastNotify payFastNotifyViewModel)
        {
            payFastNotifyViewModel.SetPassPhrase(this.payFastSettings.PassPhrase);

            var calculatedSignature = payFastNotifyViewModel.GetCalculatedSignature();

            var isValid = payFastNotifyViewModel.signature == calculatedSignature;

            System.Diagnostics.Debug.WriteLine($"Signature Validation Result: {isValid}");

            // The PayFast Validator is still under developement
            // Its not recommended to rely on this for production use cases
            var payfastValidator = new PayFastValidator(this.payFastSettings, payFastNotifyViewModel, IPAddress.Parse(this.HttpContext.Request.UserHostAddress));

            var merchantIdValidationResult = payfastValidator.ValidateMerchantId();

            System.Diagnostics.Debug.WriteLine($"Merchant Id Validation Result: {merchantIdValidationResult}");

            var ipAddressValidationResult = payfastValidator.ValidateSourceIp();

            System.Diagnostics.Debug.WriteLine($"Ip Address Validation Result: {merchantIdValidationResult}");

            // Currently seems that the data validation only works for successful payments
            if (payFastNotifyViewModel.payment_status == PayFastStatics.CompletePaymentConfirmation)
            {
                var dataValidationResult = await payfastValidator.ValidateData();

                System.Diagnostics.Debug.WriteLine($"Data Validation Result: {dataValidationResult}");
            }

            if (payFastNotifyViewModel.payment_status == PayFastStatics.CancelledPaymentConfirmation)
            {
                System.Diagnostics.Debug.WriteLine($"Subscription was cancelled");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion Methods
    }
}
