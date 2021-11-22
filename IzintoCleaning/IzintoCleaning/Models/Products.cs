using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace IzintoCleaning.Models
{
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        [DisplayName("Product name")]
        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }

        [DisplayName("Product type")]
        [Required(ErrorMessage = "Product type is required")]
        public string ProductType { get; set; }

        [DisplayName("Product quantity")]
        [Required(ErrorMessage = "Product quantity is required")]
        public string ProductQuantity { get; set; }

        [DisplayName("Product cost")]
        [Required(ErrorMessage = "Product cost is required")]
        public string ProductCost { get; set; }

        public string AdminID { get; set; }
        [ForeignKey("AdminID")]
        public virtual Admin admin { get; set; }
        public int SuppID { get; set; }
        [ForeignKey("SuppID")]
        public virtual Supplier supplier { get; set; }
    }
}