using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FoodManagementSystem.Models
{
    public class FoodModel
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DiscountPercent { get; set; }
        public string ImageUrl { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }

        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public decimal DiscountPrice { get;  set; }
        public string DeliveryCharges { get; set; }

        OnlineFoodOrderingSystemEntities db = new OnlineFoodOrderingSystemEntities();

        public async Task<List<FoodModel>> GetFoodList()
        {
            List<FoodModel> List = await (from f in db.Foods
                                              where f.IsActive == true
                                              select new FoodModel
                                              {
                                                  FoodId = f.FoodId,
                                                  FoodName = f.FoodName,
                                                  Description = f.Description,
                                                  Price = (decimal)f.Price,
                                                  DiscountPercent = (int)f.DiscountPercent,
                                                  DiscountPrice = (f.Price > 0 && f.DiscountPercent > 0)
                                                           ? (decimal)f.Price - ((decimal)f.Price * (int)f.DiscountPercent / 100m)
                                                                    : (decimal)f.Price,
                                                                              ImageUrl = f.ImageUrl,
                                                  IsAvailable = (bool)f.IsAvailable,
                                                  UserID = f.UserID,
                                                  IsActive = (bool)f.IsActive,
                                                  DeliveryCharges = f.DeliveryCharges,
                                                  CreatedDate = (DateTime)f.CreatedDate
                                              }
                    ).ToListAsync();
            return List;
        }
        //public async Task<List<FoodModel>> _GetFoodList(string name)
        //{
        //    List<FoodModel> List = await (from f in db.Foods
        //                                  where f.IsActive == true
        //                                  select new FoodModel
        //                                  {
        //                                      FoodId = f.FoodId,
        //                                      FoodName = f.FoodName,
        //                                      Description = f.Description,
        //                                      Price = (decimal)f.Price,
        //                                      DiscountPercent = (int)f.DiscountPercent,
        //                                      DiscountPrice = (f.Price > 0 && f.DiscountPercent > 0)
        //                                               ? (decimal)f.Price - ((decimal)f.Price * (int)f.DiscountPercent / 100m)
        //                                                        : (decimal)f.Price,
        //                                      ImageUrl = f.ImageUrl,
        //                                      IsAvailable = (bool)f.IsAvailable,
        //                                      UserID = f.UserID,
        //                                      IsActive = (bool)f.IsActive,
        //                                      DeliveryCharges = f.DeliveryCharges,
        //                                      CreatedDate = (DateTime)f.CreatedDate
        //                                  }
        //            ).ToListAsync();
        //    return List;
        //}
        public async Task<List<FoodModel>> _GetFoodList(string name)
        {
            return await (from f in db.Foods
                          where f.IsActive == true
                          && (string.IsNullOrEmpty(name)
                              || f.FoodName.Contains(name))
                          select new FoodModel
                          {
                              FoodId = f.FoodId,
                              FoodName = f.FoodName,
                              Description = f.Description,
                              Price = (decimal)f.Price,
                              DiscountPercent = (int)f.DiscountPercent,
                              DiscountPrice = (f.Price > 0 && f.DiscountPercent > 0)
                                  ? (decimal)f.Price - ((decimal)f.Price * (int)f.DiscountPercent / 100m)
                                  : (decimal)f.Price,
                              ImageUrl = f.ImageUrl,
                              IsAvailable = (bool)f.IsAvailable,
                              UserID = f.UserID,
                              IsActive = (bool)f.IsActive,
                              DeliveryCharges = f.DeliveryCharges,
                              CreatedDate = (DateTime)f.CreatedDate
                          }).ToListAsync();
        }

        public async Task<List<FoodModel>> GetFoodDetails(int FoodId)
        {
            List<FoodModel> List = await (from f in db.Foods
                                          where f.IsActive == true && f.FoodId == FoodId
                                          select new FoodModel
                                          {
                                              FoodId = f.FoodId,
                                              FoodName = f.FoodName,
                                              Description = f.Description,
                                              Price = (decimal)f.Price,
                                              DiscountPercent = (int)f.DiscountPercent,
                                              DiscountPrice = (f.Price > 0 && f.DiscountPercent > 0)
                                                       ? (decimal)f.Price - ((decimal)f.Price * (int)f.DiscountPercent / 100m)
                                                                    : (decimal)f.Price,
                                              ImageUrl = f.ImageUrl,
                                              //StockQuantity = (int)f.StockQuantity,
                                              IsAvailable = (bool)f.IsAvailable,
                                              UserID = f.UserID,
                                              IsActive = (bool)f.IsActive,
                                              DeliveryCharges = f.DeliveryCharges
                                          }
                    ).ToListAsync();
            return List;
        }

        public async Task<int> AddFood(FoodModel model)
        {
            int result = 0;

            // Folder paths
            string imageFolder = HttpContext.Current.Server.MapPath("~/assets/FoodManagementSystemWebsite_assets/images/Food/");

            // Create unique filenames
            string imageFileName = FoodName + Path.GetExtension(model.ImageFile.FileName);

            string imageSavePath = Path.Combine(imageFolder, imageFileName);

            // Save files to disk
            model.ImageFile.SaveAs(imageSavePath);


            Food f = new Food();

            f.FoodName = FoodName;
            f.Description = Description;
            f.Price = Price;
            f.DiscountPercent = DiscountPercent;
            f.ImageUrl = "~/assets/FoodManagementSystemWebsite_assets/images/Food/" + imageFileName;
            f.IsAvailable = IsAvailable;
            f.UserID = (int)HttpContext.Current.Session["UserID"];
            f.CreatedDate = DateTime.Now;
            f.IsActive = true;
            f.DeliveryCharges = DeliveryCharges;
            db.Foods.Add(f);
            await db.SaveChangesAsync();
            result = 1;
            return result;
        }

        public async Task<int> Delete(int foodId)
        {
            int result = 0;

            var exist = await db.Foods.Where(x => x.FoodId == foodId).FirstOrDefaultAsync();

            if (exist != null)
            {

                // Delete music file from folder
                if (!string.IsNullOrEmpty(exist.ImageUrl))
                {
                    string CoverImagePhysicalPath = HttpContext.Current.Server.MapPath(exist.ImageUrl);
                    if (System.IO.File.Exists(CoverImagePhysicalPath))
                        System.IO.File.Delete(CoverImagePhysicalPath);
                }

                db.Foods.Remove(exist);   // ❗ Permanently delete
                await db.SaveChangesAsync();
                result = 1;
                return result;
            }
            else
            {
                result = 0;
                return result;
            }
        }

        public async Task<FoodModel> EditFood(int foodid)
        {
            var exist = await db.Foods.Where(x => x.FoodId == foodid).FirstOrDefaultAsync();

            if (exist == null)
            {
                return null;
            }

            FoodModel b = new FoodModel();

            b.FoodName = exist.FoodName;
            b.Description = exist.Description;
            b.Price = (decimal)exist.Price;
            b.DiscountPercent = (int)exist.DiscountPercent;
            b.ImageUrl = exist.ImageUrl;
            b.IsAvailable = (bool)exist.IsAvailable;
            b.UserID = exist.UserID;
            b.DeliveryCharges = exist.DeliveryCharges;
            return b;
        }

        //public async Task<int> 
        //{
        //    int result = 0;

        //    // Folder paths
        //    string imageFolder = HttpContext.Current.Server.MapPath("~/assets/FoodManagementSystemWebsite_assets/images/Food/");

        //    // Create unique filenames
        //    string imageFileName = FoodName + Path.GetExtension(model.ImageFile.FileName);

        //    string imageSavePath = Path.Combine(imageFolder, imageFileName);

        //    // Save files to disk
        //    model.ImageFile.SaveAs(imageSavePath);


        //    Food f = new Food();

        //    f.FoodName = FoodName;
        //    f.Description = Description;
        //    f.Price = Price;
        //    f.DiscountPercent = DiscountPercent;
        //    f.ImageUrl = "" + imageFileName;
        //    f.IsAvailable = IsAvailable;
        //    f.UserID = (int)HttpContext.Current.Session["UserID"];
        //    f.CreatedDate = DateTime.Now;
        //    f.IsActive = true;
        //    f.DeliveryCharges = DeliveryCharges;
        //    db.Foods.Add(f);
        //    await db.SaveChangesAsync();
        //    result = 1;
        //    return result;
        //}

        public async Task<int> _EditFood(FoodModel model)
        {
            int result = 0;

            var exist = await db.Foods.Where(x => x.FoodId == model.FoodId).FirstOrDefaultAsync();

            if (exist == null)
            {
                result = 0;
                return result;
            }
            else
            {
                if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                {
                    // Folder paths
                    string imageFolder = HttpContext.Current.Server.MapPath("~/assets/FoodManagementSystemWebsite_assets/images/Food/");

                    // Create unique filenames
                    string imageFileName = model.FoodName + Path.GetExtension(model.ImageFile.FileName);

                    string imageSavePath = Path.Combine(imageFolder, imageFileName);

                    // Save files to disk
                    model.ImageFile.SaveAs(imageSavePath);

                    // ✅ update image path only when new image uploaded
                    exist.ImageUrl = "~/assets/FoodManagementSystemWebsite_assets/images/Food/" + imageFileName;
                }

                exist.FoodName = model.FoodName;
                exist.Description = model.Description;
                exist.Price = model.Price;
                exist.DiscountPercent = model.DiscountPercent;
                exist.IsAvailable = model.IsAvailable;
                exist.UserID = (int)HttpContext.Current.Session["UserID"];
                exist.CreatedDate = DateTime.Now;
                exist.IsActive = true;
                exist.DeliveryCharges = model.DeliveryCharges;
                await db.SaveChangesAsync();
                result = 1;
            }
            return result;
        }

        public async Task<int> TodayOrder()
        {
            var today = DateTime.Today;

            return await db.Orders
                .Where(o => DbFunctions.TruncateTime(o.OrderDate) == today)
                .CountAsync();
        }
        public async Task<int> PendingOrder()
        {
            return await db.Orders
                .Where(o => o.Place == "Pending"
                         && o.Packed == null
                         && o.Ontheway == null
                         && o.Delivered == null)
                .CountAsync();
        }
        public async Task<int> PackedOrder()
        {
            return await db.Orders
                .Where(o => o.Packed == "Packed"
                         && o.Ontheway == null
                         && o.Delivered == null)
                .CountAsync();
        }
        public async Task<int> OnthewayOrder()
        {
            return await db.Orders
                .Where(o => o.Ontheway == "Ontheway"
                         && o.Delivered == null)
                .CountAsync();
        }
        public async Task<int> DeliveredOrder()
        {
            return await db.Orders
                .Where(o => o.Delivered == "Delivered")
                .CountAsync();
        }
    }
}