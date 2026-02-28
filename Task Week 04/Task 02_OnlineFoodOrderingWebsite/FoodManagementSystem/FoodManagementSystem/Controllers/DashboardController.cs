using FoodManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FoodManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        //ErrorLogMasterModel
        ErrorLogMasterModel elm = new ErrorLogMasterModel();

        public async Task<ActionResult> Index()
        {
            try
            {
                FoodModel f = new FoodModel();
                var result = await f.GetFoodList();
                return View(result);
            }
            catch (Exception ex)
            {
                elm.Add(
                       ex.Message == null ? "No Message" : ex.Message,
                       ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                       DateTime.Now,
                       HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                       "DashboardController", "Index"
                   );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }

    }
}