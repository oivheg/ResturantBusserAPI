using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ResturantBusserAPI.Models
{
    public class TimeStamp
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid TimeStampID { get; set; }
        public DateTime? Inn { get; set; }
        public DateTime? Out { get; set; }
        public Guid UserGUID { get; set; }

    }
}