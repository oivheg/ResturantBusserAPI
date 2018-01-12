using ResturantBusserAPI.Models;
using System.Data.Entity;

namespace ResturantBusserAPI.DBA
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext() : base("Connection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Master> Masters { get; set; }
        public DbSet<TimeStamp> TimeStamps { get; set; }
    }
}