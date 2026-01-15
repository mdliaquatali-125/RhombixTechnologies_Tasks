using BookLibraryManagmentSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BookLibraryManagmentSystem.Models
{
    public class LoginModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime EditDate { get; set; }
        public string EditBy { get; set; }

        LibraryBookManagmentSystemdbEntities db = new LibraryBookManagmentSystemdbEntities();
        public async Task<int> CheckLogin()
        {
            int result = 0;

            var CheckUser = await (from um in db.UserMasters
                                   where um.UserName == UserName &&
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

        public static bool Logout()
        {
            return DataHelper.delete_Session();
        }
    }
}