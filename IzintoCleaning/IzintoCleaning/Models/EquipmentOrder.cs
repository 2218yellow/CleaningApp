using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;


namespace IzintoCleaning.Models
{
    public class EquipmentOrder
    {

        [Key]
        [DisplayName("Order No.  ")]
        public int EquipmentOrderId { get; set; }

        public int qty { get; set; }

       // public string cart_item_id { get; set; }
       // public virtual ICollection<Cart_Item> cart_item { get; set; }
       // [ForeignKey("cart_item_id")]

       // public virtual Cart Cart { get; set; }
       // [ForeignKey("Cart")]
       // public string cart_id { get; set; }
       // public virtual Equipment Equipment { get; set; }

        public decimal amt { get; set; }

        [DisplayName("Customer Email")]
        public string creator { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();

        //public int getQuatntity()
        //{
        //    var i = (from o in db.Cart_Items
        //             where o.cart_item_id == cart_item_id
        //             select o.quantity).FirstOrDefault();
        //    return i;
                     
        //}
        //public decimal getFinalPrice()
        //{
        //    var pr = (from i in db.Cart_Items
        //             where i.cart_item_id == cart_item_id
        //             select i.price).FirstOrDefault();
        //    return pr;

        //}
      
    } }
          

