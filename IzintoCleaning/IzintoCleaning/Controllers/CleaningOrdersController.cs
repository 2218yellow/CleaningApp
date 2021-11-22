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
    public class CleaningOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CleaningOrders
        public CleaningOrdersController()
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
                var cleaningOrder1 = db.CleaningOrders.Include(c => c.Cleaning).Include(c => c.Customer);
                return View(cleaningOrder1.ToList());//.Where(x => x.CustID == id));
            }
            var userid = User.Identity.GetUserId();
            var cleaningorder = db.CleaningOrders.Include(c => c.Cleaning).Include(c => c.Customer).Where(x => x.CustID == userid).Where(x=>x.paymentstatus==false);

            //var cleaningOrders = db.CleaningOrders.Include(c => c.Cleaning).Include(c => c.Customer);
            return View(cleaningorder.ToList());
        }

        public ActionResult PrevOrders()
        {
            var userid = User.Identity.GetUserId();
            var cleaningorder = db.CleaningOrders.Include(c => c.Cleaning).Include(c => c.Customer).Where(x => x.CustID == userid).Where(x => x.paymentstatus == true);
            return View(cleaningorder.ToList());
        }
        //public ActionResult Confirmation()
        //{
        //    var cleaningOrders = db.CleaningOrders.Include(c => c.Cleaning).Include(c => c.Customer).Where(c=>c.!=null);
        //    return View(cleaningOrders.ToList());
        //}

        // GET: CleaningOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CleaningOrder cleaningOrder = db.CleaningOrders.Find(id);
            if (cleaningOrder == null)
            {
                return HttpNotFound();
            }
            return View(cleaningOrder);
        }

        // GET: CleaningOrders/Create
        public ActionResult Create()
        {
            ViewBag.Package_ID = new SelectList(db.Cleanings, "Package_ID", "PackageName");
            var userid = User.Identity.GetUserId();
            ViewBag.CustID = new SelectList(db.customer.Where(c => c.CustID == userid), "CustID", "CustName");
       
            return View();
        }

        // POST: CleaningOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Order_ID,Package_ID,CustID,area,storey,cost,DateOfService,address")] CleaningOrder cleaningOrder)
        {
            if (ModelState.IsValid)
            {
                string str = Request.Params["btn1"];
                if (str == "View Final Amount")
                {
                    cleaningOrder.cost = cleaningOrder.calcFinal();
                    cleaningOrder.areacost = cleaningOrder.getCostArea();
                    cleaningOrder.floorcost = cleaningOrder.getFloorCost();
                   // cleaningOrder.showStatus = cleaningOrder.getAssign();
                }
                else if (str == "Confirm Order")
                {
                    cleaningOrder.cost = cleaningOrder.calcFinal();
                    cleaningOrder.areacost = cleaningOrder.getCostArea();
                    cleaningOrder.floorcost = cleaningOrder.getFloorCost();

                    db.CleaningOrders.Add(cleaningOrder);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Package_ID = new SelectList(db.Cleanings, "Package_ID", "PackageName", cleaningOrder.Package_ID);
            ViewBag.CustID = new SelectList(db.customer, "CustID", "CustName", cleaningOrder.CustID);
   
            return View(cleaningOrder);
        }

        // GET: CleaningOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CleaningOrder cleaningOrder = db.CleaningOrders.Find(id);
            if (cleaningOrder == null)
            {
                return HttpNotFound();
            }
            var userid = User.Identity.GetUserId();
            ViewBag.Package_ID = new SelectList(db.Cleanings, "Package_ID", "PackageName", cleaningOrder.Package_ID);
            ViewBag.CustID = new SelectList(db.customer.Where(c => c.CustID == userid), "CustID", "CustName", cleaningOrder.CustID);
          
            return View(cleaningOrder);
        }

        // POST: CleaningOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Order_ID,Package_ID,CustID,area,storey,cost,DateOfService,EmpId,address")] CleaningOrder cleaningOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cleaningOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Package_ID = new SelectList(db.Cleanings, "Package_ID", "PackageName", cleaningOrder.Package_ID);
            ViewBag.CustID = new SelectList(db.customer, "CustID", "CustName", cleaningOrder.CustID);
         
            return View(cleaningOrder);
        }

        // GET: CleaningOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CleaningOrder cleaningOrder = db.CleaningOrders.Find(id);
            if (cleaningOrder == null)
            {
                return HttpNotFound();
            }
            return View(cleaningOrder);
        }

        // POST: CleaningOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CleaningOrder cleaningOrder = db.CleaningOrders.Find(id);
            db.CleaningOrders.Remove(cleaningOrder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteSelected (int id)
        {
            CleaningOrder cleaningOrder = db.CleaningOrders.Find(id);
            db.CleaningOrders.Remove(cleaningOrder);
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
            CleaningOrder cleaningOrder = db.CleaningOrders.Find(id);

            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);
            // Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;
            // Buyer Details
            onceOffRequest.email_address = "sbtu01@payfast.co.za";
           decimal amount = cleaningOrder.calcFinal();
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

           cleaningOrder.paymentstatus = true;
            db.Entry(cleaningOrder).State = EntityState.Modified;
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


    