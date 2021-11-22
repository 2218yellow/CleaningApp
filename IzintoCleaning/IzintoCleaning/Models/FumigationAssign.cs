using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IzintoCleaning.Models
{
    public class FumigationAssign
    {

        [Key]
        [DisplayName("EmployeeOrderId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FumigationAssignId { get; set; }
        [DisplayName("Employee")]
        public int EmpID { get; set; }
        //public virtual Employee Employee { get; set; }
        [DisplayName("Order")]
        public int Order_ID { get; set; }
        //public virtual CleaningOrder CleaningOrder { get; set;}

        ApplicationDbContext db = new ApplicationDbContext();

        public void GetName()
        {
            var name = (from c in db.Employees
                        where c.EmpID == EmpID
                        select c.EmpName).Single();

            FumigationOrder fumigation = (from o in db.FumigationOrders
                                          where o.Order_ID == Order_ID
                                      select o).Single();
            fumigation.EmpmloyeeName = name;
            db.SaveChanges();

        }
        public void GetConfirm()
        {


            FumigationOrder fumigation = (from c in db.FumigationOrders
                                      where c.Order_ID == Order_ID
                                      select c).Single();
            fumigation.Confirm = true;
            db.SaveChanges();

        }
        public string GetEmail()
        {

            var Custid = (from c in db.FumigationOrders
                          where c.Order_ID == Order_ID
                          select c.CustID).Single();
            var email = (from c in db.customer
                         where c.CustID == Custid
                         select c.Email).Single();

            return email;
        }

        public string GetCustomerName()
        {

            var Custid = (from c in db.FumigationOrders
                          where c.Order_ID == Order_ID
                          select c.CustID).Single();
            var name = (from c in db.customer
                        where c.CustID == Custid
                        select c.CustName).Single();

            return name;
        }

        public string GetEmployeeName()
        {

            var employeeName = (from c in db.Employees
                                where c.EmpID == EmpID
                                select c.EmpName).Single();


            return employeeName;
        }
        public string GetServiceName()
        {

            var package = (from c in db.FumigationOrders
                           where c.Order_ID == Order_ID
                           select c.Package_ID).Single();
            var serve = (from c in db.Cleanings
                         where c.Package_ID == package
                         select c.PackageName).Single();


            return serve;
        }
        public DateTime GetServiceDate()
        {

            var date = (from c in db.FumigationOrders
                        where c.Order_ID == Order_ID
                        select c.DateOfService).Single();


            return date;
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