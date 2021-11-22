using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IzintoCleaning.Models
{
    public class Cart
    {
        [Key]
        public string cart_id { get; set; }
        public DateTime date_created { get; set; }

        public virtual ICollection<Cart_Item> Cart_Items { get; set; }
    }
}