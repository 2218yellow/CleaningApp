using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IzintoCleaning.Models
{
    public class Customer_Service
    {

        private ApplicationDbContext dataContext;

        public Customer_Service()
        {
            this.dataContext = new ApplicationDbContext();
        }


        public List<Customer> GetCustomers()
        {
            return dataContext.customer.ToList();
        }
        public bool AddCustomer(Customer customer)
        {
            try
            {
                dataContext.customer.Add(customer);            
                dataContext.SaveChanges();                       
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool UpdateCustomer(Customer customer)
        {
            try
            {
                dataContext.Entry(customer).State = EntityState.Modified;
                dataContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
      /*  public Customer GetCustomer(string email)
        {
            return dataContext.customer.FirstOrDefault(x=>x.Email == email);
        }

       /* public string GetGender(string id_num)
        {
            if (Convert.ToInt16(id_num.Substring(7, 1)) >= 5)
                return "Male";
            else
                return "Female";
        }*/
    }
}
