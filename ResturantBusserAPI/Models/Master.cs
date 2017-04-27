using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ResturantBusserAPI.Models
{
    public class Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MasterID { get; set; }
        public DateTime RegisterDate { get; set; }
        public String Resturant { get; set; }
        public String Email { get; set; }
        public  Decimal Phone { get; set; }
        public String Contact { get; set; }
        public String OrgNr { get; set; }
        public String AppId { get; set; }
        public String MasterKey { get; set; }
    }
}