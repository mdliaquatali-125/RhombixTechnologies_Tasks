using FoodManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FoodManagementSystem.Controllers
{
    public class ContactController : Controller
    {
        ErrorLogMasterModel elm = new ErrorLogMasterModel();
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Contact(ContactModel model)
        {
            try
            {
                int result = await model.ContactUS();

                if (result == 0)
                {
                    return Json(new
                    {
                        Status = "Error",
                    });
                }
                else {
                    return Json(new
                    {
                        Status = "Success",
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
                     "ContactController", "Contact"
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