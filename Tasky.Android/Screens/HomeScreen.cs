using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Tasky.BL;
using Tasky.Droid.Adapters;

namespace Tasky.Droid.Screens 
{
	[Activity (Label = "Tasky Pro with Push", MainLauncher = true, Icon="@drawable/launcher")]			
	public class HomeScreen : Activity 
    {
        public string CloudMessagingRegistrationId { get; private set; }

        protected IList<UserTask> tasks;

		protected TaskListAdapter adapter;
		protected Button addTaskButton = null;
		protected ListView taskListView = null;
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate(bundle);

            ///////////////////
            // Push the device registration to Azure Mobile Services.
            ///////////////////

            RegisterForNotifications();

			View titleView = Window.FindViewById(Android.Resource.Id.Title);
			if (titleView != null) {
			  IViewParent parent = titleView.Parent;
			  if (parent != null && (parent is View)) {
			    View parentView = (View)parent;
			    parentView.SetBackgroundColor(Color.Rgb(0x26, 0x75 ,0xFF)); //38, 117 ,255
			  }
			}

			// set our layout to be the home screen
			SetContentView(Resource.Layout.HomeScreen);

			//Find our controls
			taskListView = FindViewById<ListView> (Resource.Id.lstTasks);
			addTaskButton = FindViewById<Button> (Resource.Id.btnAddTask);

			// wire up add task button handler
			if(addTaskButton != null) {
				addTaskButton.Click += (sender, e) => {
					StartActivity(typeof(TaskDetailsScreen));
				};
			}
			
			// wire up task click handler
			if(taskListView != null) {
				taskListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                    var taskDetails = new Intent (this, typeof (TaskDetailsScreen));
                    taskDetails.PutExtra("TaskID", adapter.Items[e.Position].ID);
                    StartActivity (taskDetails);
				};
			}            
		}

        /// <summary>
        /// Registers for notifications.
        /// </summary>
        private void RegisterForNotifications()
        {
            string senders = "YOUR-GOOGLE-API-KEY";

            Intent intent = new Intent("com.google.android.c2dm.intent.REGISTER");
            intent.SetPackage("com.google.android.gsf");
            intent.PutExtra("app", PendingIntent.GetBroadcast(this, 0, new Intent(), 0));
            intent.PutExtra("sender", senders);
            StartService(intent);
        }
      
		protected override void OnResume()
		{
			base.OnResume ();

            adapter = new TaskListAdapter(this);
            taskListView.Adapter = adapter;
		}
	}
}