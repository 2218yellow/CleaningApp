using IzintoCleaning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data;
using System.Net;
using System.Web.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;
using PayFast;
using PayFast.AspNet;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace IzintoCleaning.Controllers
{
    public class ShoppingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public string shoppingCartID { get; set; }
        public const string CartSessionKey = "CartId";
        //GET
        public ShoppingController()
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
            return View(db.Equipment.ToList());
        }
        public ActionResult add_to_cart(int id)
        {
            var item = db.Equipment.Find(id);
            if (item != null)
            {
                add_item_to_cart(id);
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult remove_from_cart(string id)
        {
            var item = db.Cart_Items.Find(id);
            if (item != null)
            {
                remove_item_from_cart(id: id);
                return RedirectToAction("ShoppingCart");
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult ShoppingCart()
        {

            shoppingCartID = GetCartID();
            ViewBag.Total = get_cart_total(id: shoppingCartID);
            ViewBag.TotalQTY = get_Cart_Items().FindAll(x => x.cart_id == shoppingCartID).Sum(q => q.quantity);
            return View(db.Cart_Items.ToList().FindAll(x => x.cart_id == shoppingCartID));
        }
        [HttpPost]
        public ActionResult ShoppingCart(List<Cart_Item> items)
        {
            shoppingCartID = GetCartID();

            foreach (var i in items)
            {
                updateCart(i.cart_item_id, i.quantity);
            }
            ViewBag.Total = get_cart_total(shoppingCartID);
            ViewBag.TotalQTY = get_Cart_Items().FindAll(x => x.cart_id == shoppingCartID).Sum(q => q.quantity);
            return View(db.Cart_Items.ToList().FindAll(x => x.cart_id == shoppingCartID));
        }
        public ActionResult countCartItems()
        {
            int qty = get_Cart_Items().Count();
            return Content(qty.ToString());
        }
        public ActionResult Checkout()
        {
            if (get_Cart_Items().Count == 0)
            {
                ViewBag.Err = "Opps... you should have atleast one cart item, please shop a few items";
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("PlaceOrder");
        }
        [Authorize]
        public ActionResult DeliveryOption()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeliveryOption(string colorRadio, string Street, string City, string PostalCode)
        {
            if (!String.IsNullOrEmpty(colorRadio))
            {
                if (colorRadio.Equals("StandardDelivery"))
                {
                    Session["Street"] = Street;
                    Session["City"] = City;
                    Session["PostalCode"] = PostalCode;
                    return RedirectToAction("PlaceOrder", new { id = "deliver" });
                }
            }
            return View();
        }
        public ActionResult PlaceOrder(string id)
        {

            var nn = User.Identity.Name;
            var patient = db.Users.ToList().Find(x => x.Email == HttpContext.User.Identity.Name);
            db.Orders.Add(new Order()
            {
                Email = patient.Email,
                date_created = DateTime.Now,
                shipped = true,
                status = "Payed"
            });
            db.SaveChanges();
            var order = db.Orders.ToList()
                .FindAll(x => x.Email == patient.UserName)
                .LastOrDefault();

            if (id == "deliver")
            {
                db.Order_Addresses.Add(new Order_Address()
                {
                    Order_ID = order.Order_ID,
                    street = Session["Street"].ToString(),
                    city = Session["City"].ToString(),
                    zipcode = Session["PostalCode"].ToString()
                });
                db.SaveChanges();
            }

            Item_Business ob = new Item_Business();

            var items = get_Cart_Items();

            foreach (var item in items)
            {
                var x = new Order_Item()
                {
                    Order_id = order.Order_ID,
                    item_id = item.item_id,
                    quantity = item.quantity,
                    price = item.price
                };
                ob.updateStock_bot(x.item_id, x.quantity);
                db.Order_Items.Add(x);
                db.SaveChanges();
            }
            empty_Cart();
            //order tracking
            //db.Order_Trackings.Add(new Order_Tracking()
            //{
            //    order_ID = order.Order_ID,
            //    date = DateTime.Now,
            //    status = "Awaiting Payment",
            //    Recipient = ""
            //});
            db.SaveChanges();

            //Redirect to payment
            return RedirectToAction("Payment", new { id = order.Order_ID });
        }
        public ActionResult Payment(int? id)
        {
            var order = db.Orders.Find(id);
            ViewBag.Order = order;
            ViewBag.Account = db.Users.Find(order.Email);
            ViewBag.Address = db.Order_Addresses.ToList().Find(x => x.Order_ID == order.Order_ID);
            ViewBag.Items = db.Order_Items.ToList().FindAll(x => x.Order_id == order.Order_ID);
            ViewBag.Total = get_order_total(order.Order_ID);


            try
            {
                string url = "<a href=" + "http://ramrajdentistry.azurewebsites.net/Shopping/Payment/" + id + " >  here" + "</a>";
                string table = "<br/>" +
                               "Items in this order<br/>" +
                               "<table>";
                table += "<tr>" +
                         "<th>Item</th>"
                         +
                         "<th>Quantity</th>"
                         +
                         "<th>Price</th>" +
                         "</tr>";
                foreach (var item in (List<Order_Item>)ViewBag.Items)
                {
                    string itemsinoder = "<tr> " +
                                         "<td>" + item.Item.EquipName + " </td>" +
                                         "<td>" + item.quantity + " </td>" +
                                         "<td>R " + item.price + " </td>" +
                                         "<tr/>";
                    table += itemsinoder;
                }

                table += "<tr>" +
                         "<th></th>"
                         +
                         "<th></th>"
                         +
                         "<th>" + get_order_total(order.Order_ID).ToString("R0.00") + "</th>" +
                         "</tr>";
                table += "</table>";

                var client = new SendGridClient("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");
                var from = new EmailAddress("no-reply@shopifyhere.com", "Shopify Here");
                var subject = "Order " + id + " | Awaiting Payment";
                var to = new EmailAddress(((Customer)ViewBag.Account).Email, ((Customer)ViewBag.Account).CustName + " ");
                var htmlContent = "Hi " + order.Profile.CustName + ", Your order was placed, you can securely pay your order from " + url + table;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
                var response = client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        
        

        

        public ActionResult Secure_Payment(int? id)
        {
            var username = User.Identity.GetUserName();
            var order = db.Orders.Find(id);
            ViewBag.Order = order;
            string email = username;
            ViewBag.Account = email;
            ViewBag.Address = db.Order_Addresses.ToList().Find(x => x.Order_ID == order.Order_ID);
            ViewBag.Items = db.Order_Items.ToList().FindAll(x => x.Order_id == order.Order_ID);
            
            ViewBag.Total = get_order_total(order.Order_ID);

            return Redirect(PaymentLink(get_order_total(order.Order_ID).ToString(), "Order Payment | Order No: " + order.Order_ID, order.Order_ID));
        }
        public ActionResult Payment_Cancelled(int? id)
        {
            var order = db.Orders.Find(id);
            ViewBag.Order = order;
            ViewBag.Account = db.customer.Find(order.Email);
            ViewBag.Address = db.Order_Addresses.ToList().Find(x => x.Order_ID == order.Order_ID);
            ViewBag.Items = db.Order_Items.ToList().FindAll(x => x.Order_id == order.Order_ID);

            ViewBag.Total = get_order_total(order.Order_ID);
            try
            {
                string url = "<a href=" + "http://shopify-here.azurewebsites.net/Shopping/Payment/" + id + " >  here" + "</a>";
                string table = "<br/>" +
                               "Items in this order<br/>" +
                               "<table>";
                table += "<tr>" +
                         "<th>Item</th>"
                         +
                         "<th>Quantity</th>"
                         +
                         "<th>Price</th>" +
                         "</tr>";
                foreach (var item in (List<Order_Item>)ViewBag.Items)
                {
                    string items = "<tr> " +
                                   "<td>" + item.Item.EquipName + " </td>" +
                                   "<td>" + item.quantity + " </td>" +
                                   "<td>R " + item.price + " </td>" +
                                   "<tr/>";
                    table += items;
                }

                table += "<tr>" +
                         "<th></th>"
                         +
                         "<th></th>"
                         +
                         "<th>" + get_order_total(order.Order_ID).ToString("R0.00") + "</th>" +
                         "</tr>";
                table += "</table>";

                var client = new SendGridClient("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");
                var from = new EmailAddress("no-reply@shopifyhere.com", "Shopify Here");
                var subject = "Order " + id + " | Awaiting Payment";
                var to = new EmailAddress(order.Profile.Email, order.Profile.CustName+ " ");
                var htmlContent = "Hi " + order.Profile.CustName + ", Your order payment process was cancelled, you can still securely pay your order from " + url + table;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
                var response = client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult Payment_Successfull(int? id)
        {
            var order = db.Orders.Find(id);
            try
            {
                order.status = "At warehouse";

                ////order tracking
                //db.Order_Trackings.Add(new Order_Tracking()
                //{
                //    order_ID = order.Order_ID,
                //    date = DateTime.Now,
                //    status = "Payment Recieved | Order still at warehouse",
                //    Recipient = ""
                //});
                //db.SaveChanges();
                //db.Payments.Add(new Payment()
                //{
                //    Date = DateTime.Now,
                //    Email = db.customer.FirstOrDefault(p => p.CustName == User.Identity.Name).Email,
                //    AmountPaid = get_order_total(order.Order_ID),
                //    PaymentFor = "Order " + id + " Payment",
                //    PaymentMethod = "PayFast Online"
                //});
                //db.SaveChanges();
                if (db.Order_Addresses.Where(p => p.Order_ID == id) != null)
                {
                    var expected_Date = DateTime.Now.AddDays(2);
                    do
                    {
                        expected_Date = expected_Date.AddDays(1);
                    } while (expected_Date.DayOfWeek.ToString().ToLower() == "sunday" ||
                        expected_Date.DayOfWeek.ToString().ToLower() == "saturday");

                    //Delivery
                }
                db.SaveChanges();
                ViewBag.Items = db.Order_Items.ToList().FindAll(x => x.Order_id == order.Order_ID);

                update_Stock((int)id);

                string table = "<br/>" +
                               "Items in this order<br/>" +
                               "<table>";
                table += "<tr>" +
                         "<th>Item</th>"
                         +
                         "<th>Quantity</th>"
                         +
                         "<th>Price</th>" +
                         "</tr>";
                foreach (var item in (List<Order_Item>)ViewBag.Items)
                {
                    string items = "<tr> " +
                                   "<td>" + item.Item.EquipName + " </td>" +
                                   "<td>" + item.quantity + " </td>" +
                                   "<td>R " + item.price + " </td>" +
                                   "<tr/>";
                    table += items;
                }

                table += "<tr>" +
                         "<th></th>"
                         +
                         "<th></th>"
                         +
                         "<th>" + get_order_total(order.Order_ID).ToString("R 0.00") + "</th>" +
                         "</tr>";
                table += "</table>";

                var client = new SendGridClient("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");
                var from = new EmailAddress("no-reply@shopifyhere.com", "Shopify Here");
                var subject = "Order " + id + " | Payment Recieved";
                var to = new EmailAddress(order.Profile.Email, order.Profile.CustName + " ");
                var htmlContent = "Hi " + order.Profile.CustName + ", We recieved your payment, you will have your goodies any time from now " + table;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
                var response = client.SendEmailAsync(msg);
            }
            catch (Exception ex) { }

            ViewBag.Order = order;
            ViewBag.Account = db.Users.Find(order.Email);
            ViewBag.Address = db.Order_Addresses.ToList().Find(x => x.Order_ID == order.Order_ID);
            ViewBag.Total = get_order_total(order.Order_ID);

            return View();
        }

        #region Cart Methods
        public void add_item_to_cart(int id)
        {
            shoppingCartID = GetCartID();

            var item = db.Equipment.Find(id);
            if (item != null)
            {
                var cartItem =db.Cart_Items.FirstOrDefault(x => x.cart_id == shoppingCartID && x.item_id == item.EquipID);
                if (cartItem == null)
                {
                    var cart = db.Carts.Find(shoppingCartID);
                    if (cart == null)
                    {
                        db.Carts.Add(entity: new Cart()
                        {
                            cart_id = shoppingCartID,
                            date_created = DateTime.Now
                        });
                        db.SaveChanges();
                    }

                    db.Cart_Items.Add(entity: new Cart_Item()
                    {
                        cart_item_id = Guid.NewGuid().ToString(),
                        cart_id = shoppingCartID,
                        item_id = item.EquipID,
                        quantity = 1,
                        price = item.EquipCost
                    }
                        );
                }
                else
                {
                    cartItem.quantity++;
                }
                db.SaveChanges();
            }
        }
        public void remove_item_from_cart(string id)
        {
            shoppingCartID = GetCartID();

            var item = db.Cart_Items.Find(id);
            if (item != null)
            {
                var cartItem =
                    db.Cart_Items.FirstOrDefault(predicate: x => x.cart_id == shoppingCartID && x.item_id == item.item_id);
                if (cartItem != null)
                {
                    db.Cart_Items.Remove(entity: cartItem);
                }
                db.SaveChanges();
            }
        }
        public List<Cart_Item> get_Cart_Items()
        {
            shoppingCartID = GetCartID();
            return db.Cart_Items.ToList().FindAll(match: x => x.cart_id == shoppingCartID);
        }
        public void updateCart(string id, int qty)
        {
            var item = db.Cart_Items.Find(id);
            if (qty < 0)
                item.quantity = qty / -1;
            else if (qty == 0)
                remove_item_from_cart(item.cart_item_id);
            else
                item.quantity = qty;
            db.SaveChanges();
        }
        public decimal get_cart_total(string id)
        {
            decimal amount = 0;
            foreach (var item in db.Cart_Items.ToList().FindAll(match: x => x.cart_id.ToString() == id))
            {
                amount += (item.price * item.quantity);
            }
            return amount;
        }
        public void empty_Cart()
        {
            shoppingCartID = GetCartID();
            foreach (var item in db.Cart_Items.ToList().FindAll(match: x => x.cart_id == shoppingCartID))
            {
                db.Cart_Items.Remove(item);
            }
            try
            {
                db.Carts.Remove(db.Carts.Find(shoppingCartID));
                db.SaveChanges();
            }
            catch (Exception ex) { }
        }
        public string GetCartID()
        {
            if (System.Web.HttpContext.Current.Session[name: CartSessionKey] == null)
            {
                if (!String.IsNullOrWhiteSpace(value: System.Web.HttpContext.Current.User.Identity.Name))
                {
                    System.Web.HttpContext.Current.Session[name: CartSessionKey] = System.Web.HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid temp = Guid.NewGuid();
                    System.Web.HttpContext.Current.Session[name: CartSessionKey] = temp.ToString();
                }
            }
            return System.Web.HttpContext.Current.Session[name: CartSessionKey].ToString();
        }
        #endregion

        #region Customer Order Methods
        public decimal get_order_total(int id)
        {
            decimal amount = 0;
            foreach (var item in db.Order_Items.ToList().FindAll(match: x => x.Order_id == id))
            {
                amount += (item.price * item.quantity);
            }
            return amount;
        }
        public string PaymentLink(string totalCost, string paymentSubjetc, int order_id)
        {

            string paymentMode = ConfigurationManager.AppSettings["PaymentMode"], site, merchantId, merchantKey, returnUrl;

            if (paymentMode == "test")
            {
                site = "https://sandbox.payfast.co.za/eng/process?";
                merchantId = "10010427";
                merchantKey = "6xiy4isdr4pa4";
            }
            else if (paymentMode == "live")
            {
                site = "https://www.payfast.co.za/eng/process?";
                merchantId = ConfigurationManager.AppSettings["PF_MerchantID"];
                merchantKey = ConfigurationManager.AppSettings["PF_MerchantKey"];
            }
            else
            {
                throw new InvalidOperationException("Payment method unknown.");
            }
            var stringBuilder = new StringBuilder();
            //string url = Url.Action("Quotes", "Order",
            //    new System.Web.Routing.RouteValueDictionary(new { id = orderid }),
            //    "http", Request.Url.Host);

            stringBuilder.Append("&merchant_id=" + HttpUtility.HtmlEncode(merchantId));
            stringBuilder.Append("&merchant_key=" + HttpUtility.HtmlEncode(merchantKey));
            stringBuilder.Append("&return_url=" + HttpUtility.HtmlEncode("http://shopify-here.azurewebsites.net/Shopping/Payment_Successfull/" + order_id));
            stringBuilder.Append("&cancel_url=" + HttpUtility.HtmlEncode("http://shopify-here.azurewebsites.net/Shopping/Payment_Cancelled/" + order_id));
            stringBuilder.Append("&notify_url=" + HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["PF_NotifyURL"]));

            string amt = totalCost;
            amt = amt.Replace(",", ".");

            stringBuilder.Append("&amount=" + HttpUtility.HtmlEncode(amt));
            stringBuilder.Append("&item_name=" + HttpUtility.HtmlEncode(paymentSubjetc));
            stringBuilder.Append("&email_confirmation=" + HttpUtility.HtmlEncode("1"));
            stringBuilder.Append("&confirmation_address=" + HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["PF_ConfirmationAddress"]));

            return (site + stringBuilder);
        }
        public void update_Stock(int id)
        {
            var order = db.Orders.Find(id);
            List<Order_Item> items = db.Order_Items.ToList().FindAll(x => x.Order_id == id);
            foreach (var item in items)
            {
                var product = db.Equipment.Find(item.item_id);
                if (product != null)
                {
                    if ((product.QuantityInStock -= item.quantity) >= 0)
                    {
                        product.QuantityInStock -= item.quantity;
                    }
                    else
                    {
                        item.quantity = product.QuantityInStock;
                        product.QuantityInStock = 0;
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex) { }
                }
            }
        }
        #endregion
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
            Order order = db.Orders.Find(id);

            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);
            // Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;
            // Buyer Details
            onceOffRequest.email_address = "sbtu01@payfast.co.za";
            decimal amount = get_order_total(id);
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

          //  order.paymentstatus = true;
            db.Entry(order).State = EntityState.Modified;
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