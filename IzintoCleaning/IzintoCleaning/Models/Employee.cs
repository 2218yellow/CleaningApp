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
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpID { get; set; }

        [DisplayName("Employee Name")]
        [Required(ErrorMessage = "Name is required")]
        public string EmpName { get; set; }

        [DisplayName("Employee Surname")]
        [Required(ErrorMessage = "Surname is required")]
        public string EmpSurame { get; set; }

        [DisplayName("Employee Email")]
        [Required(ErrorMessage = "Email is required")]
        public string EmpEmail { get; set; }

        [DisplayName("Employee Phone")]
        [MaxLength(10, ErrorMessage = "Phone number must only be 10 digits long"), MinLength(10, ErrorMessage = "Phone number must be 10 digits long")]
        [Required(ErrorMessage = "Phone is required")]
        public string EmpPhone { get; set; }

        [DisplayName("Employee Address")]
        [Required(ErrorMessage = "Address is required")]
        public string EmpAddress { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName("Expected Pay")]
        [Required(ErrorMessage = "Address is required")]
        public decimal Wage { get; set; }

        [Required(ErrorMessage = "Employee status is required")]
        [DisplayName("Employee Status")]
        public Availabity EmployeeStatus { get; set; }

        public string Status { get; set; }

        public IEnumerable<SelectListItem> Statuses { set; get; }

        public string AdminID { get; set; }
        [ForeignKey("AdminID")]
        public virtual Admin admin { get; set; }

        public int SerCode { get; set; }
        [ForeignKey("SerCode")]
        public virtual Service service { get; set; }
        public DateTime? Today { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();
        public decimal getWage()
        {
            var wage = (from i in db.Service where SerCode == i.SerCode select i.SerWage).SingleOrDefault();
            return wage;
        }

        public string Availabilty()
        {
            
            if (EmployeeStatus == Availabity.N)
            {
                Status = "Busy";

            }
            else if (EmployeeStatus == Availabity.Y)
            {
                Status = "Available";
            }

            return Status;
        }
    }

    public enum Availabity
        {
            Y,
            N
            
        }
    
}