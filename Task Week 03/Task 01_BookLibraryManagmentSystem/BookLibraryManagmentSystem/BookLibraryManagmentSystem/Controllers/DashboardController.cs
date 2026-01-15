using BookLibraryManagmentSystem.Models;
using BookLibraryManagmentSystem.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookLibraryManagmentSystem.Controllers
{
    [DashboardSession]
    public class DashboardController : Controller
    {
        //ErrorLogMasterModel
        ErrorLogMasterModel elm = new ErrorLogMasterModel();

        public async Task<ActionResult> Index()
        {
            try
            {
                BookModel b = new BookModel();
                ViewBag.BookCount = await b.GetBookCount();
                ViewBag.AvailableBookCount = await b.GetAvailableBookCount();
                ViewBag.IssueBook = await b.GetIssueBookCount();
                ViewBag.ReturnBook = await b.GetReturnBookCount();
                return View();
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