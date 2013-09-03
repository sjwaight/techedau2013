using System;
using System.Xml;
using System.Runtime.Serialization;
using Tasky.BL.Contracts;
using Tasky.DL.SQLite;

namespace Tasky.BL
{
	/// <summary>
	/// Represents a Task.
	/// </summary>
	public class UserTask : IBusinessEntity
	{
        public UserTask() { }

        public int ID { get; set; }

#if !WINDOWS_PHONE
        [DataMember(Name = "name")]
#endif
		public string Name { get; set; }

#if !WINDOWS_PHONE
        [DataMember(Name = "notes")]
#endif
        public string Notes { get; set; }

#if !WINDOWS_PHONE
        [DataMember(Name = "done")]
#endif
		public bool Done { get; set; }

#if !WINDOWS_PHONE
        [DataMember(Name = "assignee")]
#endif
        public string Assignee { get; set; }
	}
}