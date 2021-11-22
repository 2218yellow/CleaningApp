using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IzintoCleaning.Models
{
    public class Item_Service
    {
        private ApplicationDbContext dataContext;

        public Item_Service()
        {
            this.dataContext = new ApplicationDbContext();
        }

        public List<Equipment> GetItems()
        {
            return dataContext.Equipment.ToList();
        }
        public bool AddItem(Equipment equipment)
        {
            try
            {
                dataContext.Equipment.Add(equipment);
                dataContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }

        //public void UpdateQuantity(int id, int qty)
        //{
        //    ApplicationDbContext db = new ApplicationDbContext();
        //    var qytUpdate = db.Items.Find(id);
        //    qytUpdate.QuantityInStock = qytUpdate.QuantityInStock - qty;
        //}
        public bool UpdateItem(Equipment equipment)
        {
            try
            {
                dataContext.Entry(equipment).State = EntityState.Modified;
                dataContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool RemoveItem(Equipment equipment)
        {
            try
            {
                dataContext.Equipment.Remove(equipment);
                dataContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public Equipment GetItem(int? EquipID)
        {
            return dataContext.Equipment.Find(EquipID);
        }
    }
}