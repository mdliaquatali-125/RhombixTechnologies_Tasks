using FoodManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FoodManagementSystem.Controllers
{
    public class SigninController : Controller
    {
        ErrorLogMasterModel elm = new ErrorLogMasterModel();
        // GET: Signin
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Validate(UsermasterModel model)
        {
            try
            {
                string url = "";

                if (ModelState.IsValid)
                {
                    int result = await model.CheckLogin();
                    string msg = "";
                    if (result == 1)
                    {
                        if (model.UserName == "Admin")
                        {
                            url = "/Admin/Index";
                            msg = "Admin Signin successfully.";
                        }
                        else
                        {
                           url = "/dashboard/Index";
                            msg = "User Signin successfully.";
                        }

                        return Json(new
                        {
                            Status = "Success",
                            Message = msg,
                            URL = url
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Plese enter valid credentials.",
                            URL = "/Signin/Index"
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Validation failed.",
                        URL = "/Signin/Index"
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
                     "SigninController", "Validate"
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
           UsermasterModel.Logout();
            return RedirectToAction("Index", "Dashboard");
        }

    }
}