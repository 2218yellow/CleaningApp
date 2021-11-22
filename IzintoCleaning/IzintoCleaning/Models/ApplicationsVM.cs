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
    public class ApplicationsVM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int appicationID { get; set; }
        public int AppID { get; set; }
        public string AppName { get; set; }
        public string AppSurname { get; set; }
        public string AppEmail { get; set; }
        public string AppAdd { get; set; }
        public int SerCode { get; set; }
        public string PostDesc { get; set; }
        public DateTime PostClosingDate { get; set; }
    }
}