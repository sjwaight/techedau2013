using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Threading;
using Tasky.BL;
using System.Collections.ObjectModel;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Tasky.BL.Managers;

namespace TaskyWP7 
{
    /// <summary>
    /// Task List View Model
    /// </summary>
    public class TaskListViewModel : ViewModelBase 
    {

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public ObservableCollection<TaskViewModel> Items { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is updating.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is updating; otherwise, <c>false</c>.
        /// </value>
        public bool IsUpdating { get; set; }

        /// <summary>
        /// Gets or sets the list visibility.
        /// </summary>
        /// <value>
        /// The list visibility.
        /// </value>
        public Visibility ListVisibility { get; set; }

        /// <summary>
        /// Gets or sets the no data visibility.
        /// </summary>
        /// <value>
        /// The no data visibility.
        /// </value>
        public Visibility NoDataVisibility { get; set; }

        /// <summary>
        /// Gets the updating visibility.
        /// </summary>
        /// <value>
        /// The updating visibility.
        /// </value>
        public Visibility UpdatingVisibility
        {
            get
            {
                return (IsUpdating || Items == null || Items.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// The dispatcher
        /// </summary>
        Dispatcher dispatcher;

        /// <summary>
        /// Begins the update.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        public void BeginUpdate(Dispatcher dispatcher) {
            this.dispatcher = dispatcher;

            IsUpdating = true;

            ThreadPool.QueueUserWorkItem(async delegate {

                //var entries = new List<UserTask>();

                /////////////////////////

                var entries = await TaskManager.Instance.GetTasksForUser("Steve");

                /////////////////////////

                PopulateData(entries);
            });
        }

        /// <summary>
        /// Populates the data.
        /// </summary>
        /// <param name="entries">The entries.</param>
        void PopulateData(IEnumerable<UserTask> entries)
        {
            dispatcher.BeginInvoke(delegate {

                Items = new ObservableCollection<TaskViewModel>(
                    from e in entries
                    select new TaskViewModel(e));

                OnPropertyChanged("Items");

                ListVisibility = Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                NoDataVisibility = Items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

                OnPropertyChanged("ListVisibility");
                OnPropertyChanged("NoDataVisibility");
                OnPropertyChanged("IsUpdating");
                OnPropertyChanged("UpdatingVisibility");
            });
        }
    }
}