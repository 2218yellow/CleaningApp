using IzintoCleaning.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IzintoCleaning.Models
{
    public class Item_Business
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<Equipment> all()
        {
            return db.Equipment.ToList();
        }
        public bool add(Equipment model)
        {
            try
            {
                db.Equipment.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool edit(Equipment model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool delete(Equipment model)
        {
            try
            {
                db.Equipment.Remove(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public Equipment find_by_id(int? id)
        {
            return db.Equipment.Find(id);
        }
        public void updateStock_Received(int item_id, int quantity)
        {
            var item = db.Equipment.Find(item_id);
            item.QuantityInStock += quantity;
            db.SaveChanges();
        }
        public void updateStock_bot(int item_id, int quantity)
        {
            var item = db.Equipment.Find(item_id);
            item.QuantityInStock -= quantity;
            db.SaveChanges();
        }
        public void updateOrder(int id, decimal price)
        {
            var item = db.Order_Items.Find(id);
            item.price = price;
            //item.replied = true;
            //item.date_replied = DateTime.Now;
            //item.status = "Supplier Replied with Pricing Details";
            db.SaveChanges();
        }

    }
}