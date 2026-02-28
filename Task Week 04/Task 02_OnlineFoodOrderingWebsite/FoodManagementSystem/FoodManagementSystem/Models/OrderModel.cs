using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace FoodManagementSystem.Models
{
    public class OrderModel
    {
        public int OrderID { get; set; }
        public string OrderName { get; set; }
        public int OrderPrice { get; set; }
        public int DeliveryCharges { get; set; }
        public string OrderImage { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmailAddress { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
        public string Locality { get; set; }
        public string COD { get; set; }
        public string Place { get; set; }
        public string Packed { get; set; }
        public string Ontheway { get; set; }
        public string Delivered { get; set; }
        public int UserID { get; set; }
        public int OrderQuantity { get; set; }

        OnlineFoodOrderingSystemEntities db= new OnlineFoodOrderingSystemEntities();

            public string FoodName { get; set; }
            public string ImageUrl { get; set; }
            public decimal DiscountPrice { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }


        public async Task Order(List<OrderModel> orders)
        {
            int _userid = (int)(HttpContext.Current.Session["UserID"] == null ? 0 : HttpContext.Current.Session["UserID"]);
            foreach (var order in orders)
            {
                Order ord = new Order(); // 🔥 inside loop

                ord.OrderName = order.FoodName; // yahan tum galat property use kar rahe the
                ord.OrderPrice = (int?)order.DiscountPrice;
                ord.DeliveryCharges = order.DeliveryCharges;
                ord.OrderImage = order.ImageUrl;
                ord.CustomerName = order.CustomerName;
                ord.UserID = _userid;
                ord.Address = order.Address;
                ord.CustomerEmailAddress = order.CustomerEmailAddress;
                ord.PinCode = order.PinCode;
                ord.Locality = order.Locality;
                ord.COD = order.COD;
                ord.OrderQuantity = order.Quantity;
                ord.OrderDate = DateTime.Now;
                ord.Place = "Pending";

                 db.Orders.Add(ord); // 🔥 ADD TO DB
            }

            await db.SaveChangesAsync(); // 🔥 SAVE

            // 🔥 Now delete cart items of that user
            var userCartItems = db.AddtoCarts.Where(x => x.UserID == _userid).ToList();

            db.AddtoCarts.RemoveRange(userCartItems);

            await db.SaveChangesAsync(); // ✅ Cart cleared



        }

        public async Task<List<OrderModel>> GetOrderList()
        {
            List<OrderModel> List = await (from or in db.Orders
                                           select new OrderModel
                                           {
                                               OrderID = or.OrderID,
                                               OrderName = or.OrderName,
                                               OrderPrice = (int)or.OrderPrice,
                                               DeliveryCharges = (int)or.DeliveryCharges,
                                               OrderImage = or.OrderImage,
                                               CustomerName = or.CustomerName,
                                               CustomerEmailAddress = or.CustomerEmailAddress,
                                               Address = or.Address,
                                               PinCode = or.PinCode,
                                               COD = or.COD,
                                               Locality = or.Locality,
                                               Place = or.Place,
                                               Packed = or.Packed,
                                               Ontheway = or.Ontheway,
                                               Delivered = or.Delivered,
                                               OrderQuantity = or.OrderQuantity,
                                               OrderDate = (DateTime)or.OrderDate,
                                           }
                    ).ToListAsync();
            return List;
        }
        public async Task<List<OrderModel>> _GetOrderList(int orderID)
        {
            List<OrderModel> List = await (from or in db.Orders
                                           where or.OrderID == orderID
                                           select new OrderModel
                                           {
                                               OrderID = or.OrderID,
                                               OrderName = or.OrderName,
                                               OrderPrice = (int)or.OrderPrice,
                                               DeliveryCharges = (int)or.DeliveryCharges,
                                               OrderImage = or.OrderImage,
                                               CustomerName = or.CustomerName,
                                               CustomerEmailAddress = or.CustomerEmailAddress,
                                               Address = or.Address,
                                               PinCode = or.PinCode,
                                               COD = or.COD,
                                               Locality = or.Locality,
                                               Place = or.Place,
                                               Packed = or.Packed,
                                               Ontheway = or.Ontheway,
                                               Delivered = or.Delivered,
                                               OrderQuantity = or.OrderQuantity,
                                               OrderDate = (DateTime)or.OrderDate
                                           }
                    ).ToListAsync();
            return List;
        }

        public string UpdateStatus(int orderId, string status)
        {
            var order = db.Orders.FirstOrDefault(x => x.OrderID == orderId);

            if (order == null)
            {
                return "Error";
            }

            // Status ke hisaab se columns set karo

            if (status == "Pending")
            {
                order.Place = "Pending";
                order.Packed = null;
                order.Ontheway = null;
                order.Delivered = null;
            }
            else if (status == "Packed")
            {
                order.Place = "Pending";
                order.Packed = "Packed";
                order.Ontheway = null;
                order.Delivered = null;
            }
            else if (status == "Ontheway")
            {
                order.Place = "Pending";
                order.Packed = "Packed";
                order.Ontheway = "Ontheway";
                order.Delivered = null;
            }
            else if (status == "Delivered")
            {
                order.Place = "Pending";
                order.Packed = "Packed";
                order.Ontheway = "Ontheway";
                order.Delivered = "Delivered";
            }

            db.SaveChanges();

            // Jo status save hua wohi return karo
            return status;
        }
    }
}