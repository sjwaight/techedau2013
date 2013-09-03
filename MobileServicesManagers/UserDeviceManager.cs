using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Tasky.BL;

namespace Tasky.BL.Managers
{
    /// <summary>
    /// User Device Manager
    /// </summary>
    public class UserDeviceManager
    {
        private static readonly object mutex = new object();

        private readonly MobileServiceClient mobileService;
        
        private static UserDeviceManager instance;

        private List<UserDeviceRegistration> items;

        private readonly IMobileServiceTable<UserDeviceRegistration> table;

        private UserDeviceManager() 
        {
            mobileService = new MobileServiceClient("https://YOUR-SERVICE.azure-mobile.net/", "YOUR-KEY");
            table = mobileService.GetTable<UserDeviceRegistration>();
            items = new List<UserDeviceRegistration>();
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static UserDeviceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (mutex)
                    {
                        instance = new UserDeviceManager();
                    }
                }
                return instance;
            }
        }
 
        /// <summary>
        /// Mobiles the services table.
        /// </summary>
        /// <returns></returns>
        public IMobileServiceTable<UserDeviceRegistration> MobileServicesTable { get { return table; } private set {} }  

        /// <summary>
        /// Inserts the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public async void Insert(UserDeviceRegistration item)
        {
            this.table.InsertAsync(item);
        }
    }
}