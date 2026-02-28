using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodManagementSystem.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult OverView()
        {
            return View();
        }
        public ActionResult MyOrder()
        {
            return View();
        }
        public ActionResult ShoppingWishlist()
        {
            return View();
        }
        public ActionResult MyAddress()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            return View();
        }
    }
}