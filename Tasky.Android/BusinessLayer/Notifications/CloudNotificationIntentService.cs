using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Tasky.BL.Managers;

namespace Tasky.Droid.BusinessLayer.Notifications
{   
    [Service]
    public class CloudNotificationIntentService : IntentService
    {
        static PowerManager.WakeLock sWakeLock;
        static object mutex = new object();

        public static void RunIntentInService(Context context, Intent intent, string wakleLockTag)
        {
            lock (mutex)
            {
                if (sWakeLock == null)
                {
                    // This is called from BroadcastReceiver, there is no init.
                    var pm = PowerManager.FromContext(context);
                    sWakeLock = pm.NewWakeLock(WakeLockFlags.Partial, "My WakeLock Tag");
                }
            }

            sWakeLock.Acquire();
            intent.SetClass(context, typeof(CloudNotificationIntentService));
            context.StartService(intent);
        }

        protected override void OnHandleIntent(Intent intent)
        {
            try
            {
                Context context = this.ApplicationContext;
                string action = intent.Action;

                if (action.Equals("com.google.android.c2dm.intent.REGISTRATION"))
                {
                    // Registering device for notifications

                    var registrationId = intent.GetStringExtra("registration_id");
                    var error = intent.GetStringExtra("error");

                    //////////////////////////////////////////////////////

                    UserDeviceManager.Instance.Insert(new BL.UserDeviceRegistration { Name = "Larry", ServiceType = "GCM", ServiceKey = registrationId });

                    //////////////////////////////////////////////////////

                    SendLocalNotification("Tasky Pro Registered", "Ready for Notifications");
                }
                else if (action.Equals("com.google.android.c2dm.intent.RECEIVE"))
                {
                    // Receiving a push notification from GCM

                    SendLocalNotification("New Task Assigned!", intent.GetStringExtra("message"));
                }
            }
            finally
            {
                lock (mutex)
                {
                    //Sanity check for null as this is a public method
                    if (sWakeLock != null)
                        sWakeLock.Release();
                }
            }
        }

        private void SendLocalNotification(string displayTitle, string displayContent)
        {
            // Build the notification
            var builder = new NotificationCompat.Builder(this)
                                                .SetAutoCancel(true) // dismiss the notification from the notification area when the user clicks on it
                                                .SetContentTitle(displayTitle) // Set the title
                                                .SetSmallIcon(Resource.Drawable.launcher) // This is the icon to display
                                                .SetContentText(displayContent); // the message to display.

            // Finally publish the notification
            NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Notify(1234, builder.Build());
        }
    }
}