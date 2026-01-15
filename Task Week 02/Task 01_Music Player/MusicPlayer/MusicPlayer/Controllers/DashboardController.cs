using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MusicPlayer.Models;
using MusicPlayer.Security;
using MusicPlayer.Utilities;

namespace MusicPlayer.Controllers
{
    [DashboardSession]
    public class DashboardController : Controller
    {
        //ErrorLogMasterModel
        ErrorLogMasterModel elm = new ErrorLogMasterModel();

        // GET: Dashboard
        public async Task<ActionResult> Index()
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
                       "DashboardController", "Index"
                   );
                return RedirectToAction("Index", "ErrorLogMaster");
            }

        }
    }
}