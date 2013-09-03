using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.WindowsAzure.MobileServices;
using Tasky.BL;
using System.Threading.Tasks;

namespace Tasky.BL.Managers
{
    public class TaskManager
    {
        private static readonly object mutex = new object();

        private readonly MobileServiceClient mobileService;
        
        private static TaskManager instance;

        private List<UserTask> items;

        private readonly IMobileServiceTable<UserTask> table;

        private TaskManager() 
        {
            mobileService = new MobileServiceClient("https://YOUR-SERVICE.azure-mobile.net/", "YOUR-KEY");
            table = mobileService.GetTable<UserTask>();
            items = new List<UserTask>();
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static TaskManager Instance
        {
          get 
          {
             if (instance == null)
             {
                 lock (mutex)
                 {
                     instance = new TaskManager();
                 }
             }
             return instance;
          }
       }

        /// <summary>
        /// Gets the tasks.
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserTask>> GetTasks()
        {
             this.items = await this.table.Where(ti => !ti.Done).ToListAsync();

             return this.items;
        }

        /// <summary>
        /// Gets the tasks for user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public async Task<List<UserTask>> GetTasksForUser(string userName)
        {
            try
            {
                this.items = await this.table.Where(ti => !ti.Done && ti.Assignee == userName).ToListAsync();               
            }
            catch (AggregateException ae)
            {

            }

            return this.items;
        }


        /// <summary>
        /// Gets the task by id.
        /// </summary>
        /// <param name="taskIdentifier">The task identifier.</param>
        /// <returns></returns>
        public async Task<UserTask> GetTaskById(int taskIdentifier)
        {
            this.items = await this.table.Where(ti => ti.ID == taskIdentifier).ToListAsync();

            return this.items.First();
        }

        /// <summary>
        /// Inserts the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public async Task<bool> Insert(UserTask item)
        {
            var success = true;

            var serviceCall = this.table.InsertAsync(item);

            await serviceCall;
           
            if(serviceCall.IsFaulted)
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Update(UserTask item)
        {
            table.UpdateAsync(item).ContinueWith(t =>
            {
                var updatedItem = this.items.Where(i => i.ID == item.ID).First();
                updatedItem = item;
            }, TaskScheduler.Current);
        }
    }
}