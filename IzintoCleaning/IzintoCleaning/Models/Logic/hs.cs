//using System;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace IzintoCleaning.Models
//{
//    public class Order_Item
//    {
//        [System.ComponentModel.DataAnnotations.Key]
//        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
//        public int Order_Item_id { get; set; }
//        [ForeignKey("Order")]
//        public int Order_id { get; set; }
//        public virtual Order Order { get; set; }

//        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("Equipment")]
//        public int Equip_ID { get; set; }
//        public virtual Equipment Equipment { get; set; }

//        public int quantity { get; set; }
//        public double price { get; set; }

//        public string Status { get; set; }

//        /*  public bool replied { get; set; }
//          public Nullable<DateTime> date_replied { get; set; }

//          public bool accepted { get; set; }
//          public Nullable<DateTime> date_accepted { get; set; }

//          public bool shipped { get; set; }
//          public string status { get; set; }
//          public Nullable<DateTime> date_shipped { get; set; }*/
//    }
//}