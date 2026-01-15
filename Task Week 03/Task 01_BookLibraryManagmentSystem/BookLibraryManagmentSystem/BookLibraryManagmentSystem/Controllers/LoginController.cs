using BookLibraryManagmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookLibraryManagmentSystem.Controllers
{
    public class LoginController : Controller
    {
        //ErrorLogMasterModel
        ErrorLogMasterModel elm = new ErrorLogMasterModel();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Validate(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int result = await model.CheckLogin();

                    if (result == 1)
                    {
                        string url = "/dashboard/Index";
                        return Json(new
                        {
                            Status = "Success",
                            Message = "User login successfully.",
                            URL = url
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Plese enter valid credentials.",
                            URL = "/Login/Index"
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Validation failed.",
                        URL = "/Login/Index"
                    });
                }
            }
            catch (Exception ex)
            {
                elm.Add(
                     ex.Message == null ? "No Message" : ex.Message,
                     ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                     DateTime.Now,
                     HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                     "LoginController", "Validate"
                 );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }

        public ActionResult Logout()
        {
            LoginModel.Logout();
            return RedirectToAction("Index");

        }
    }
}