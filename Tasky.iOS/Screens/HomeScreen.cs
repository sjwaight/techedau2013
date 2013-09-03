using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using Tasky.AL;
using Tasky.BL;
using Tasky.BL.Managers;

namespace Tasky.Screens.iPhone
{
	public class HomeScreen : DialogViewController 
    {
        BindingContext context;
        TaskDialog taskDialog;
        UserTask currentTask;
        DialogViewController detailsScreen;

		List<UserTask> tasks;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeScreen"/> class.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
		public HomeScreen () : base (UITableViewStyle.Plain, null)
		{
			Initialize();
		}

        /// <summary>
        /// Initializes this instance.
        /// </summary>
		protected void Initialize()
		{
			NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Add), false);
			NavigationItem.RightBarButtonItem.Clicked += (sender, e) => { ShowTaskDetails(new UserTask()); };
		}

        /// <summary>
        /// Shows the task details.
        /// </summary>
        /// <param name="task">The task.</param>
        protected void ShowTaskDetails(UserTask task)
        {
            currentTask = task;
            taskDialog = new TaskDialog(task);

            var title = MonoTouch.Foundation.NSBundle.MainBundle.LocalizedString("Task Details", "Task Details");
            context = new BindingContext(this, taskDialog, title);
            detailsScreen = new DialogViewController(context.Root, true);

            ActivateController(detailsScreen);
        }

        /// <summary>
        /// Saves the task.
        /// </summary>
		public void SaveTask()
		{
			context.Fetch(); // re-populates with updated values
			currentTask.Name = taskDialog.Name;
			currentTask.Notes = taskDialog.Notes;
			currentTask.Done = taskDialog.Done;
            currentTask.Assignee = taskDialog.Assignees[taskDialog.CurrentAssignee];

            /////////////////////
            // Save task to Azure Mobile Services
            // This is an async method so the mobile app will simply continue on.
            /////////////////////

            TaskManager.Instance.Insert(currentTask);

            NavigationController.PopViewControllerAnimated (true);
		    //	context.Dispose (); // documentation suggests this is required, but appears to cause a crash sometimes
		}

        /// <summary>
        /// Cancels the task.
        /// </summary>
		public void CancelTask()
		{
			NavigationController.PopViewControllerAnimated(true);
		}

        /// <summary>
        /// Invoked when the UITableView is going to be shown.
        /// </summary>
        /// <param name="animated"></param>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
			// reload/refresh
			PopulateTable();			
		}

        /// <summary>
        /// Populates the table.
        /// </summary>
		protected async void PopulateTable()
		{
            /////////////////////
            // Load task from Azure Mobile Services
            /////////////////////

            tasks = await TaskManager.Instance.GetTasksForUser("Tim");

			var newTaskDefaultName = MonoTouch.Foundation.NSBundle.MainBundle.LocalizedString ("<new task>", "<new task>");
			// make into a list of MT.D elements to display
			List<Element> le = new List<Element>();
            foreach (var t in tasks)
            {
                le.Add(new StringElement((t.Name == "" ? newTaskDefaultName : t.Name), t.Notes));
            }

			// add to section
			var s = new Section();
			s.AddAll (le);
			// add as root
			Root = new RootElement("Tasky Pro") { s }; 
		}

        /// <summary>
        /// Method invoked when an element in the DialogViewController has been selected.
        /// </summary>
        /// <param name="indexPath"></param>
        public override void Selected (MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var task = tasks[indexPath.Row];
			ShowTaskDetails(task);
		}

        /// <summary>
        /// Method invoked by the DialogViewController to create its UITableViewSource.
        /// </summary>
        /// <param name="unevenRows">Whether the Source object should support rows with different sizes.   If true, the returned object should override the UITableViewSource.GetHeightForRow.</param>
        /// <returns></returns>
        /// <remarks>
        ///   <para>
        /// This is a virtual method that subclasses of
        /// DialogViewController can override to create their own
        /// versions of the <see cref="T:MonoTouch.Dialog.DialogViewController.Source" /> object (which is merely a subclass of <see cref="T:MonoTouch.UIKit.UITableViewSource" />.
        ///   </para>
        ///   <para>
        /// If the value of unevenRows is true, the resulting source should override the GetHeightForRow method as the DialogViewController has determined that there will be rows with different sizes
        ///   </para>
        /// </remarks>
        public override Source CreateSizingSource (bool unevenRows)
		{
			return new EditingSource (this);
		}

        /// <summary>
        /// Deletes the task row.
        /// </summary>
        /// <param name="rowId">The row id.</param>
		public void DeleteTaskRow(int rowId)
		{
			// Do nothing
		}
	}
}