//using Newtonsoft.Json;
using System.Runtime.Serialization;
using Tasky.BL.Contracts;

namespace Tasky.BL
{
    /// <summary>
    /// Represents a Tasky User Device Regsitration.
    /// </summary>
    public class UserDeviceRegistration : IBusinessEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDeviceRegistration"/> class.
        /// </summary>
        public UserDeviceRegistration() { }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the service.
        /// </summary>
        /// <value>
        /// The type of the service.
        /// </value>
        public string ServiceType { get; set; }

        /// <summary>
        /// Gets or sets the service key.
        /// </summary>
        /// <value>
        /// The service key.
        /// </value>
        public string ServiceKey { get; set; }
    }
}