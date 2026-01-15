using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BookLibraryManagmentSystem.Models
{
    public class BorrowModel
    {

        public int BorrowID { get; set; }
        public int BookID { get; set; }
        public string BorrowerName { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }


        //For Book
        public string BookTitle { get; set; }

        LibraryBookManagmentSystemdbEntities db = new LibraryBookManagmentSystemdbEntities();

        public async Task<int> AddBorrow(BorrowModel model)
        {
            int result = 0;

            var exist = await db.BorrowHistories.Where(x => x.BorrowerName == BorrowerName.ToLower()).CountAsync();

            if (exist > 0)
            {
                result = 0;
                return result;
            }

            BorrowHistory b = new BorrowHistory();

            b.BookID = BookID;
            b.BorrowerName = BorrowerName;
            b.BorrowDate = BorrowDate;
            b.ReturnDate = ReturnDate;
            b.Status = Status;
            b.CreatedAt = DateTime.Now;
            db.BorrowHistories.Add(b);
            await db.SaveChangesAsync();
            result = 1;
            return result;
        }

        public async Task<List<BorrowModel>> GetBorrowList()
        {
            List<BorrowModel> List = await (from b in db.BookDetails
                                            join bh in db.BorrowHistories
                                                on b.BookID equals bh.BookID
                                            select new BorrowModel
                                            {
                                                BookID = b.BookID,
                                                BorrowID = bh.BorrowID,
                                                BookTitle = b.Title,
                                                BorrowerName = bh.BorrowerName,
                                                BorrowDate = bh.BorrowDate,
                                                ReturnDate = (DateTime)bh.ReturnDate,
                                                Status = bh.Status,
                                            }
                    ).ToListAsync();
            return List;
        }

        public async Task<int> Delete(int borrowID)
        {
            int result = 0;

            var exist = await db.BorrowHistories.Where(x => x.BorrowID == borrowID).FirstOrDefaultAsync();

            if (exist != null)
            {
                db.BorrowHistories.Remove(exist);   // ❗ Permanently delete
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

        public async Task<BorrowModel> BorrowEdit(int borrowID)
        {
            var exist = await db.BorrowHistories.Where(x => x.BorrowID == borrowID).FirstOrDefaultAsync();


            if (exist == null)
            {
                return null;
            }
            else
            {
                var book = await db.BookDetails.Where(c => c.BookID == exist.BookID).FirstOrDefaultAsync();


                BorrowModel b = new BorrowModel();
                b.BookID = exist.BookID;
                b.BookTitle = book.Title;
                b.BorrowerName = exist.BorrowerName;
                b.BorrowDate = exist.BorrowDate;
                b.ReturnDate = (DateTime)exist.ReturnDate;
                b.Status = exist.Status;
                b.CreatedAt = (DateTime)exist.CreatedAt;
                return b;
            }
        }

        public async Task<int> _BorrowEdit(BorrowModel model)
        {
            int result = 0;

            var exist = await db.BorrowHistories.Where(x => x.BorrowID == model.BorrowID).FirstOrDefaultAsync();

            if (exist == null)
            {
                result = 0;
                return result;
            }

            // Check duplicate username except current user
            var existTitleName = await db.BorrowHistories.Where(x => x.BorrowerName.ToLower() == model.BorrowerName.ToLower() && x.BorrowID != model.BorrowID).CountAsync();

            if (existTitleName > 0)
            {
                result = 2;
                return result;
            }
            else
            {
                exist.BookID = model.BookID;
                exist.BorrowerName = model.BorrowerName;
                exist.BorrowDate = model.BorrowDate;
                exist.ReturnDate = model.ReturnDate;
                exist.Status = model.Status;
                exist.CreatedAt = DateTime.Now;
                await db.SaveChangesAsync();
                result = 1;
            }
            return result;
        }
    }
}