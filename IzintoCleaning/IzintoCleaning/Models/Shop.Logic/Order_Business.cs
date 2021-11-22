using IzintoCleaning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IzintoCleaning.Models
{
    //public class Order_Business
    //{
    //    private ApplicationDbContext db = new ApplicationDbContext();
       
    //    public List<Models.Order> cust_all()
    //    {
    //        return db.Orders.ToList();
    //    }
    //    public List<Order> cust_find_by_status(string status)
    //    {
    //        return db.Orders.Where(p => p.status.ToLower() == status.ToLower()).ToList();
    //    }
    //    public Order cust_find_by_id(int? id)
    //    {
    //        return db.Orders.Find(id);
    //    }
    //    public List<Order_Item> cust_Order_items(int? id)
    //    {
    //        return cust_find_by_id(id).Order_Items.ToList();
    //    }

        //public List<Order_Tracking> get_tracking_report(int? id)
        //{
        //    return db.Order_Trackings.Where(x => x.order_ID == id).ToList();
        //}
        //public void mark_as_packed(int? id)
        //{
        //    var order = cust_find_by_id(id);
        //    order.packed = true;
        //    if (db.Order_Addresses.Where(p => p.Order_ID == id) != null)
        //    {
        //        order.status = "With courier";
            
        //        db.Order_Trackings.Add(new Order_Tracking()
        //        {
        //            order_ID = order.Order_ID,
        //            date = DateTime.Now,
        //            status = "Order Packed, Now with our courier",
        //            Recipient = ""
        //        });
        //    }

        //    db.SaveChanges();
        //}
        //public void schedule_delivery(int? order_Id, DateTime date)
        //{
        //    var order = cust_find_by_id(order_Id);
        //    order.status = "Scheduled for delivery";
        //    //order tracking
        //    db.Order_Trackings.Add(new Order_Tracking()
        //    {
        //        order_ID = order.Order_ID,
        //        date = DateTime.Now,
        //        status = "Scheduled for delivery on " + date.ToLongDateString(),
        //        Recipient = ""
        //    });
        //    db.SaveChanges();
        //}
  //  }
}
