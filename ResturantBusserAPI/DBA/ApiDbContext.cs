using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ResturantBusserAPI.Models;

namespace ResturantBusserAPI.DBA
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext() : base("Connection")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Master> Masters { get; set; }
    }

}