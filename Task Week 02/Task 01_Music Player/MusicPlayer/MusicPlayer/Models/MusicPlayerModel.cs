using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MusicPlayer.Models
{
    public class MusicPlayerModel
    {
        public int MusicID { get; set; }
        public string MusicTitle { get; set; }
        public string MusicFilePath { get; set; }
        public string MusicCategory { get; set; }
        public string MusicStatus { get; set; }
        public string MusicImage { get; set; }
        public string SingerName { get; set; }
        public string MusicLike { get; set; }
        public HttpPostedFileBase MusicFile { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }

        MusicPlayerdbEntities db = new MusicPlayerdbEntities();

        public async Task<int> AddMusic(MusicPlayerModel model, HttpPostedFileBase MusicFile, HttpPostedFileBase ImageFile)
        {
            int result = 0;

            var exist = await db.Musics.Where(x => x.MusicTitle ==model.MusicTitle.ToLower()).CountAsync();

            if (exist > 0)
            {
                result = 0;
                return result;
            }

            // Folder paths
            string musicFolder = HttpContext.Current.Server.MapPath("~/assets/Audio/Music/");
            string imageFolder = HttpContext.Current.Server.MapPath("~/assets/Audio/Audio Image/");

            // Create unique filenames
            string musicFileName = MusicTitle + Path.GetExtension(MusicFile.FileName);
            string imageFileName = MusicTitle + Path.GetExtension(ImageFile.FileName);

            // Full save paths
            string musicSavePath = Path.Combine(musicFolder, musicFileName);
            string imageSavePath = Path.Combine(imageFolder, imageFileName);

            // Save files to disk
            MusicFile.SaveAs(musicSavePath);
            ImageFile.SaveAs(imageSavePath);


            Music m = new Music();

            m.MusicTitle =  MusicTitle;
            m.SingerName = SingerName;
            m.MusicFilePath = "/assets/Audio/Music/" + musicFileName;
            m.MusicCategory = MusicCategory;
            m.MusicImage = "/assets/Audio/Audio Image/" + imageFileName;
            db.Musics.Add(m);
            await db.SaveChangesAsync();
            result = 1;
            return result;
        }

        public async Task<List<MusicPlayerModel>> GetMusicList()
        {
            List<MusicPlayerModel> List = await (from m in db.Musics
                                          select new MusicPlayerModel
                                          {
                                              MusicID = m.MusicID,
                                              MusicTitle = m.MusicTitle,
                                              MusicFilePath = m.MusicFilePath,
                                              MusicCategory = m.MusicCategory,
                                              MusicImage = m.MusicImage,
                                              SingerName = m.SingerName,
                                              MusicLike = m.MusicLike
                                          }
                    ).ToListAsync();
            return List;
        }

        public async Task<int> Delete(int musicID)
        {
            int result = 0;

            var exist = await db.Musics.Where(x => x.MusicID == musicID).FirstOrDefaultAsync();

            if (exist != null)
            {

                // Delete music file from folder
                if (!string.IsNullOrEmpty(exist.MusicFilePath))
                {
                    string musicPhysicalPath = HttpContext.Current.Server.MapPath(exist.MusicFilePath);
                    if (System.IO.File.Exists(musicPhysicalPath))
                        System.IO.File.Delete(musicPhysicalPath);
                }

                // Delete image file from folder
                if (!string.IsNullOrEmpty(exist.MusicImage))
                {
                    string imagePhysicalPath = HttpContext.Current.Server.MapPath(exist.MusicImage);
                    if (System.IO.File.Exists(imagePhysicalPath))
                        System.IO.File.Delete(imagePhysicalPath);
                }


                db.Musics.Remove(exist);   // ❗ Permanently delete
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

        public async Task<int> LikeUnlike(string result, int cardId)
        {
            var exist = await db.Musics.Where(x => x.MusicID == cardId).FirstOrDefaultAsync();

            exist.MusicLike = result;
                await db.SaveChangesAsync();
            return  1;
        }
        public async Task<List<MusicPlayerModel>> GetMusicListLikeUnlike()
        {
            List<MusicPlayerModel> List = await (from m in db.Musics
                                                 where m.MusicLike == "Like"
                                                 select new MusicPlayerModel
                                                 {
                                                     MusicID = m.MusicID,
                                                     MusicTitle = m.MusicTitle,
                                                     MusicFilePath = m.MusicFilePath,
                                                     MusicCategory = m.MusicCategory,
                                                     MusicImage = m.MusicImage,
                                                     SingerName = m.SingerName,
                                                     MusicLike = m.MusicLike
                                                 }
                    ).ToListAsync();
            return List;
        }
    }
}