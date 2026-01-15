using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace To_Do_List.Models
{
    public class TaskModel
    {
        public int TaskID { get; set; }
        public string TaskTitle { get; set; }
        public string TaskPriority { get; set; }
        public string TaskCategory { get; set; }
        public DateTime? TaskDate { get; set; }
        [Required] 
        public string TaskDescription { get; set; }
        public string Status { get; set; }


        ToDoListdbEntities db = new ToDoListdbEntities();

        public async Task<int> AddTask()
        {
            int result = 0;

            var exist = await db.Tasks.Where(x => x.TaskTitle == TaskTitle.ToLower()).CountAsync();

            if (exist > 0)
            {
                result = 0;
                return result;
            }

            Task t = new Task();

            t.TaskTitle = TaskTitle;
            t.TaskPriority = TaskPriority;
            t.TaskCategory = TaskCategory;
            t.TaskDate = TaskDate;
            t.TaskDescription = TaskDescription;
            t.Status = Status;
            db.Tasks.Add(t);
            await db.SaveChangesAsync();
            result = 1;
            return result;
        }

        public async Task<List<TaskModel>> GetTaskList()
        {
            List<TaskModel> List = await (from t in db.Tasks
                                          select new TaskModel
                                          {
                                              TaskID = t.TaskID,
                                              TaskTitle = t.TaskTitle,
                                              TaskPriority = t.TaskPriority,
                                              TaskCategory = t.TaskCategory,
                                              TaskDescription = t.TaskDescription,
                                              TaskDate = (DateTime)t.TaskDate,
                                              Status = t.Status
                                          }
                    ).ToListAsync();
            return List;
        }

        public async Task<int> TotalTaskCount()
        {
            int count = await db.Tasks.CountAsync();
            return count;
        }

        public async Task<int> TotalTaskPendingCount()
        {
            int count = await db.Tasks
                        .Where(t => t.Status == "Pending")
                        .CountAsync();

            return count;
        }
        public async Task<int> TotalTaskInProgressCount()
        {
            int count = await db.Tasks
                         .Where(t => t.Status == "In Progress")
                         .CountAsync();

            return count;
        }
        public async Task<int> TotalTaskCompleteCount()
        {
            int count = await db.Tasks
                         .Where(t => t.Status == "Complete")
                         .CountAsync();

            return count;
        }

        public async Task<int> DeleteUser(int taskID)
        {
            int result = 0;

            var exist = await db.Tasks.Where(x => x.TaskID == taskID).FirstOrDefaultAsync();

            if (exist != null)
            {
                db.Tasks.Remove(exist);   // ❗ Permanently delete
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

        public async Task<TaskModel> EditTask(int taskID)
        {
            var exist = await db.Tasks.Where(x => x.TaskID == taskID).FirstOrDefaultAsync();

            if (exist == null)
            {
                return null;
            }

            TaskModel t = new TaskModel();

            t.TaskID = exist.TaskID;
            t.TaskTitle = exist.TaskTitle;
            t.TaskPriority = exist.TaskPriority;
            t.TaskCategory = exist.TaskCategory;
            t.TaskDescription = exist.TaskDescription;
            t.TaskDate = (DateTime)exist.TaskDate;
            t.Status = exist.Status;
            return t;
        }

        public async Task<int> EditUser()
        {
            int result = 0;

            var exist = await db.Tasks.Where(x => x.TaskID == TaskID).FirstOrDefaultAsync();

            if (exist == null)
            {
                result = 0;
                return result;
            }

            // Check duplicate username except current user
            var existTitleName = await db.Tasks.Where(x => x.TaskTitle.ToLower() == TaskTitle.ToLower() && x.TaskID != TaskID).CountAsync();

            if (existTitleName > 0)
            {
                result = 2;
                return result;
            }
            else
            {
                exist.TaskID = TaskID;
                exist.TaskTitle = TaskTitle;
                exist.TaskPriority = TaskPriority;
                exist.TaskCategory = TaskCategory;
                exist.TaskDescription = TaskDescription;
                exist.TaskDate = TaskDate;
                exist.Status = Status;
                await db.SaveChangesAsync();
                result = 1;
            }
            return result;
        }

    }
}