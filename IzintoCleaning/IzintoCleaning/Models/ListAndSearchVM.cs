using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IzintoCleaning.Models
{
    public class ListAndSearchVM
    {
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public string SelectStatus { get; set; }
    }
}