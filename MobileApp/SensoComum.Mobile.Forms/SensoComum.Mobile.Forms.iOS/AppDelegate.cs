using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AppCenter;

using Foundation;
using UIKit;
using Microsoft.AppCenter.Push;

namespace SensoComum.Mobile.Forms.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            if (!AppCenter.Configured)
            {
                Push.PushNotificationReceived += (sender, e) =>
                {
                    var summary = $"Push notification received: " +
                        $"\n\tTitle: {e.Title}" +
                        $"\n\tMessage: {e.Message}";

                    if(e.CustomData != null)
                    {
                        summary += "\n\tCustom data:\n";
                        foreach(var key in e.CustomData.Keys)
                        {
                            summary += $"\t\t{key} : {e.CustomData[key]}\n";
                        }
                    }

                    System.Diagnostics.Debug.WriteLine(summary);
                };                
            }

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, System.Action<UIBackgroundFetchResult> completionHandler)
        {
            var result = Push.DidReceiveRemoteNotification(userInfo);
            if (result)
            {
                completionHandler?.Invoke(UIBackgroundFetchResult.NewData);
            }
            else
            {
                completionHandler?.Invoke(UIBackgroundFetchResult.NoData);
            }
        }
    }
}
