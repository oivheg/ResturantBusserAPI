using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using ResturantBusserAPI.DBA;
using ResturantBusserAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ResturantBusserAPI.Controllers
{
    public class UserAPIController : ApiController
    {
        private ApiDbContext dbContext = null;

        public UserAPIController()
        {
            dbContext = new ApiDbContext();
        }

        // CreateUser are used by busser app to create a user, a new user are only added if the user input a correct master ID:
        [HttpPost]
        public async Task<IHttpActionResult> CreateUser(User User)
        {
            using (var dbCtx = new ApiDbContext())
            {
                //Add student object into Student's EntitySet
                dbCtx.Users.Add(User);
                // call SaveChanges method to save student & StudentAddress into database
                await dbCtx.SaveChangesAsync();
            }

            return Ok(User.UserId);
        }

        //Creates the master user
        public async Task<IHttpActionResult> CreateMaster(Master master)
        {
            using (var dbCtx = new ApiDbContext())
            {
                master.RegisterDate = DateTime.Today;
                //Add student object into Student's EntitySet
                dbCtx.Masters.Add(master);
                // call SaveChanges method to save student & StudentAddress into database
                //dbContext.Entry(master).State = System.Data.Entity.EntityState.Added;
                await dbCtx.SaveChangesAsync();
            }

            return Ok(master.MasterKey);
        }

        // This should only be used by the MASTER and not the busser app.
        // This Should GET all users that are currently active, this should only return the ones that are registered on the same MasterId.
        [HttpGet]
        public List<User> GetAllActiveusers(String Appid)
        {
            //             List<User> list =  dbContext.Users.ToList();
            List<User> list = new List<User>();
            Master mstr = dbContext.Masters.SingleOrDefault(_Master => _Master.AppId == Appid);
            var returnValue = (from e in dbContext.Users
                               join o in dbContext.TimeStamps on e.UserGuid equals o.UserGUID
                               select new
                               {
                                   e.UserName,
                                   e.MasterKey,
                                   e.Active,
                                   o.Inn
                               });

            returnValue = returnValue.OrderBy(x => x.Inn);
            foreach (var user in returnValue)
            {
                var tmpUMID = user.MasterKey.ToString().Trim().ToLower();
                var active = user.Active;

                // here we crate teh jsson array, so we area ble to use the data in the app.

                if (tmpUMID == mstr.MasterKey.ToLower() && active)
                {
                    User tmpUser = new User();
                    tmpUser.UserName = user.UserName;
                    list.Add(tmpUser);
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
        public IHttpActionResult FindUser(String Appid, String Email = "")
        {
            User user = null;
            String tmpuser = null;
            using (var context = new ApiDbContext())
            {
                if (Email != "")
                {
                    var query = from p in context.Users
                                where p.Email.Trim() == Email
                                select p;
                    user = query.SingleOrDefault();
                    user.AppId = Appid;
                }
                else
                {
                    //runs stored procedures on sql serer, gets the user based on appid
                    //var query1 = context.Database.SqlQuery<string>(
                    //     "select * from Finduser('" + Appid + "')");
                    //tmpuser = query1.SingleOrDefault();

                    var query = from p in context.Users
                                where p.AppId.Trim() == Appid.Trim()
                                select p;
                    user = query.SingleOrDefault();
                }

                // This will raise an exception if entity not found
                // Use SingleOrDefault instead
                //user.UserName = tmpuser;
                user.UserName = user.UserName.Trim();
            }
            dbContext.Entry(user).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChangesAsync();
            return Ok(user);
        }

        // used to update, should be done in the User APP,
        [HttpPost]
        public async Task<IHttpActionResult> UpdatUser(User user)
        {
            User usd = null;

            using (var context = new ApiDbContext())
            {
                var query = from p in context.Users
                            where p.Email.ToLower().Trim() == user.Email.ToLower().Trim()
                            select p;

                // This will raise an exception if entity not found
                // Use SingleOrDefault instead
                usd = query.Single();
            }

            usd.AppId = user.AppId;
            //usd.password = user.password;

            dbContext.Entry(usd).State = System.Data.Entity.EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Msgreceived(User user)
        {
            String AppId = user.AppId;

            User myUser = dbContext.Users.SingleOrDefault(_user => _user.AppId.Trim() == AppId.Trim());

            Master mstr = null;

            using (var context = new ApiDbContext())
            {
                var query = from p in context.Masters
                            where p.MasterKey.ToLower().Trim() == myUser.MasterKey.ToLower().Trim()
                            select p;

                // This will raise an exception if entity not found
                // Use SingleOrDefault instead
                mstr = query.Single();
            }
            String MstrAppId = "";
            byte[] bytes = Encoding.UTF8.GetBytes(myUser.UserName);
            String tmpUserName = Encoding.UTF8.GetString(bytes);
            MstrAppId = mstr.AppId;
            SendDataToMaster(MstrAppId, "recieved", tmpUserName);

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult DinnerForAll(String mstrKey)
        {
            String _AppID = "cCuGQAtZxWQ:APA91bF286hnYN_OYYdOjPg4noCg2cIpfwRwRLssPE0O63so0UZowaqUhcpLgPAMBrXiFakdogiSgviyJ0Nx7OHwJr03u9AS-IopRNikTwfw7UhOFJJuTivVuLw7z-aD9Lty9g8H_bcd";
            ;

            foreach (var user in dbContext.Users)
            {
                if (mstrKey.Trim().ToLower() == user.MasterKey.Trim().ToLower() && user.Active == true)
                {
                    FCMClient(user.AppId);
                }
            }

            // FCMRElay();
            var usd = dbContext.Users;

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult CancelDinnerForAll(String mstrKey)
        {
            foreach (var user in dbContext.Users)
            {
                if (mstrKey.Trim().ToLower() == user.MasterKey.Trim().ToLower() && user.Active == true)
                {
                    FCMClient(user.AppId, true);
                }
            }

            // FCMRElay();
            var usd = dbContext.Users;

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult CancelDinner(User user)
        {
            //String _AppID = "cCuGQAtZxWQ:APA91bF286hnYN_OYYdOjPg4noCg2cIpfwRwRLssPE0O63so0UZowaqUhcpLgPAMBrXiFakdogiSgviyJ0Nx7OHwJr03u9AS-IopRNikTwfw7UhOFJJuTivVuLw7z-aD9Lty9g8H_bcd";
            String _AppID;

            User myUser = dbContext.Users.SingleOrDefault(_user => _user.UserName == user.UserName);
            _AppID = myUser.AppId;

            System.Diagnostics.Debug.Write("this is ClientAppId after database " + _AppID);
            return Ok(FCMClient(_AppID, true));
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
            var usd = dbContext.Users.SingleOrDefault(_user => _user.UserId == user.UserId);
            usd.AppId = user.AppId;

            dbContext.Entry(usd).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult MasterAppId(Master master)
        {
            Master mstr;

            if (master.Email != null)
            {
                //var query1 = from p in dbContext.Masters
                //            where p.Email.ToLower().Trim() == master.Email.ToLower().Trim()
                //            select p;
                mstr = dbContext.Masters.SingleOrDefault(_master => _master.Email.ToLower().Trim() == master.Email.ToLower().Trim());
                mstr.AppId = master.AppId.Trim();
            }
            else
            {
                //var query = from p in dbContext.Masters
                //            where p.AppId.ToLower().Trim() == master.AppId.ToLower().Trim()
                //            select p;
                //mstr = query.SingleOrDefault();
                mstr = dbContext.Masters.SingleOrDefault(_master => _master.AppId == master.AppId);
            }

            // This will raise an exception if entity not found
            // Use SingleOrDefault instead

            if (mstr == null)
            {
                return NotFound();
            }

            dbContext.Entry(mstr).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChangesAsync();

            return Ok(mstr.MasterKey);
        }

        [HttpPost]
        public IHttpActionResult ChckKey(Master mKey)
        {
            Master mstr;

            mstr = dbContext.Masters.SingleOrDefault(_master => _master.MasterKey.Trim().ToLower() == mKey.MasterKey.Trim().ToLower());

            // This will raise an exception if entity not found
            // Use SingleOrDefault instead

            if (mstr == null)
            {
                return NotFound();
            }

            return Ok(true);
        }

        private static String FCMClient(String AppID, Boolean CancelVib = false)
        {
            String json = "";
            if (CancelVib)
            {
                String value = "cancelVibration";
                json = ("{ \"data\": { \"Action\": \"" + value + "\" }, \"content_available\" : true,\"priority\":\"high\",  \"to\": \"" + AppID + "\"}");
                //IOS //json = ("{ \"data\": { \"Action\": \"" + value + "\" },  \"content-available\":\"true\",\"priority\":\"high\",  \"notification\":{ \"sound\": \" \" }, \"to\": \"" + AppID + "\"}");
            }
            else
            {
                json = ("{ \"data\": { \"title\": \"Maten er Ferdig\",\"body\":\"Bord 35 er klar for henting\", \"sound\": \"default\"  },\"content_available\" : true, \"priority\":\"high\" , \"to\" : \"" + AppID + "\"}");
                // IOS  //json = ("{ \"data\": { \"title\": \"Maten er Ferdig\",\"body\":\"Bord 35 er klar for henting\", \"sound\": \"default\"  }, \"content-available\":\"true\", \"priority\":\"high\"  \"notification\":{\"sound\": \" \" } ,\"to\" : \"" + AppID + "\"}");
            }

            return SendData(json);
        }

        //private static String Master_Key = "AAAALvVIaEA:APA91bEyyCkyg3vALRMMLh42nCPpN1OWzXl8GpZMWYuh7nzzY14rWcRVeNDEt9W3kLFhyKWMB7WbTJ7Sbh0YgiTPlBy8Is8-2P-zSpkDH6cbt10JTN_--oOIwuGf5h3uGCNpCWkGNW3y";

        //private static String Busser_key = "AAAAvL8vPx4:APA91bFmAuSguRJ2PCcnXo14yeVePTbAa21nyNoQwn5JPJn9Bc9VPW8LvbG4I7On3JDLNnl-hKkxjCiDxV7vLRSYkT4PUN1BwrQBuKYdFQMiPNYyy_IBG3RvFbJKi1CjV2HzGHTSWwDd";
        private static String Busser_key = "AAAA_HJQnFk:APA91bFYl-1n55IAPiV7p4Wdl8K6nROOB7m6VtQdpWsWmGT6H4CE3TwAnVoYSnD3HyJQkGQKOtjoYFikqvmpV6vXahLVGjgkwTANVr1OOiTmHpIbP8ka8QPhjarxiAs2b_mvEnHWdfUl";

        private static String Master_Key = "AAAAOCJo_R8:APA91bE_wlFq4MNF2SG53F1Zf6MP2fdYbGUU0qoZFCXdkYYuu4UqdlSp7-MYayTevcm4MQTWv1B8v5GOjIJd8yxnK_T8oHPrFvifWJx0rNqd2dMwzja5nFVPYI7mn3MqkEh8wiEKvU7s";

        private static string SendData(string json)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers.Add("Authorization", "key=" + Busser_key);
                var response = client.UploadString("https://fcm.googleapis.com/fcm/send", json);
                return response;
            }
        }

        private static String FCMMaster(String AppID)
        {
            return SendDataToMaster(AppID, "refresh", "");
        }

        private static string SendDataToMaster(string AppID, string value, string user)
        {
            string tjson = ("{ \"data\": { \"Action\": \"" + value + "\",\"user\": \"" + user.Trim() + "\",}\"to\" : \"" + AppID + "\"}");

            //string tjson = ("{ \"data\": { \"Action\": \"" + value + "\",\"user\":\"" + user + "\" },\"to\" : \"" + AppID + "\"}");

            using (WebClient client = new WebClient())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(tjson);
                String tmpUserName = Encoding.UTF8.GetString(bytes);
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                // client.Headers[HttpRequestHeader.ContentLength] = bytes.Length.ToString();

                client.Headers.Add("Authorization", "key=" + Master_Key);
                return client.UploadString("https://fcm.googleapis.com/fcm/send", tjson);
            }
        }

        // This is used by the user app to inform that the user is active or not.
        [HttpPost]
        public IHttpActionResult UserisActive(User user, Boolean logout = false)
        {
            Master mstr = null;
            TimeStamp Tsmp = null;

            //User usd = dbContext.Users.SingleOrDefault(_user => _user.UserName.Trim() == user.UserName.Trim());
            //var usd = dbContext.Users.Find(user.UserId);
            var usd = dbContext.Users.SingleOrDefault(_user => _user.AppId.ToLower().Trim() == user.AppId.ToLower().Trim());

            //// var mstr = dbContext.Masters.Find(usd.MasterKey.Trim());

            //need to prevent aldready loggen in user from logging in again, either "login out" fucntion or just oversee. porblem lies with if there is multiple timestamps.

            // probalby just check if ther is exsisting timestamp and just use that .

            if (user.Active)
            {
                Tsmp = new TimeStamp();
                Tsmp.UserGUID = usd.UserGuid;
                Tsmp.Inn = DateTime.Now;
                dbContext.TimeStamps.Add(Tsmp);
            }
            else
            {
                using (var context = new ApiDbContext())
                {
                    var query = from p in context.TimeStamps
                                where p.UserGUID == usd.UserGuid
                                select p;

                    // This will raise an exception if entity not found
                    // Use SingleOrDefault instead
                    Tsmp = query.SingleOrDefault();
                }

                Tsmp.Out = DateTime.Now;

                dbContext.Entry(Tsmp).State = System.Data.Entity.EntityState.Modified;
            }

            using (var context = new ApiDbContext())
            {
                var query = from p in context.Masters
                            where p.MasterKey.ToLower().Trim() == usd.MasterKey.Trim().ToLower()
                            select p;

                // This will raise an exception if entity not found
                // Use SingleOrDefault instead
                mstr = query.Single();
            }

            //usd.UserName = user.UserName;
            usd.Active = user.Active;
            if (logout)
            {
                usd.AppId = "  ";
            }
            //usd.password = user.password;
            // Inform master of change.
            if (mstr != null)
            {
                FCMMaster(mstr.AppId);
            }

            //Add  object into  EntitySet ( sql database)

            dbContext.Entry(usd).State = System.Data.Entity.EntityState.Modified;
            //Saves chagnes to entity Data, Sql database
            dbContext.SaveChangesAsync();
            //FCMMaster();

            return Ok();
        }

        // This is a test function, and will use this to test different stuff.
        [HttpPost]
        public IHttpActionResult testUser(string id)
        {
            var user = dbContext.Users.Find(id);
            user.Active = false;

            dbContext.Entry(user).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChangesAsync();
            return Ok(user);
        }
    }
}