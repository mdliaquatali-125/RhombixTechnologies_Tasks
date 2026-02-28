using FoodManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FoodManagementSystem.Controllers
{
    public class SignupController : Controller
    {
        ErrorLogMasterModel elm = new ErrorLogMasterModel();
        // GET: Signup
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Signup(UsermasterModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    int result = await model.SignupCreate();

                    if (result == 1)
                    {
                        return Json(new
                        {
                            Status = "Success",
                            Message = "User Signup successfully.",
                            URL = "/Signin/Index"
                        });
                    }
                    else if (result == 0)
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Email already exists.",
                            URL = "/Signup/Index"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Failed to Signup. Please try again.",
                            URL = "/Signup/Index"
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Validation failed.",
                        URL = "/Signup/Index"
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
                     "SignupController", "Validate"
                 );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }
    }
}