using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace IzintoCleaning.Models
{

    public class PDF
    {
        [Key]
        public string FileId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public FileFormat FileFormat { get; set; }
        public int AppID { get; set; }
        [ForeignKey("AppID")]
        public virtual Applicant Applicant { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();

        public string getPhone()
        {
            var phone = (from i in db.Applicant where AppID == i.AppID select i.AppPhone).SingleOrDefault();
            return phone;
        }
    }
}