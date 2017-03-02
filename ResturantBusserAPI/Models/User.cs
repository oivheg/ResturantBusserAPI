using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResturantBusserAPI.Models
{
    public class User
    {
        public String UserId { get; set; }
        public String UserName { get; set; }
        public int MasterID { get; set; }
        public int Active { get; set; }

    }
}