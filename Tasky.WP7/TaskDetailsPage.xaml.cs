using Microsoft.Phone.Controls;
using System;
using Tasky.BL;
using Tasky.BL.Managers;

namespace TaskyWP7 
{
    /// <summary>
    /// Task Detail Page (Add or Edit)
    /// </summary>
    public partial class TaskDetailsPage : PhoneApplicationPage 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskDetailsPage"/> class.
        /// </summary>
        public TaskDetailsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected async override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New) {

                var vm = new TaskViewModel();
                var task = default(UserTask);

                if (NavigationContext.QueryString.ContainsKey("id"))
                {
                    PageTitle.Text = "edit task";

                    var id = int.Parse(NavigationContext.QueryString["id"]);

                    /////////////////////
                    // Load task from Azure Mobile Services
                    /////////////////////

                    task = await TaskManager.Instance.GetTaskById(id);

                    // if we're loading the item, select the assignee.
                    if (!String.IsNullOrWhiteSpace(task.Assignee) && AssigneePicker != null && AssigneePicker.Items.Count > 0)
                    {
                        var count = 0;
                        foreach(var pickerItem in AssigneePicker.Items)
                        {
                            var item = pickerItem as ListPickerItem;
                            if (item != null)
                            {
                                if(item.Content.Equals(task.Assignee))
                                {
                                    AssigneePicker.SelectedIndex = count;
                                    break;
                                }
                            }
                            count++;
                        }
                    }
                }
                else
                {
                    PageTitle.Text = "add task";
                }

                if (task != null) 
                {
                    vm.Update(task);
                }

                DataContext = vm;

                AssigneePicker.SelectionChanged += AssigneePicker_SelectionChanged;
            }
        }

        /// <summary>
        /// Handles the save.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void HandleSave(object sender, EventArgs e)
        {
            var taskvm = (TaskViewModel)DataContext;
            var task = taskvm.GetTask();

            /////////////////////
            // Save task to Azure Mobile Services
            // This is an async method so the mobile app will simply continue on.
            /////////////////////

            TaskManager.Instance.Insert(task);


            NavigationService.GoBack();
        }

        /// <summary
        /// Handles the delete.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void HandleDelete(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        /// <summary>
        /// Handles the SelectionChanged event of the AssigneePicker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void AssigneePicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var taskvm = (TaskViewModel)DataContext;

            if (AssigneePicker != null && AssigneePicker.SelectedItem != null && e.AddedItems.Count > 0)
            {
                taskvm.Assignee = (e.AddedItems[0] as ListPickerItem).Content.ToString();
            }
        }
    }
}