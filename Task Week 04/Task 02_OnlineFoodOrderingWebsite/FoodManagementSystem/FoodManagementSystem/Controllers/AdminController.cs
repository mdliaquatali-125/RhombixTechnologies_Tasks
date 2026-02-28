using FoodManagementSystem.Models;
using FoodManagementSystem.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FoodManagementSystem.Controllers
{
    [DashboardSession]
    public class AdminController : Controller
    {
        ErrorLogMasterModel elm = new ErrorLogMasterModel();
        // GET: Admin
        public async Task<ActionResult> Index()
        {
            try
            {
                FoodModel f = new FoodModel();
                ViewBag.TodayOrder = await f.TodayOrder();
                ViewBag.PendingOrder = await f.PendingOrder();
                ViewBag.PackedOrder = await f.PackedOrder();
                ViewBag.OnthewayOrder = await f.OnthewayOrder();
                ViewBag.DeliveredOrder = await f.DeliveredOrder();
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
        [HttpGet]
        public ActionResult AddFood()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddFood(FoodModel model)
        {
            try
            {
                    int result = await model.AddFood(model);

                    if (result == 1)
                    {
                        return Json(new
                        {
                            Status = "Success",
                            Message = "Food added successfully.",
                            URL = "/Admin/AllFood"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Failed to add food. Please try again.",
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
                       "AdminController", "AddFood"
                   );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }
        public async Task<ActionResult> AllFood()
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
                       "AdminController", "AllFood"
                   );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }
        public async Task<ActionResult> Order()
        {
            try
            {
                OrderModel or = new OrderModel();
                var result = await or.GetOrderList();
                return View(result);
            }
            catch (Exception ex)
            {
                elm.Add(
                       ex.Message == null ? "No Message" : ex.Message,
                       ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                       DateTime.Now,
                       HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                       "AdminController", "Order"
                   );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }
        public async Task<ActionResult> OrderReceipt(int orderId)
        {
            try
            {
                OrderModel or = new OrderModel();
                var result = await or._GetOrderList(orderId);
                return View(result);
            }
            catch (Exception ex)
            {
                elm.Add(
                       ex.Message == null ? "No Message" : ex.Message,
                       ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                       DateTime.Now,
                       HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                       "AdminController", "Order"
                   );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditOrderReceipt(int orderId)
        {
            try
            {
                OrderModel or = new OrderModel();
                var result = await or._GetOrderList(orderId);
                return View(result);
            }
            catch (Exception ex)
            {
                elm.Add(
                       ex.Message == null ? "No Message" : ex.Message,
                       ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                       DateTime.Now,
                       HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                       "AdminController", "Order"
                   );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }
        [HttpPost]
        public ActionResult UpdateReceiptStatus(int orderId, string status)
        {
            try
            {
                OrderModel or = new OrderModel();

                var savedStatus = or.UpdateStatus(orderId, status);

                if (savedStatus == "Error")
                {
                    return Json(new { success = false });
                }

                return Json(new
                {
                    success = true,
                    status = savedStatus
                });
            }
            catch (Exception ex)
            {
                elm.Add(
                       ex.Message == null ? "No Message" : ex.Message,
                       ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                       DateTime.Now,
                       HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                       "AdminController", "UpdateReceiptStatus"
                   );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }

        public async Task<ActionResult> Delete(int FoodId)
        {
            try
            {
                FoodModel b = new FoodModel();

                int result = await b.Delete(FoodId);
                if (result == 1)
                {
                    return Json(new
                    {
                        Status = "Success",
                        Message = "Food deleted successfully.",
                        URL = "/Admin/AllFood"
                    });
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Failed to delete Food. Please try again.",
                        URL = "/Admin/AllFood"
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
                       "AdminController", "Delete"
                   );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }
        [HttpGet]
        public async Task<ActionResult> EditFood(int Foodid)
        {
            try
            {
                FoodModel b = new FoodModel();
                var result = await b.EditFood(Foodid);
                return View(result);
            }
            catch (Exception ex)
            {
                elm.Add(
                        ex.Message == null ? "No Message" : ex.Message,
                        ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                        DateTime.Now,
                        HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                        "AdminController", "EditFood"
                    );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }
        [HttpPost]
        public async Task<ActionResult> EditFood(FoodModel model)
        {
            try
            {
                int result = await model._EditFood(model);

                if (result == 1)
                {
                    return Json(new
                    {
                        Status = "Success",
                        Message = "Food updated successfully.",
                        URL = "/Admin/AllFood"
                    });
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Failed to updated food. Please try again.",
                        URL = "/Book/AllFood"
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
                       "AdminController", "EditFood"
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