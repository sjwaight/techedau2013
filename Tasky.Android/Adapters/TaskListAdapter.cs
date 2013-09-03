using Android.Content;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tasky.BL;
//using Tasky.BL.Managers;

namespace Tasky.Droid.Adapters 
{
    /// <summary>
    /// Task List Adapter
    /// </summary>
    public class TaskListAdapter : BaseAdapter<UserTask> 
    {
        private readonly LayoutInflater inflater;
        private bool isUpdating;

        /// <summary>
        /// Occurs when [is updating changed].
        /// </summary>
        public event EventHandler IsUpdatingChanged;

        /// <summary>
        /// The items
        /// </summary>
        public List<UserTask> Items;

        /// <summary>
        /// Gets a value indicating whether this instance is updating.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is updating; otherwise, <c>false</c>.
        /// </value>
        public bool IsUpdating
        {
            get { return this.isUpdating; }
            private set
            {
                this.isUpdating = value;

                var changed = IsUpdatingChanged;
                if (changed != null)
                    changed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has stable ids.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has stable ids; otherwise, <c>false</c>.
        /// </value>
        public override bool HasStableIds
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public override int Count
        {
            get { return this.Items.Count; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="TaskListAdapter"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public TaskListAdapter(Context context)
		{
			this.inflater = (LayoutInflater)context.GetSystemService (Context.LayoutInflaterService);

            Items = new List<UserTask>();

			RefreshAsync();
		}

        /// <summary>
        /// Gets the <see cref="UserTask"/> with the specified position.
        /// </summary>
        /// <value>
        /// The <see cref="UserTask"/>.
        /// </value>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public override UserTask this[int position]
		{
			get { return this.Items[position]; }
		}

        /// <summary>
        /// Gets the item id.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
		public override long GetItemId (int position)
		{
			return this.Items[position].ID;
		}

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="convertView">The convert view.</param>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var item = this.Items[position];

            var view = (convertView ??
                        inflater.Inflate(Android.Resource.Layout.SimpleListItemChecked,parent,false)) as CheckedTextView;

            view.SetText(item.Name == "" ? "<new task>" : item.Name, TextView.BufferType.Normal);
            view.Checked = item.Done;

			return view;
		}

        /// <summary>
        /// Refreshes the async.
        /// </summary>
		public async void RefreshAsync()
		{
			IsUpdating = true;

            //this.Items = await TaskManager.Instance.GetTasksForUser("Larry");

            NotifyDataSetChanged();

            IsUpdating = false;
		}

        /// <summary>
        /// Inserts the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public async void Insert(UserTask item)
		{
			IsUpdating = true;

            //var serviceCall = TaskManager.Instance.Insert(item);

            //var result = await serviceCall;

            //if (!result)
            //{
            //    this.Items.Remove(item);
            //    NotifyDataSetChanged();
            //}

            IsUpdating = false;
		}
	}
}