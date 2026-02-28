using FoodManagementSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FoodManagementSystem.Models
{
    public class UsermasterModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime EditDate { get; set; }
        public string EditBy { get; set; }

        OnlineFoodOrderingSystemEntities db = new OnlineFoodOrderingSystemEntities();
       
        public async Task<int> CheckLogin()
        {
            int result = 0;

            var CheckUser = await (from um in db.UserMasters
                                   where (um.UserName == UserName || um.Email == UserName || um.Phone == UserName)&&
                                   um.UserPassword == UserPassword
                                   && um.IsActive == true
                                   select um).SingleOrDefaultAsync();

            if (CheckUser != null)
            {
                DataHelper.Set_Session(CheckUser);
                result = 1;
            }
            else
            {
                result = 0;
            }
            return result;
        }

        public async Task<int> SignupCreate()
        {
            int result = 0;

            var exist = await db.UserMasters.Where(x => x.Email == Email.ToLower() && x.IsActive == true).CountAsync();

            if (exist > 0)
            {
                result = 0;
                return result;
            }

            UserMaster um = new UserMaster();

            um.UserName = UserName;
            um.Email = Email; 
            um.Phone = Phone;
            um.UserPassword = UserPassword;
            um.IsActive = true;
            um.CreatedBy = HttpContext.Current.Session["UserName"] == null ? "UnKnownUser" : HttpContext.Current.Session["UserName"].ToString();
            um.CreatedDate = DateTime.Now;
            db.UserMasters.Add(um);
            await db.SaveChangesAsync();
            result = 1;
            return result;
        }

        public static bool Logout()
        {
            return DataHelper.delete_Session();
        }
    }
}