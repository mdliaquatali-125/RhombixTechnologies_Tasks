using MusicPlayer.Models;
using MusicPlayer.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MusicPlayer.Controllers
{
    [DashboardSession]
    public class MusicPlayerController : Controller
    {
        //ErrorLogMasterModel
        ErrorLogMasterModel elm = new ErrorLogMasterModel();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(MusicPlayerModel model, HttpPostedFileBase MusicFile, HttpPostedFileBase ImageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int result = await model.AddMusic(model, MusicFile,ImageFile);

                    if (result == 1)
                    {
                        return Json(new
                        {
                            Status = "Success",
                            Message = "Music added successfully.",
                            URL = "/dashboard/Index"
                        });
                    }
                    else if (result == 0)
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Music already exists.",
                            URL = "/MusicPlayer/Index"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Failed to add music. Please try again.",
                            URL = "/MusicPlayer/Index"
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Validation failed.",
                        URL = "/MusicPlayer/Index"
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
                       "MusicPlayerController", "Index"
                   );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }

        public async Task<ActionResult> PlayList()
        {

            try
            {
                MusicPlayerModel m = new MusicPlayerModel();
                var result = await m.GetMusicList();
                return View(result);
            }
            catch (Exception ex)
            {
                elm.Add(
                       ex.Message == null ? "No Message" : ex.Message,
                       ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                       DateTime.Now,
                       HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                       "MusicPlayerController", "PlayList"
                   );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }

        public async Task<ActionResult> Delete(int MusicID)
        {
            try
            {
                MusicPlayerModel m = new MusicPlayerModel();

                int result = await m.Delete(MusicID);
                if (result == 1)
                {
                    return Json(new
                    {
                        Status = "Success",
                        Message = "Music deleted successfully.",
                        URL = "/MusicPlayer/PlayList"
                    });
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Failed to delete Task. Please try again.",
                        URL = "/MusicPlayer/PlayList"
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
                       "MusicPlayerController", "Delete"
                   );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }
        public ActionResult FilterMusic(string category)
        {
            MusicPlayerdbEntities db = new MusicPlayerdbEntities();

            // Fetch filtered data
            var musics = db.Musics.AsQueryable();

            switch (category)
            {
                case "_pop":
                    musics = musics.Where(m => m.MusicCategory == "Pop");
                    break;
                case "_electronic":
                    musics = musics.Where(m => m.MusicCategory == "Electronic");
                    break;
                case "_classical":
                    musics = musics.Where(m => m.MusicCategory == "Classical");
                    break;
                default:
                    break; // All
            }

            // Map to MusicPlayerModel
            IEnumerable<MusicPlayerModel> model = musics
                .Select(m => new MusicPlayerModel
                {
                    MusicTitle = m.MusicTitle,
                    MusicFilePath = m.MusicFilePath,
                    MusicImage = m.MusicImage,
                    SingerName = m.SingerName,
                    MusicCategory = m.MusicCategory
                })
                .ToList();

            return View("Index", model); // or your view name
        }

        public async Task<ActionResult> LikeUnlike(string Result, int cardId)
        {
            try
            {
                MusicPlayerModel m = new MusicPlayerModel();

                int _Result = await m.LikeUnlike(Result, cardId);
                if (_Result == 1)
                {
                    return Json(new
                    {
                        Status = "Success",
                        Message = "Like",
                        URL = "/dashboard/index"
                    });
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "UnLike",
                        URL = "/dashboard/index"
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
                       "MusicPlayerController", "LikeUnlike"
                   );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }

        public async Task<ActionResult> PlayListLikeUnlike()
        {

            try
            {
                MusicPlayerModel m = new MusicPlayerModel();
                var result = await m.GetMusicListLikeUnlike();
                return View(result);
            }
            catch (Exception ex)
            {
                elm.Add(
                       ex.Message == null ? "No Message" : ex.Message,
                       ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                       DateTime.Now,
                       HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                       "MusicPlayerController", "PlayListLikeUnlike"
                   );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }
    }
}