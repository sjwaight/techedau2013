using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android.Graphics;
using Android.Views;

using Tasky.BL;
using Tasky.BL.Managers;

namespace Tasky.Droid.Screens 
{
	
	[Activity (Label = "Task Details")]			
	public class TaskDetailsScreen : Activity 
    {
        protected UserTask task = new UserTask();
		protected Button cancelDeleteButton = null;
		protected EditText notesTextEdit = null;
		protected EditText nameTextEdit = null;
		protected Button saveButton = null;
        protected Spinner assigneeSpinner = null;

		CheckBox doneCheckbox;

        /// <summary>
        /// Called when [create].
        /// </summary>
        /// <param name="bundle">The bundle.</param>
		protected async override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);
			
			View titleView = Window.FindViewById(Android.Resource.Id.Title);
			if (titleView != null) {
			  IViewParent parent = titleView.Parent;
			  if (parent != null && (parent is View)) {
			    View parentView = (View)parent;
			    parentView.SetBackgroundColor(Color.Rgb(0x26, 0x75 ,0xFF)); //38, 117 ,255
			  }
			}

			int taskID = Intent.GetIntExtra("TaskID", 0);

            if(taskID > 0) 
            {
                /////////////////////
                // Load task from Azure Mobile Services
                /////////////////////
                task = await TaskManager.Instance.GetTaskById(taskID);
            }
			
			// set our layout to be the home screen
			SetContentView(Resource.Layout.TaskDetails);
			nameTextEdit = FindViewById<EditText>(Resource.Id.txtName);
			notesTextEdit = FindViewById<EditText>(Resource.Id.txtNotes);
			saveButton = FindViewById<Button>(Resource.Id.btnSave);
			doneCheckbox = FindViewById<CheckBox>(Resource.Id.chkDone);
			
    		cancelDeleteButton = FindViewById<Button>(Resource.Id.btnCancelDelete);

            assigneeSpinner = FindViewById<Spinner>(Resource.Id.TaskAssignee);

            if (assigneeSpinner != null)
            {
                var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.assignee_array, Android.Resource.Layout.SimpleSpinnerItem);

                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                assigneeSpinner.Adapter = adapter;
            }		
			
			// set the cancel delete based on whether or not it's an existing task
			if(cancelDeleteButton != null)
			{ cancelDeleteButton.Text = (task.ID == 0 ? "Cancel" : "Delete"); }
			
			// name
			if(nameTextEdit != null) { nameTextEdit.Text = task.Name; }
			
			// notes
			if(notesTextEdit != null) { notesTextEdit.Text = task.Notes; }
			
			if(doneCheckbox != null) { doneCheckbox.Checked = task.Done; }

			// button clicks 
			cancelDeleteButton.Click += (sender, e) => { CancelDelete(); };
			saveButton.Click += (sender, e) => { Save(); };
		}

        /// <summary>
        /// Saves this instance.
        /// </summary>
		protected void Save()
		{
			task.Name = nameTextEdit.Text;
			task.Notes = notesTextEdit.Text;
			task.Done = doneCheckbox.Checked;
            task.Assignee = assigneeSpinner.GetItemAtPosition(assigneeSpinner.SelectedItemPosition).ToString();

            /////////////////////
            // Save task to Azure Mobile Services
            // This is an async method so the mobile app will simply continue on.
            /////////////////////
          
            TaskManager.Instance.Insert(task);

			Finish();
		}

        /// <summary>
        /// Cancels the delete.
        /// </summary>
		protected void CancelDelete()
		{
			Finish();
		}
	}
}