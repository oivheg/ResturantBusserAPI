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
        // CreateUser are used by busser app to create a user, a new user are only added if the user input a correct master ID:
        [HttpPost]
        public IHttpActionResult CreateUser(User User)
        {

            dbContext.Users.Add(User);
            dbContext.SaveChangesAsync();

            return Ok(User.UserId);
        }
        // This should only be used by the MASTER and not the busser app.
        // This Should GET all users that are currently active, this should only return the ones that are registered on the same MasterId. 
        [HttpGet]
        public IEnumerable<User> GetAllActiveusers()
        {
            var list = dbContext.Users.ToList();

            return list;
        }
        // DeleteUser, right now this are not used.
        [HttpGet]
        public IHttpActionResult DeleteUser(int id)
        {
            var user = dbContext.Users.Find(id);

            dbContext.Users.Remove(user);

            dbContext.SaveChangesAsync();

            return Ok(user.UserId + " " + user.UserName + " is deleted successfully.");

        }
        // This is used to veiw one user, and might be used by the busser to shwo the current user to the user.
        [HttpGet]
        public IHttpActionResult ViewUser(int id)
        {
            var user = dbContext.Users.Find(id);
            return Ok(user);
        }
        // used to update, should be done in the User APP, 
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
        // This is used by the user app to inform that the user is active or not.
        [HttpPost]
        public IHttpActionResult UserisActive(User user)
        {


            var usd = dbContext.Users.Find(user.UserId);
            
            

            //usd.UserName = user.UserName;
            usd.Active = user.Active;
            //usd.password = user.password;


            dbContext.Entry(usd).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChangesAsync();

            return Ok();
        }
        // This is a test function, and will use this to test different stuff.
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
