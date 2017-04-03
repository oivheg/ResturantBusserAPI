using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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


            using (var dbCtx = new ApiDbContext())
            {
                //Add student object into Student's EntitySet
                dbCtx.Users.Add(User);
                // call SaveChanges method to save student & StudentAddress into database
                dbCtx.SaveChanges();
            }

            return Ok(User.UserId);
        }

        //Creates the master user
        public IHttpActionResult CreateMaster(Master master)
        {

            using (var dbCtx = new ApiDbContext())
            {
                master.RegisterDate = DateTime.Today;
                //Add student object into Student's EntitySet
                dbCtx.Masters.Add(master);
                // call SaveChanges method to save student & StudentAddress into database
                dbCtx.SaveChanges();
            }

            return Ok(master.MasterKey);
        }

        // This should only be used by the MASTER and not the busser app.
        // This Should GET all users that are currently active, this should only return the ones that are registered on the same MasterId. 
        [HttpGet]
        public List<User> GetAllActiveusers(String masterKey)
        {

            //             List<User> list =  dbContext.Users.ToList();
            List<User> list = new List<User>();
            foreach (var user in dbContext.Users)
            {
                var tmpUMID = user.MasterKey.ToString().Trim().ToLower();
                var active = user.Active;

                // here we crate teh jsson array, so we area ble to use the data in the app.
                var tmp1 = masterKey.ToString().Trim().ToLower(); ;

                if (tmpUMID == tmp1 && active)
                {
                    list.Add(user);
                }

            }


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
        // This is used to veiw one user, and might be used by the busser to shwo the current user to the user. [FromBody]
        [HttpGet]
        public IHttpActionResult FindUser(String userName)
        {
            User user = null;
            using (var context = new ApiDbContext())
            {
                var query = from p in context.Users
                            where p.UserName.Trim() == userName.ToLower()
                            select p;

                // This will raise an exception if entity not found
                // Use SingleOrDefault instead
                user = query.Single();
                user.UserName = user.UserName.Trim();

                Console.WriteLine(user);
            }

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
        public IHttpActionResult DinnerForAll(String mstrKey)
        {
            String _AppID = "cCuGQAtZxWQ:APA91bF286hnYN_OYYdOjPg4noCg2cIpfwRwRLssPE0O63so0UZowaqUhcpLgPAMBrXiFakdogiSgviyJ0Nx7OHwJr03u9AS-IopRNikTwfw7UhOFJJuTivVuLw7z-aD9Lty9g8H_bcd";
            ;




            foreach (var user in dbContext.Users)
            {
                if (mstrKey == user.MasterKey)
                {
                    FCMClient(user.AppId);
                }


            }

            // FCMRElay();
            var usd = dbContext.Users;




            return Ok();
        }

        [HttpPost]
        public IHttpActionResult DinnerisReady(User user)
        {
            //String _AppID = "cCuGQAtZxWQ:APA91bF286hnYN_OYYdOjPg4noCg2cIpfwRwRLssPE0O63so0UZowaqUhcpLgPAMBrXiFakdogiSgviyJ0Nx7OHwJr03u9AS-IopRNikTwfw7UhOFJJuTivVuLw7z-aD9Lty9g8H_bcd";
            String _AppID;

            // FCMRElay();
            // var usd = dbContext.Users.Find(user.UserName);
            User myUser = dbContext.Users.SingleOrDefault(_user => _user.UserName == user.UserName);
            _AppID = myUser.AppId;

            System.Diagnostics.Debug.Write("this is ClientAppId after database " + _AppID);
            return Ok(FCMClient(_AppID));
        }

        [HttpPost]
        public IHttpActionResult ClientAppId(User user)
        {

            var usd = dbContext.Users.Find(user.UserId);
            usd.AppId = user.AppId;


            dbContext.Entry(usd).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChangesAsync();

            return Ok();
        }

        public IHttpActionResult MasterAppId(Master master)
        {

            Master mstr = null;


            using (var context = new ApiDbContext())
            {
                var query = from p in context.Masters
                            where p.MasterKey.ToLower().Trim() == master.MasterKey.ToLower().Trim()
                            select p;

                // This will raise an exception if entity not found
                // Use SingleOrDefault instead
                mstr = query.Single();
                mstr.AppId = master.AppId.Trim();

            }


            dbContext.Entry(mstr).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChangesAsync();

            return Ok();
        }

        static String FCMClient(String AppID)
        {

            String json = ("{ \"data\": { \"title\": \"Maten er Ferdig\",\"body\":\"Bord 35 er klar for henting\", \"sound\": \"default\"  },\"to\" : \"" + AppID + "\"}");
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers.Add("Authorization", "key=AAAAvL8vPx4:APA91bFmAuSguRJ2PCcnXo14yeVePTbAa21nyNoQwn5JPJn9Bc9VPW8LvbG4I7On3JDLNnl-hKkxjCiDxV7vLRSYkT4PUN1BwrQBuKYdFQMiPNYyy_IBG3RvFbJKi1CjV2HzGHTSWwDd");
                var response = client.UploadString("https://fcm.googleapis.com/fcm/send", json);
                return response;
            }

        }

        static String FCMMaster(String AppID)
        {

            String json = ("{ \"data\": { \"Refresh\": \"true\" },\"to\" : \"" + AppID + "\"}");
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers.Add("Authorization", "key=AAAALvVIaEA:APA91bEyyCkyg3vALRMMLh42nCPpN1OWzXl8GpZMWYuh7nzzY14rWcRVeNDEt9W3kLFhyKWMB7WbTJ7Sbh0YgiTPlBy8Is8-2P-zSpkDH6cbt10JTN_--oOIwuGf5h3uGCNpCWkGNW3y");
                var response = client.UploadString("https://fcm.googleapis.com/fcm/send", json);
                return response;
            }

        }
        // This is used by the user app to inform that the user is active or not.
        [HttpPost]
        public IHttpActionResult UserisActive(User user)
        {


            var usd = dbContext.Users.Find(user.UserId);
            var mstr = dbContext.Masters.Find(usd.MasterKey.Trim());


            //usd.UserName = user.UserName;
            usd.Active = user.Active;
            if (user.AppId != null)
            {
                usd.AppId = "";
            }
            //usd.password = user.password;
            // Inform master of change.
            if (mstr != null)
            {
                FCMMaster(mstr.AppId);
            }


            dbContext.Entry(usd).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChangesAsync();
            //FCMMaster();


            return Ok();
        }
        // This is a test function, and will use this to test different stuff.
        [HttpPost]
        public IHttpActionResult testUser(int id)
        {
            var user = dbContext.Users.Find(id);
            user.Active = false;

            dbContext.Entry(user).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChangesAsync();
            return Ok(user);
        }
    }
}
