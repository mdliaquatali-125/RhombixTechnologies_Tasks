using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BookLibraryManagmentSystem.Models
{
    public class BookModel
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string ISBN { get; set; }
        public int Quantity { get; set; }
        public string CoverImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsAvailable { get; set; }

        public HttpPostedFileBase _CoverImage { get; set; }

        LibraryBookManagmentSystemdbEntities db = new LibraryBookManagmentSystemdbEntities();
       
        public async Task<int> AddBook(BookModel model)
        {
            int result = 0;

            var exist = await db.BookDetails.Where(x => x.Title == Title.ToLower()).CountAsync();

            if (exist > 0)
            {
                result = 0;
                return result;
            }

            // Folder paths
            string imageFolder = HttpContext.Current.Server.MapPath("~/assets/images/CoverImage/");

            // Create unique filenames
            string imageFileName = Title + Path.GetExtension(model._CoverImage.FileName);

            string imageSavePath = Path.Combine(imageFolder, imageFileName);

            // Save files to disk
           model._CoverImage.SaveAs(imageSavePath);


            BookDetail b = new BookDetail();

            b.Title = Title;
            b.Author = Author;
            b.Category = Category;
            b.ISBN = ISBN;
            b.Quantity = Quantity;
            b.IsAvailable = IsAvailable;
            b.CoverImage = "~/assets/images/CoverImage/" + imageFileName;
            b.CreatedAt = DateTime.Now;
            db.BookDetails.Add(b);
            await db.SaveChangesAsync();
            result = 1;
            return result;
        }

        public async Task<List<BookModel>> GetBookList()
        {
            List<BookModel> List = await (from b in db.BookDetails
                                                 select new BookModel
                                                 {
                                                     BookID = b.BookID,
                                                     Title = b.Title,
                                                     Author = b.Author,
                                                     Category = b.Category,
                                                     ISBN = b.ISBN,
                                                     Quantity = (int)b.Quantity,
                                                     IsAvailable = (bool)b.IsAvailable,
                                                     CoverImage = b.CoverImage,
                                                     CreatedAt = (DateTime)b.CreatedAt
                                                 }
                    ).ToListAsync();
            return List;
        }

        public async Task<int> Delete(int bookID)
        {
            int result = 0;

            var exist = await db.BookDetails.Where(x => x.BookID == bookID).FirstOrDefaultAsync();

            if (exist != null)
            {

                // Delete music file from folder
                if (!string.IsNullOrEmpty(exist.CoverImage))
                {
                    string CoverImagePhysicalPath = HttpContext.Current.Server.MapPath(exist.CoverImage);
                    if (System.IO.File.Exists(CoverImagePhysicalPath))
                        System.IO.File.Delete(CoverImagePhysicalPath);
                }

                db.BookDetails.Remove(exist);   // ❗ Permanently delete
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

        public async Task<BookModel> EditBook(int bookID)
        {
            var exist = await db.BookDetails.Where(x => x.BookID == bookID).FirstOrDefaultAsync();

            if (exist == null)
            {
                return null;
            }

            BookModel b = new BookModel();

            b.Title = exist.Title;
            b.Author = exist.Author;
            b.Category = exist.Category;
            b.ISBN = exist.ISBN;
            b.Quantity = (int)exist.Quantity;
            b.IsAvailable = (bool)exist.IsAvailable;
            b.CoverImage = exist.CoverImage;
            b.CreatedAt = (DateTime)exist.CreatedAt;
            return b;
        }

        public async Task<int> _EditBook(BookModel model)
        {
            int result = 0;

            var exist = await db.BookDetails.Where(x => x.BookID == model.BookID).FirstOrDefaultAsync();

            if (exist == null)
            {
                result = 0;
                return result;
            }

            // Check duplicate username except current user
            var existTitleName = await db.BookDetails.Where(x => x.Title.ToLower() == model.Title.ToLower() && x.BookID != model.BookID).CountAsync();

            if (existTitleName > 0)
            {
                result = 2;
                return result;
            }
            else
            {
                if (model._CoverImage != null && model._CoverImage.ContentLength > 0)
                {
                    // Folder paths
                    string imageFolder = HttpContext.Current.Server.MapPath("~/assets/images/CoverImage/");

                    // Create unique filenames
                    string imageFileName = model.Title + Path.GetExtension(model._CoverImage.FileName);

                    string imageSavePath = Path.Combine(imageFolder, imageFileName);

                    // Save files to disk
                    model._CoverImage.SaveAs(imageSavePath);

                    // ✅ update image path only when new image uploaded
                    exist.CoverImage = "/assets/images/CoverImage/" + imageFileName;
                }

                exist.Title = model.Title;
                exist.Author = model.Author;
                exist.Category = model.Category;
                exist.ISBN = model.ISBN;
                exist.Quantity = model.Quantity;
                exist.IsAvailable = model.IsAvailable;
                exist.CreatedAt = DateTime.Now;
                await db.SaveChangesAsync();
                result = 1;
            }
            return result;
        }

        public async Task<List<BookModel>> GetBookTitleList()
        {
            List<BookModel> List = await (from b in db.BookDetails
                                          select new BookModel
                                          {
                                              BookID = b.BookID,
                                              Title = b.Title
                                          }
                    ).ToListAsync();
            return List;
        }

        public async Task<int> GetBookCount()
        {
            return await db.BookDetails.CountAsync();
        }

        public async Task<int> GetAvailableBookCount()
        {
            return await db.BookDetails
                           .Where(b => b.IsAvailable == true)
                           .CountAsync();
        }

        public async Task<int> GetIssueBookCount()
        {
            return await db.BorrowHistories.CountAsync();
        }

        public async Task<int> GetReturnBookCount()
        {
            return await db.BorrowHistories.Where(b => b.Status == "Returned").CountAsync();
        }
    }
}