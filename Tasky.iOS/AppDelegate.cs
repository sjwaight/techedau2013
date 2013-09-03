using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Tasky.AL;
using Tasky.BL.Managers;

namespace Tasky 
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate 
    {
		// class-level declarations
		UIWindow window;
		UINavigationController navController;
		UITableViewController homeViewController;
	
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);


            ////////////////////////////////////////
            // Register the application to receive APNS notifications.
            ////////////////////////////////////////

            UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(UIRemoteNotificationType.Alert
                                                                   | UIRemoteNotificationType.Badge
                                                                   | UIRemoteNotificationType.Sound);
			
			// make the window visible
			window.MakeKeyAndVisible ();
			
			// create our nav controller
			navController = new UINavigationController ();

			// create our home controller based on the device
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				homeViewController = new Tasky.Screens.iPhone.HomeScreen();
			} else {
				homeViewController = new Tasky.Screens.iPhone.HomeScreen(); // TODO: replace with iPad screen if we implement for iPad
			}
			
			// Styling
			UINavigationBar.Appearance.TintColor = UIColor.FromRGB (38, 117 ,255); // nice blue
			UITextAttributes ta = new UITextAttributes();
			ta.Font = UIFont.FromName ("AmericanTypewriter-Bold", 0f);
			UINavigationBar.Appearance.SetTitleTextAttributes(ta);
			ta.Font = UIFont.FromName ("AmericanTypewriter", 0f);
			UIBarButtonItem.Appearance.SetTitleTextAttributes(ta, UIControlState.Normal);
			

			// push the view controller onto the nav controller and show the window
			navController.PushViewController(homeViewController, false);
			window.RootViewController = navController;
			window.MakeKeyAndVisible ();
			
			return true;
		}

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            //The deviceToken is of interest here, this is what your push notification server needs to send out a notification
            // to the device.  So, most times you'd want to send the device Token to your servers when it has changed

            //First, get the last device token we know of
            string lastDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("deviceToken");

            //There's probably a better way to do this
            NSString strFormat = new NSString("%@");
            NSString newDeviceToken = new NSString(MonoTouch.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr(new MonoTouch.ObjCRuntime.Class("NSString").Handle, new MonoTouch.ObjCRuntime.Selector("stringWithFormat:").Handle, strFormat.Handle, deviceToken.Handle));

            //We only want to send the device token to the server if it hasn't changed since last time
            // no need to incur extra bandwidth by sending the device token every time
            if (!newDeviceToken.Equals(lastDeviceToken))
            {
                ///////////////////
                // Push the device registration to Azure Mobile Services.
                ///////////////////

                UserDeviceManager.Instance.Insert(new BL.UserDeviceRegistration { Name = "Tim", ServiceType = "APNS", ServiceKey = newDeviceToken });

                //Save the new device token for next application launch
                NSUserDefaults.StandardUserDefaults.SetString(newDeviceToken, "deviceToken");
            }
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            //Registering for remote notifications failed for some reason
            //This is usually due to your provisioning profiles not being properly setup in your project options
            // or not having the right mobileprovision included on your device
            // or you may not have setup your app's product id to match the mobileprovision you made

            Console.WriteLine("Failed to Register for Remote Notifications: {0}", error.LocalizedDescription);
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            //This method gets called whenever the app is already running and receives a push notification
            // YOU MUST HANDLE the notifications in this case.  Apple assumes if the app is running, it takes care of everything
            // this includes setting the badge, playing a sound, etc.
            processNotification(userInfo, false);
        }

        void processNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            //Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
            if (null != options && options.ContainsKey(new NSString("aps")))
            {
                //Get the aps dictionary
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

                string alert = string.Empty;
                string sound = string.Empty;
                int badge = -1;

                //Extract the alert text
                //NOTE: If you're using the simple alert by just specifying "  aps:{alert:"alert msg here"}  "
                //      this will work fine.  But if you're using a complex alert with Localization keys, etc., your "alert" object from the aps dictionary
                //      will be another NSDictionary... Basically the json gets dumped right into a NSDictionary, so keep that in mind
                if (aps.ContainsKey(new NSString("alert")))
                    alert = (aps[new NSString("alert")] as NSString).ToString();

                //Extract the sound string
                if (aps.ContainsKey(new NSString("sound")))
                    sound = (aps[new NSString("sound")] as NSString).ToString();

                //Extract the badge
                if (aps.ContainsKey(new NSString("badge")))
                {
                    string badgeStr = (aps[new NSString("badge")] as NSObject).ToString();
                    int.TryParse(badgeStr, out badge);
                }

                //If this came from the ReceivedRemoteNotification while the app was running,
                // we of course need to manually process things like the sound, badge, and alert.
                if (!fromFinishedLaunching)
                {
                    //Manually set the badge in case this came from a remote notification sent while the app was open
                    if (badge >= 0)
                        UIApplication.SharedApplication.ApplicationIconBadgeNumber = badge;

                    //Manually play the sound
                    if (!string.IsNullOrEmpty(sound))
                    {
                        //This assumes that in your json payload you sent the sound filename (like sound.caf)
                        // and that you've included it in your project directory as a Content Build type.
                        var soundObj = MonoTouch.AudioToolbox.SystemSound.FromFile(sound);
                        soundObj.PlaySystemSound();
                    }

                    //Manually show an alert
                    if (!string.IsNullOrEmpty(alert))
                    {
                        UIAlertView avAlert = new UIAlertView("Notification", alert, null, "OK", null);
                        avAlert.Show();
                    }
                }

            }

            //You can also get the custom key/value pairs you may have sent in your aps (outside of the aps payload in the json)
            // This could be something like the ID of a new message that a user has seen, so you'd find the ID here and then skip displaying
            // the usual screen that shows up when the app is started, and go right to viewing the message, or something like that.
            if (options.ContainsKey(new NSString("customKeyHere")))
            {
                //launchWithCustomKeyValue = (options[new NSString("customKeyHere")] as NSString).ToString();

                //You could do something with your customData that was passed in here
            }
        }
	}
}