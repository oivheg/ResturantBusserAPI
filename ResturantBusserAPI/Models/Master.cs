using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResturantBusserAPI.Models
{
    public class Master
    {
        public String MasterID { get; set; }
        public DateTime RegisterDate { get; set; }
        public String Resturant { get; set; }
        public String Email { get; set; }
        public  char Phone { get; set; }
        public String Contact { get; set; }
        public String OrgNr { get; set; }
        public String AppId { get; set; }
    }
}