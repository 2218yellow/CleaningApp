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
    public class FumigationOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Order_ID { get; set; }

        public int Package_ID { get; set; }
        [ForeignKey("Package_ID")]
        public virtual Fumigation Fumigation { get; set; }
        public bool Confirm { get; set; }

        public string CustID { get; set; }
        [ForeignKey("CustID")]
        public virtual Customer Customer { get; set; }
        public bool paymentstatus { get; set; }

        [Required(ErrorMessage = "Enter the area of the house to be fumigated")]
        [Range(30, 200, ErrorMessage = "Size of house must be between 30 and 200 square meters")]
        public decimal area { get; set; }

        [Required(ErrorMessage = "Enter the number of floors to be fumigated")]
        [Range(1, 4, ErrorMessage = "We Fumigate builidings with 1-4 floors")]
        public int storey { get; set; }

        [DataType(DataType.Currency)]
        public decimal cost { get; set; }

        [Column("ServiceDate", TypeName = "datetime2")]
        [DisplayName("Date Of Service")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please select the date you want the service perfomed")]
        public DateTime DateOfService { get; set; }

        [DisplayName("Cost Per 30 sq meter")]
        [DataType(DataType.Currency)]
        public decimal areacost { get; set; }

        [DisplayName("Cost Per Floor")]
        [DataType(DataType.Currency)]
        public decimal floorcost { get; set; }

        public string EmpmloyeeName { get; set; }

        public string address { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();

        //public void Assign()
        //{

        //    var asign = (from i in db.Employees where EmpId == i.EmpID select i.Status).SingleOrDefault();
        //    asign = "Not Available";
        //    db.SaveChanges();
        //}

        //public string getAssign()
        //{
        //    var asign = (from i in db.Employees where EmpId == i.EmpID select i.Status).SingleOrDefault();
        //    return asign;
        //}

        public decimal getCostArea()
        {
            var cost = (from i in db.Fumigations where Package_ID == i.Package_ID select i.CostArea).SingleOrDefault();
            return cost;
        }

        public decimal getFloorCost()
        {
            var cost = (from i in db.Fumigations where Package_ID == i.Package_ID select i.CostStorey).SingleOrDefault();
            return cost;
        }

        public decimal calcFinal()
        {
            decimal final;
            decimal a = (area / 30) * getCostArea();
            decimal s = storey * getFloorCost();
            final = a + s;
            return final;

        }
    }
}