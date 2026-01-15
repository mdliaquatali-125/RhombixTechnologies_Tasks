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

    public class BookController : Controller
    {
        //ErrorLogMasterModel
        ErrorLogMasterModel elm = new ErrorLogMasterModel();
        
        public async Task<ActionResult> BookList()
        {
            try
            {
                BookModel b = new BookModel();
                var result = await b.GetBookList();
                return View(result);
            }
            catch (Exception ex)
            {
                elm.Add(
                       ex.Message == null ? "No Message" : ex.Message,
                       ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                       DateTime.Now,
                       HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                       "BookController", "BookList"
                   );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }

        [HttpGet]
        public ActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddBook(BookModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int result = await model.AddBook(model);

                    if (result == 1)
                    {
                        return Json(new
                        {
                            Status = "Success",
                            Message = "Book added successfully.",
                            URL = "/Book/BookList"
                        });
                    }
                    else if (result == 0)
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Book already exists.",
                            URL = "/Book/AddBook"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Failed to add book. Please try again.",
                            URL = "/Book/AddBook"
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Validation failed.",
                        URL = "/Book/AddBook"
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
                       "BookController", "AddBook"
                   );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditBook(int BookID)
        {
            try
            {
                BookModel b = new BookModel();
                var result = await b.EditBook(BookID);
                return View(result);
            }
            catch (Exception ex)
            {
                elm.Add(
                        ex.Message == null ? "No Message" : ex.Message,
                        ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                        DateTime.Now,
                        HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                        "BookController", "EditBook"
                    );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> EditBook(BookModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    int result = await model._EditBook(model);

                    if (result == 1)
                    {
                        return Json(new
                        {
                            Status = "Success",
                            Message = "Book updated successfully.",
                            URL = "/Book/BookList"
                        });
                    }
                    else if (result == 2)
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Book already exists.",
                            URL = "/Book/EditBook?BookID=" + model.BookID
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Failed to update book. Please try again.",
                            URL = "/Book/EditBook?BookID=" + model.BookID
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Validation failed.",
                        URL = "/Book/EditBook?BookID=" + model.BookID
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
                        "BookController", "EditBook"
                    );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }
        public async Task<ActionResult> Delete(int BookID)
        {
            try
            {
                BookModel b = new BookModel();

                int result = await b.Delete(BookID);
                if (result == 1)
                {
                    return Json(new
                    {
                        Status = "Success",
                        Message = "Book deleted successfully.",
                        URL = "/Book/BookList"
                    });
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Failed to delete book. Please try again.",
                        URL = "/Book/BookList"
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
                       "BookController", "Delete"
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