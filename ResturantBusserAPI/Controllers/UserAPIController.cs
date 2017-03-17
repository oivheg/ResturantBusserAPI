using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ResturantBusserAPI.DBA;
using ResturantBusserAPI.Models;
using System.Net.Http.Headers;

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

        [HttpPost]
        public IHttpActionResult DinnerisReady()
        {

           // PostRequest();

                return Ok(PostRequest());
        }

        [HttpPost]
        public IHttpActionResult AppId(User user)
        {

            var usd = dbContext.Users.Find(user.UserId);
            usd.AppId = user.AppId;

            return Ok();
        }

         static String PostRequest()
        {
            IEnumerable<KeyValuePair<String, String>> queries = new List<KeyValuePair<String, String>>()
            {
                new KeyValuePair<string, string>("Content-Type", "application/json"),
                new KeyValuePair<string, string>("Authorization", "key=AAAAvL8vPx4:APA91bFmAuSguRJ2PCcnXo14yeVePTbAa21nyNoQwn5JPJn9Bc9VPW8LvbG4I7On3JDLNnl-hKkxjCiDxV7vLRSYkT4PUN1BwrQBuKYdFQMiPNYyy_IBG3RvFbJKi1CjV2HzGHTSWwDd"),
            };

          
             String json = ("{ \"notification\": { \"title\": \"Maten er Ferdig\",\"body\":\"Bord 35 er klar for henting\"  },\"to\" : \"cCuGQAtZxWQ:APA91bF286hnYN_OYYdOjPg4noCg2cIpfwRwRLssPE0O63so0UZowaqUhcpLgPAMBrXiFakdogiSgviyJ0Nx7OHwJr03u9AS-IopRNikTwfw7UhOFJJuTivVuLw7z-aD9Lty9g8H_bcd\"}") ;

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers.Add("Authorization", "key=AAAAvL8vPx4:APA91bFmAuSguRJ2PCcnXo14yeVePTbAa21nyNoQwn5JPJn9Bc9VPW8LvbG4I7On3JDLNnl-hKkxjCiDxV7vLRSYkT4PUN1BwrQBuKYdFQMiPNYyy_IBG3RvFbJKi1CjV2HzGHTSWwDd");

              

                 var response = client.UploadString("https://fcm.googleapis.com/fcm/send", json);
                return response;
            }
            
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
