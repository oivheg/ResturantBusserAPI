using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ResturantBusserAPI.DBA;
using ResturantBusserAPI.Models;

namespace ResturantBusserAPI.Controllers
{
    public class UserAPIController : ApiController
    {
        ApiDbContext dbContext = null;
        public UserAPIController()
        {
            dbContext = new ApiDbContext();


        }

        [HttpPost]
        public IHttpActionResult InsertStudent(User User)
        {

            dbContext.Users.Add(User);
            dbContext.SaveChangesAsync();

            return Ok(User.UserId);
        }
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            var list = dbContext.Users.ToList();

            return list;
        }

        [HttpGet]
        public IHttpActionResult DeleteUser(int id)
        {
            var user = dbContext.Users.Find(id);

            dbContext.Users.Remove(user);

            dbContext.SaveChangesAsync();

            return Ok(user.UserId + " " + user.UserName + " is deleted successfully.");

        }

        [HttpGet]
        public IHttpActionResult ViewUser(int id)
        {
            var user = dbContext.Users.Find(id);
            return Ok(user);
        }

        [HttpPost]
        public IHttpActionResult UpdatUser(User user)
        {

            var usd = dbContext.Users.Find(user.UserId);

            usd.UserName = user.UserName;
            //usd.password = user.password;

            
            dbContext.Entry(usd).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult UserisActive(User user)
        {

            var usd = dbContext.Users.Find(user.UserName);

            //usd.UserName = user.UserName;
            usd.Active = user.Active;
            //usd.password = user.password;


            dbContext.Entry(usd).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult testUser(int id)
        {
            var user = dbContext.Users.Find(id);
            user.Active =false;

            dbContext.Entry(user).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChangesAsync();
            return Ok(user);
        }
    }
}
