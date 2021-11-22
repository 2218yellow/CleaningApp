using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace IzintoCleaning.Models
{
    public class WorkDone

    {
        [Key]
        [DisplayName("EmployeeOrderId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkId { get; set; }
        [DisplayName("Employee")]
        public int EmpID { get; set; }
        //public virtual Employee Employee { get; set; }
        [DisplayName("Order")]
        public int Order_ID { get; set; }
        //public virtual CleaningOrder CleaningOrder { get; set;}
        public bool Status { get; set; }
        public TimeSpan time { get; set; }


        ApplicationDbContext db = new ApplicationDbContext();

        public void GetName()
        {
            var name = (from c in db.Employees
                        where c.EmpID == EmpID
                        select c.EmpName).Single();

            CleaningOrder cleaning = (from o in db.CleaningOrders
                                      where o.Order_ID == Order_ID
                                      select o).Single();
            cleaning.EmpmloyeeName = name;
            db.SaveChanges();

        }
        public void EmployeeStatus()
        {
            Employee employee = (from o in db.Employees
                                 where o.EmpID == EmpID
                                 select o).Single();
            employee.Status = "Busy";
            db.SaveChanges();
        }
        public void EmployeeDate()
        {
            Employee employee = (from o in db.Employees
                                 where o.EmpID == EmpID
                                 select o).Single();
            employee.Today = DateTime.Today;
            db.SaveChanges();
        }

    }
}