using System;
using MonoTouch.UIKit;
using Tasky.BL;
using MonoTouch.Dialog;
using Tasky.AL;
using System.Collections.Generic;

namespace Tasky 
{
	/// <summary>
	/// Wrapper class for Task, to use with MonoTouch.Dialog. If it was just iOS platform
	/// we could apply these attributes directly to the Task class, but because we share that
	/// with other platforms this wrapper provides a bridge to MonoTouch.Dialog.
	/// </summary>
	public class TaskDialog 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskDialog"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        public TaskDialog(UserTask task)
		{
			Name = task.Name;
			Notes = task.Notes;
			Done = task.Done;
            Assignees = new List<string> { "Larry", "Steve", "Tim" };

            if (!String.IsNullOrWhiteSpace(task.Assignee))
            {
                var selected = 0;

                foreach (var assignee in Assignees)
                {
                    if(assignee.Equals(task.Assignee,  StringComparison.InvariantCultureIgnoreCase))
                    {
                        CurrentAssignee = selected;
                        break;
                    }
                    selected++;
                }

            }
		}
		
		[Entry("task name")]
		public string Name { get; set; }

		[Entry("other task info")]
		public string Notes { get; set; }

        [Checkbox]
        public bool Done = true;

        [RadioSelection("Assignee")]
        public int CurrentAssignee;
        public IList<string> Assignees;
		
		[Section ("")]
		[OnTap ("SaveTask")]
		[Alignment (UITextAlignment.Center)]
    	public string Save;
		
		[Section ("")]
		[OnTap ("CancelTask")]
		[Alignment (UITextAlignment.Center)]
    	public string Cancel;
	}
}