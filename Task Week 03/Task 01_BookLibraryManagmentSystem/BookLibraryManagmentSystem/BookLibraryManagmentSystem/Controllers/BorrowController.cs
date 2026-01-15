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
    public class BorrowController : Controller
    {
        ErrorLogMasterModel elm = new ErrorLogMasterModel();

        public async Task<ActionResult> BorrowHistory()
        {
            try
            {
                BorrowModel b = new BorrowModel();
                var result = await b.GetBorrowList();
                return View(result);
            }
            catch (Exception ex)
            {
                elm.Add(
                       ex.Message == null ? "No Message" : ex.Message,
                       ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                       DateTime.Now,
                       HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                       "BorrowController", "BorrowHistory"
                   );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }

        [HttpGet]
        public async Task<ActionResult> BorrowAdd()
        {
            try
            {
                BookModel b = new BookModel();
                var list = await b.GetBookTitleList();
                ViewBag.BookTitle = new SelectList(list, "BookID", "Title");
                return View();
            }
            catch (Exception ex)
            {
                elm.Add(
                       ex.Message == null ? "No Message" : ex.Message,
                       ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                       DateTime.Now,
                       HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                       "BorrowController", "BorrowAdd"
                   );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }

        }

        [HttpPost]
        public async Task<ActionResult> BorrowAdd(BorrowModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int result = await model.AddBorrow(model);

                    if (result == 1)
                    {
                        return Json(new
                        {
                            Status = "Success",
                            Message = "Borrow added successfully.",
                            URL = "/Borrow/BorrowHistory"
                        });
                    }
                    else if (result == 0)
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Borrow already exists.",
                            URL = "/Borrow/BorrowAdd"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Failed to add borrow. Please try again.",
                            URL = "/Borrow/BorrowAdd"
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Validation failed.",
                        URL = "/Borrow/BorrowAdd"
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
                       "BorrowController", "BorrowAdd"
                   );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult> BorrowEdit(int BorrowID)
        {
            try
            {
                BorrowModel b = new BorrowModel();
                var result = await b.BorrowEdit(BorrowID);

                BookModel _b = new BookModel();
                var list = await _b.GetBookTitleList();
                ViewBag.BookTitle = new SelectList(list, "BookID", "Title");

                return View(result);
            }
            catch (Exception ex)
            {
                elm.Add(
                        ex.Message == null ? "No Message" : ex.Message,
                        ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                        DateTime.Now,
                        HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                        "BorrowController", "BorrowEdit"
                    );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }

        [HttpPost]
        public async Task<ActionResult> BorrowEdit(BorrowModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    int result = await model._BorrowEdit(model);

                    if (result == 1)
                    {
                        return Json(new
                        {
                            Status = "Success",
                            Message = "Borrow updated successfully.",
                            URL = "/Borrow/BorrowHistory"
                        });
                    }
                    else if (result == 2)
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Borrow already exists.",
                            URL = "/Borrow/BorrowEdit?BorrowID=" + model.BorrowID
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Failed to update borrow. Please try again.",
                            URL = "/Borrow/BorrowEdit?BorrowID=" + model.BorrowID
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Validation failed.",
                        URL = "/Borrow/BorrowEdit?BorrowID=" + model.BorrowID
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
                        "BorrowController", "BorrowEdit"
                    );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }
        public async Task<ActionResult> Delete(int BorrowID)
        {
            try
            {
                BorrowModel b = new BorrowModel();

                int result = await b.Delete(BorrowID);
                if (result == 1)
                {
                    return Json(new
                    {
                        Status = "Success",
                        Message = "Borrow deleted successfully.",
                        URL = "/Borrow/BorrowHistory"
                    });
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Failed to delete borrow. Please try again.",
                        URL = "/Borrow/BorrowHistory"
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
                       "BorrowController", "Delete"
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