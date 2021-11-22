using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace IzintoCleaning.Models
{
    public class Cleaning
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Package_ID { get; set; }

        public int SerCode { get; set; }
        [ForeignKey("SerCode")]
        public virtual Service service { get; set; }

        [Required]
        [MinLength(5)]
       public string PackageName { get; set; }

        [DataType(DataType.Currency)]
        [Required]
        public decimal CostArea { get; set; }

        [DataType(DataType.Currency)]
        [Required]
        public decimal CostStorey { get; set; }
    }
}