using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace IzintoCleaning.Models
{
    public class Admin
    {
        [Key]
        public string AdminID { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Name is required")]
        public string AdminName { get; set; }

        [DisplayName("Surname")]
        [Required(ErrorMessage = "Surname is required")]
        public string AdminSurname { get; set; }

        [MaxLength(10, ErrorMessage = "Phone number must only be 10 digits long"), MinLength(10, ErrorMessage = "Phone number must be 10 digits long")]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number is required")]
        public string AdminPhoneNo { get; set; }

        [DisplayName("Address")]
        [Required(ErrorMessage = "Address is required")]
        public string AdminAddress { get; set; }
    }

    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SerCode { get; set; }

        [DisplayName("Service Name")]
        [Required(ErrorMessage = "Service name is required")]
        public string SerName { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName("Service Wage")]
        [Required(ErrorMessage = "Wage is required")]
        public decimal SerWage { get; set; }

        //[DataType(DataType.Currency)]
        //[DisplayName("Service Basic Cost")]
        //[Required(ErrorMessage = "Basic cost is required")]
        //public decimal SerBasic { get; set; }

        public string AdminID { get; set; }
        [ForeignKey("AdminID")]
        public virtual Admin admin { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();
    }

    //public class CustomerService
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int TransID { get; set; }

    //    public string CustID { get; set; }
    //    [ForeignKey("CustID")]
    //    public virtual Customer customer { get; set; }

    //    [DisplayName("Service")]
    //    public int DescID { get; set; }
    //    [ForeignKey("DescID")]
    //    public virtual ServiceDesc ServiceDesc { get; set; }
    
    //    [ForeignKey("EmpId")]
    //    public virtual Employee employee { get; set; }
    //    public int? EmpId { get; set; }

    //    [DisplayName("Basic Cost")]
    //    [DataType(DataType.Currency)]
    //    public decimal SerCost { get; set; }

    //    [DisplayName("Cost of ")]
    //    [DataType(DataType.Currency)]
    //    public decimal SerCodeCost { get; set; }

    //    [DisplayName("Total Amount")]
    //    [DataType(DataType.Currency)]
    //    public decimal FinalCost { get; set; }

    //    public string empStatus { get; set; }

    //    public string showStatus { get; set; }

    //    [Column("ServiceDate", TypeName = "datetime2")]
    //    [DisplayName("Date Of Service")]
    //    [DataType(DataType.Date)]
    //    [Required(ErrorMessage = "Please select the date you want the service perfomed")]
    //    public DateTime DateOfService { get; set; }


    //    ApplicationDbContext db = new ApplicationDbContext();

    //    public void Assign()
    //    {
            
    //        var asign = (from i in db.Employees where EmpId == i.EmpID select i.Status).SingleOrDefault();
    //        asign = "Not Available";
    //        db.SaveChanges();
    //    }

    //    public string getAssign()
    //    {
    //        var asign = (from i in db.Employees where EmpId == i.EmpID select i.Status).SingleOrDefault();
    //        return asign;
    //    }

    //    public decimal getDescCost()
    //    {
    //        var cost = (from i in db.ServiceDescs where DescID == i.DescID select i.CritCost).SingleOrDefault();
    //        return cost;
    //    }

    //    public decimal getSerCost()
    //    {
    //        var cost = (from i in db.ServiceDescs where DescID == i.DescID select i.SerBasic).SingleOrDefault();
    //        return cost;
    //    }

    //    public decimal calcFinal()
    //    {
    //        return getDescCost() + getSerCost();
    //    }

    //    public bool CheckDate()
    //    {
    //        bool result = true;

    //        if (DateOfService == DateTime.Now || DateOfService < DateTime.Now.Date)
    //        {
    //            result = false;
    //        }
    //        return result;
    //    }

    //}



    //public class ServiceDesc
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int DescID { get; set; }

    //    [Display(Name = "Criteria")]
    //    [Required(ErrorMessage = "Criteria is required")]
    //    public string CritName { get; set; }

    //    [DataType(DataType.Currency)]
    //    [Display(Name = "Cost")]
    //    [Required(ErrorMessage = "Cost is required")]
    //    public decimal CritCost { get; set; }

    //    public int SerCode { get; set; }
    //    [ForeignKey("SerCode")]
    //    public virtual Service Service { get; set; }

    //    [Display(Name = "Basic Dervice Cost")]
    //    [DataType(DataType.Currency)]
    //    public decimal SerBasic { get; set; }

    //    ApplicationDbContext db = new ApplicationDbContext();

    //    public decimal getSerCost()
    //    {
    //        var cost = (from i in db.Service where SerCode == i.SerCode select i.SerBasic).SingleOrDefault();
    //        return cost;
    //    }
    //}

    public class Customer
    {
        [Key]
        public string CustID { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        public string CustName { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Surname is required")]
        public string CustSurname { get; set; }

        [MaxLength(10, ErrorMessage = "Phone number must only be 10 digits long"), MinLength(10, ErrorMessage = "Phone number must be 10 digits long")]
        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Phone number is required")]
        public string CustPhone { get; set; }

        public string Email { get; set; }

    }

  /*  public class cleaning
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int cleaning_Id { get; set; }
        [Display(Name = "customer name")]
        [Required(ErrorMessage = "customer name is required")]
        public string custName { get; set; }
        [Display(Name = " customer Surname")]
        [Required(ErrorMessage = " customer Surname is required")]
        public string custSurname { get; set; }
        [Display(Name = "customer address")]
        [Required(ErrorMessage = "customers address is required")]
        public string custAddress { get; set; }
        [Display(Name = "Type Of Cleaning")]
        [Required(ErrorMessage = "Type Of Cleaning is required")]
        public string Typeofcleaning { get; set; }
        [Display(Name = "Number of rooms")]
        [Required(ErrorMessage = "Number of rooms is required")]
        public int numofrooms { get; set; }
        public double basicCost { get; set; }
       
        public double calcBasic()
        {
            double i = 0;
            {
                if(Typeofcleaning=="Standard")
                {
                    i = 750;
                }
                else if(Typeofcleaning=="Spring Cleaning")
                {
                    i = 1200;
                }
                return i;
            }
        }

        public double calcRooms()
        {
            double p = 0;
            if (numofrooms == 1)
            {
                p = calcBasic() + 350;
            }
            else if (numofrooms == 2)
            {
                p = calcBasic() + 650;
            }
            else if (numofrooms == 3)
            {
                p = calcBasic() + 900;
            }
            else if (numofrooms == 4)
            {
                p = calcBasic() + 1150;
            }
            else if (numofrooms == 5)
            {
                p = calcBasic() + 1400;
            }
            else if (numofrooms == 6)
            {
                p = calcBasic() + 1650;

            }
            else if (numofrooms == 7)
            {
                p = calcBasic() + 1900;
            }
            else if (numofrooms == 8)
            {
                p = calcBasic() + 2150;
            }
            else if (numofrooms == 9)
            {
                p = calcBasic() + 2400;
            }
            else if (numofrooms == 10)
            {
                p = calcBasic() + 2620;

            }

            return p;
        }
        }


        public class Fumigation
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int fumigation_Id { get; set; }
            [Display(Name = "customer name")]
            [Required(ErrorMessage = "customer name is required")]
            public string custName { get; set; }
            [Display(Name = " customer Surname")]
            [Required(ErrorMessage = " customer Surname is required")]
            public string custSurname { get; set; }
            [Display(Name = "customer address")]
            [Required(ErrorMessage = "customers address is required")]
            public string custAddress { get; set; }
            [Display(Name = "Pest Control type ")]
            [Required(ErrorMessage = "Pest Control type  is required")]
            public string pestControlType  { get; set; }
            [Display(Name = "Number of rooms")]
            [Required(ErrorMessage = "Number of rooms is required")]
            public int numofrooms { get; set; }
            public double basicCost { get; set; }
        }*/

    public class Equipment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EquipID { get; set; }

        [DisplayName("Equipment Name")]
        [Required(ErrorMessage = "Equipment name is required")]
        public string EquipName { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Equipment Type")]
        [Required(ErrorMessage = "Equipment type is required")]
        public string EquipType { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName("Equipment Cost")]
        [Required(ErrorMessage = "Equipment cost is required")]
        public decimal EquipCost { get; set; }

        public int SuppID { get; set; }
        [ForeignKey("SuppID")]
        public virtual Supplier supplier { get; set; }

        [Column("Added By")]
        [DisplayName("Added By")]
        public string AdminID { get; set; }
        [ForeignKey("AdminID")]
        public virtual Admin admin { get; set; }

        [Required(ErrorMessage = "Please enter how much stock you have")]
        [DisplayName("In Stock")]
        public int QuantityInStock { get; set; }

        //[Required(ErrorMessage = "Choose a picture to represent equipment")]
        [Display(Name = "Picture")]
        //[DataType(DataType.Upload)]
        public byte[] Picture { get; set; }

        public virtual ICollection<Cart_Item> Cart_Items { get; set; }
    }

    public class CustomerEquipment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransID { get; set; }

        [DisplayName("Equipment Name")]
        public int EquipID { get; set; }
        [ForeignKey("EquipID")]
        public virtual Equipment equipment { get; set; }

        [DisplayName("Your name")]
        public string CustID { get; set; }
        [ForeignKey("CustID")]
        public virtual Customer customer { get; set; }

        [Column("Cost")]
        [DataType(DataType.Currency)]
        [DisplayName("Cost Each")]
        public decimal EquipCost { get; set; }

        [DisplayName("Equipment Type")]
        public string EquipType { get; set; }

        [Range(1,20,  ErrorMessage = "You can order 1-20 pieces of equipment at a time")]
        [DisplayName("Quantity")]
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }

        [Column("Final Cost")]
        [DataType(DataType.Currency)]
        [DisplayName("Final Cost")]
        public decimal FinalCost { get; set; }

        [NotMapped]
        public string suppName { get; set; }

        [NotMapped]
        public string suppEmail { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();

        public string getSupplierEmail()
        {
            var email = (from i in db.Equipment where EquipID == i.EquipID select i.supplier.SuppEmail).SingleOrDefault();
            return email;
        }

        public string getSupplierName()
        {
            var name = (from i in db.Equipment where EquipID == i.EquipID select i.supplier.SuppName).SingleOrDefault();
            return name;
        }

        public decimal getPrice()
        {
            var price = (from i in db.Equipment where EquipID == i.EquipID select i.EquipCost).SingleOrDefault();
            return price;
        }

        public string getEquipType()
        {
            var type = (from i in db.Equipment where EquipID == i.EquipID select i.EquipType).SingleOrDefault();
            return type;
        }

        public decimal calcFinalCost()
        {
            return Quantity * getPrice();
        }

    }

    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SuppID { get; set; }

        public string AdminID { get; set; }
        [ForeignKey("AdminID")]
        public virtual Admin admin { get; set; }

        [DisplayName("Supplier Name")]
        [Required(ErrorMessage = "Supplier name is required")]
        public string SuppName { get; set; }

        [DisplayName("Supplier email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Supplier email is required")]
        public string SuppEmail { get; set; }

        [MaxLength(10, ErrorMessage = "Phone number must only be 10 digits long"), MinLength(10, ErrorMessage = "Phone number must be 10 digits long")]
        [DisplayName("Supplier phone")]
        [Required(ErrorMessage = "Supplier phone is required")]
        public string SuppPhone { get; set; }
    }
}