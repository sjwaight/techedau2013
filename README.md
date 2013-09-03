TechEd Australia 2013 - 
============

This repository contains the sourcecode for the demo application used during this session at TechEd Australia 2013:

http://channel9.msdn.com/Events/TechEd/Australia/2013/AZR331

In order to get this solution to completely run you will need:

1. Visual Studio 2012 Professional (minimum) or Xamarin Studio.
2. Xamarin Business Licenses for iOS and Android.
3. Windows Azure Components from Xamarin store for iOs and Android (free).
4. Windows Phone 8 SDK.
5. Windows Azure Nuget package.
6. Android SDK.
7. A Mac and the iOS SDK.
8. A Windows Azure subscription.
9. Update the Mobile Service references in the TaskManager.cs and UserDeviceManager.cs files.
10. A Google API account.
11. Update the Google API reference in the Homescreen.cs RegisterForNotifications method.
12. An Apple Developer account (with APNS certificate generated).

This sourcecode is essentially the TaskyPro sample from Xamarin (authors: Bryan Costanich and Craig Dunn) that has been updated to include an assignee field as well as Windows Azure Mobile Services code to support push notifications.  You can find the original sourcecode here:

https://github.com/xamarin/mobile-samples/tree/master/TaskyPro