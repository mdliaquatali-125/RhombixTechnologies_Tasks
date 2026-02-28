using FoodManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FoodManagementSystem.Controllers
{
    public class FoodController : Controller
    {
        ErrorLogMasterModel elm = new ErrorLogMasterModel();

        public async Task<ActionResult> NewFood()
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
                       "FoodController", "NewFood"
                   );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }
        public async Task<ActionResult> NewFoodsss(string name)
        {
            try
            {
                FoodModel f = new FoodModel();


                var result = await f._GetFoodList(name);
                    return PartialView("_FoodList", result);
            }
            catch (Exception ex)
            {
                elm.Add(
                       ex.Message == null ? "No Message" : ex.Message,
                       ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                       DateTime.Now,
                       HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                       "FoodController", "NewFood"
                   );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }
        [HttpGet]
        public async Task<ActionResult> FoodDetails(int FoodId)
        {
            try
            {
                FoodModel f = new FoodModel();
                var result = await f.GetFoodDetails(FoodId);
                return View(result);
            }
            catch (Exception ex)
            {
                elm.Add(
                       ex.Message == null ? "No Message" : ex.Message,
                       ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                       DateTime.Now,
                       HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                       "FoodController", "FoodDetails"
                   );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }

        public async Task<ActionResult> AddtoCart(AddtoCartModel model)
        {
            try
            {
                if (Session["UserId"] != null)   // ✅ direct session check karo
                {
                    var userCartItems = await model.AddtoCart();

                    return Json(new
                    {
                        Status = "Success",
                        Data = userCartItems
                    });
                }
                else
                {
                    return Json(new
                    {
                        Status = "LoginRequired",
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
                     "FoodController", "AddtoCart"
                 );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }

        public ActionResult checkout()
        {

            try
            {
                return View();
            }
            catch (Exception ex)
            {
                elm.Add(
                     ex.Message == null ? "No Message" : ex.Message,
                     ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                     DateTime.Now,
                     HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                     "FoodController", "checkout"
                 );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }

        public async Task<ActionResult> GetCheckoutCart()
        {
            try
            {
                int _UserID = (int)(HttpContext.Session["UserID"] ?? 0);

                AddtoCartModel model = new AddtoCartModel();
                var userCartItems = await model.GetDataFromAddtoCart(_UserID);

                return Json(new
                {
                    Status = "Success",
                    Data = userCartItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                elm.Add(
                    ex.Message ?? "No Message",
                    ex.InnerException?.Message ?? "No Inner Exception",
                    DateTime.Now,
                    HttpContext.Session["UserName"]?.ToString() ?? "UnknownUser",
                    "FoodController",
                    "GetCheckoutCart"
                );

                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> Order(List<OrderModel> orders)
        {
            //try
            //{
            //    OrderModel model = new OrderModel();
            //    await model.Order(orders);

            //    return Json(new
            //    {
            //        Status = "Success",
            //    }, JsonRequestBehavior.AllowGet);
            //}



            try
            {
                if (orders == null || !orders.Any())
                {
                    return Json(new
                    {
                        Status = "NoItemintheCart",
                    }, JsonRequestBehavior.AllowGet);
                }

                OrderModel model = new OrderModel();
                await model.Order(orders);

                return Json(new
                {
                    Status = "Success",
                }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                elm.Add(
                    ex.Message ?? "No Message",
                    ex.InnerException?.Message ?? "No Inner Exception",
                    DateTime.Now,
                    HttpContext.Session["UserName"]?.ToString() ?? "UnknownUser",
                    "FoodController",
                    "GetCheckoutCart"
                );

                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}