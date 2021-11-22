using IzintoCleaning.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace IzintoCleaning.Models
{
    public class Order
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Order_ID { get; set; }
        public DateTime date_created { get; set; }
        //[System.ComponentModel.DataAnnotations.Schema.ForeignKey("Profile")]
        public string Email { get; set; }
        public bool shipped { get; set; }
        public string status { get; set; }
        public bool packed { get; set; }
        public virtual ICollection<Order_Item> Order_Items { get; set; }
        public virtual ICollection<Order_Address> Order_Addresses { get; set; }
        public virtual Customer Profile { get; set; }

        // public virtual ICollection<Order_Tracking> Order_Tracking { get; set; }
        //public virtual ICollection<Delivery_Schedule> Delivery_Schedules { get; set; }
        //public virtual ICollection<Exchange_n_Return> Exchange_n_Returns { get; set; }
        ApplicationDbContext db = new ApplicationDbContext();

        public decimal get_cart_total(int id)
        {
            decimal amount = 0;
            foreach (var item in db.Cart_Items.ToList().FindAll(match: x => x.cart_id == id.ToString()))
            {
                amount += (item.price * item.quantity);
            }
            return amount;
        }
    }
}