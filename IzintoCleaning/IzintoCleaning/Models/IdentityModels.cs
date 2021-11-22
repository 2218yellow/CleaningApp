using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace IzintoCleaning.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string CustName { get; set; }
        public string CustSurname { get; set; }
        public string CustPhone { get; set; }
        
        public string AdminName { get; set; }
        public string AdminSurname { get; set; }
        public string AdminPhoneNo { get; set; }
        public string AdminAddress { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            
    }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Admin> admin { get; set; }
        public DbSet<Customer> customer { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
       // public DbSet<CustomerService> CustomerService { get; set; }
        public DbSet<CustomerEquipment> customerequipment { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        //Role Management
        public DbSet<IdentityUserRole> UserInRole { get; set; }
       // public DbSet<ApplicationUser> appUsers { get; set; }
        public DbSet<ApplicationRole> appRoles { get; set; }
       // public DbSet<ServiceDesc> ServiceDescs { get; set; }
        public DbSet<Cart_Item> Cart_Items { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Cleaning> Cleanings { get; set; }
        public DbSet<Gardening> Gardenings { get; set; }
        public DbSet<Fumigation> Fumigations { get; set; }
        public DbSet<FumigationOrder> FumigationOrders { get; set; }
        public DbSet<GardeningOrder> GardeningOrders { get; set; }
        public DbSet<CleaningOrder> CleaningOrders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_Address> Order_Addresses { get; set; }
        public DbSet<Order_Item> Order_Items { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Employee>()
                 .Property(a => a.AdminID)
                 .HasColumnName("Added By");

            modelBuilder.Entity<IdentityUserLogin>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(u => u.RoleId);
        }

        public System.Data.Entity.DbSet<IzintoCleaning.Models.EmployeeOrder> EmployeeOrders { get; set; }

        public System.Data.Entity.DbSet<IzintoCleaning.Models.FumigationAssign> FumigationAssigns { get; set; }

        public System.Data.Entity.DbSet<IzintoCleaning.Models.GardenAssign> GardenAssigns { get; set; }

        public System.Data.Entity.DbSet<IzintoCleaning.Models.WorkDone> WorkDones { get; set; }

        public System.Data.Entity.DbSet<IzintoCleaning.Models.EquipmentOrder> EquipmentOrders { get; set; }
    }
}