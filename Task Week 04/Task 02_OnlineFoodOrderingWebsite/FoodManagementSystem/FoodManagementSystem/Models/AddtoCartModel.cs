using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace FoodManagementSystem.Models
{
    public class AddtoCartModel
    {
        public int CartID { get; set; }
        public string FoodName { get; set; }
        public string Description { get; set; }
        public int DiscountPercent { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal Price { get; set; }
        public int DeliveryCharges { get; set; }
        public string ImageUrl { get; set; }
        public int FoodQuantity { get; set; }
        public int TotalCharges { get; set; }
        public int UserID { get; set; }


        OnlineFoodOrderingSystemEntities db = new OnlineFoodOrderingSystemEntities();
        public async Task<object> AddtoCart()
        {
            int _UserID = (int)(HttpContext.Current.Session["UserID"] == null ? 0 : HttpContext.Current.Session["UserID"]);
            AddtoCart atc = new AddtoCart
            {
                FoodName = FoodName,
                DiscountPercent = DiscountPercent,
                DiscountPrice = (int?)DiscountPrice,
                Price = (int?)Price,
                DeliveryCharges = DeliveryCharges,
                ImageUrl = ImageUrl,
                FoodQuantity = FoodQuantity,
                UserID = _UserID,
            };

            db.AddtoCarts.Add(atc);
            await db.SaveChangesAsync();

            var userCartItems = await db.AddtoCarts.Where(x => x.UserID == _UserID).Select(x => new
             {
                 x.CartID,
                 x.FoodName,
                 x.DiscountPercent,
                 x.DiscountPrice,
                 x.Price,
                 x.DeliveryCharges,
                 x.ImageUrl,
                 x.FoodQuantity,
                 x.TotalCharges,
                 x.UserID
             }).ToListAsync();

            return userCartItems;
        }

        public async Task<object> GetDataFromAddtoCart(int _UserID)
        {
            var userCartItems = await db.AddtoCarts.Where(x => x.UserID == _UserID).Select(x => new
            {
                x.CartID,
                x.FoodName,
                x.DiscountPercent,
                x.DiscountPrice,
                x.Price,
                x.DeliveryCharges,
                x.ImageUrl,
                x.FoodQuantity,
                x.TotalCharges,
                x.UserID
            }).ToListAsync();

            return userCartItems;
        }

    }
}