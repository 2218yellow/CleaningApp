using IzintoCleaning.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LockSmith.Shop.Logic
{
    public class Customer_Business
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public List<Customer> all()
        {
            return db.customer.ToList();
        }
        public bool add(Customer model)
        {
            try
            {
                db.customer.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool edit(Customer model)
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
        public Customer find_by_id(int? id)
        {
            return db.customer.Find(id);
        }

        public string getGender(string id_num)
        {
            if (Convert.ToInt16(id_num.Substring(7, 1)) >= 5)
                return "Male";
            else
                return "Female";
        }
    }
}
