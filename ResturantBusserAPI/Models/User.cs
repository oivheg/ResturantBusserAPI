using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResturantBusserAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public String UserName { get; set; }
        //public String passwrd { get; set; }
        public String MasterID { get; set; }
        public Boolean Active { get; set; }
        public String AppId { get; set; }

    }
}