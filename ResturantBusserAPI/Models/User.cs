using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ResturantBusserAPI.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public String UserName { get; set; }
        //public String passwrd { get; set; }
        public String MasterKey { get; set; }
        public Boolean Active { get; set; }
        public String AppId { get; set; }

    }
}