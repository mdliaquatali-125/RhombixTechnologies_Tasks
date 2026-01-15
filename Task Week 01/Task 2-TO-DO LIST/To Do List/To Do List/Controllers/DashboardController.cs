using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using To_Do_List.Models;
using To_Do_List.Security;
using To_Do_List.Utilities;

namespace To_Do_List.Controllers
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
                TaskModel t = new TaskModel();
                var result = await t.GetTaskList();

                ViewBag.TotalTaskCount = await t.TotalTaskCount();
                ViewBag.TotalTaskPending = await t.TotalTaskPendingCount();
                ViewBag.TotalTaskInProgress = await t.TotalTaskInProgressCount();
                ViewBag.TotalTaskComplete = await t.TotalTaskCompleteCount();

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

        [HttpGet]
        public ActionResult AddTask()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddTask(TaskModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int result = await model.AddTask();

                    if (result == 1)
                    {
                        return Json(new
                        {
                            Status = "Success",
                            Message = "Task added successfully.",
                            URL = "/dashboard/Index"
                        });
                    }
                    else if (result == 0)
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Task already exists.",
                            URL = "/dashboard/AddTask"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Failed to add task. Please try again.",
                            URL = "/dashboard/AddTask"
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Validation failed.",
                        URL = "/dashboard/AddTask"
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
                       "DashboardController", "AddTask"
                   );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }

        public async Task<ActionResult> Delete(int TaskID)
        {
            try
            {
                TaskModel t = new TaskModel();

                int result = await t.DeleteUser(TaskID);
                if (result == 1)
                {
                    return Json(new
                    {
                        Status = "Success",
                        Message = "Task deleted successfully.",
                        URL = "/Dashboard/Index"
                    });
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Failed to delete Task. Please try again.",
                        URL = "/Dashboard/Index"
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
                       "DashboardController", "Delete"
                   );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }

        }

        [HttpGet]
        public async Task<ActionResult> Edit(int TaskID)
        {
            try
            {
                TaskModel t = new TaskModel();
                var result = await t.EditTask(TaskID);
                return View(result);
            }
            catch (Exception ex)
            {
                elm.Add(
                        ex.Message == null ? "No Message" : ex.Message,
                        ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                        DateTime.Now,
                        HttpContext.Session["UserName"] == null ? "UnknownUser" : HttpContext.Session["UserName"].ToString(),
                        "DashboardController", "Edit"
                    );
                return RedirectToAction("Index", "ErrorLogMaster");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TaskModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    int result = await model.EditUser();

                    if (result == 1)
                    {
                        return Json(new
                        {
                            Status = "Success",
                            Message = "Task updated successfully.",
                            URL = "/Dashboard/Index"
                        });
                    }
                    else if (result == 2)
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Task already exists.",
                            URL = "/Dashboard/Edit?TaskID=" + model.TaskID
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Failed to update Task. Please try again.",
                            URL = "/Dashboard/Edit?TaskID=" + model.TaskID
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Validation failed.",
                        URL = "/Dashboard/Edit?TaskID=" + model.TaskID
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
                       "DashboardController", "Edit"
                   );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }
    }
}