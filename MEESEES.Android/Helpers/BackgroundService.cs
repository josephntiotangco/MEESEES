using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MEESEES.Commons;
using Xamarin.Forms;

namespace MEESEES.Droid.Helpers
{
    [Service(Label = "BackgroundService")]
    public class BackgroundService : Service
    {
        int counter = 0;
        bool isRunningTimer = true;

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Device.StartTimer(TimeSpan.FromMinutes(30), () =>
            {
                MessagingCenter.Send<string>(counter.ToString(), "counterValue");
                counter += 1;
                if (counter > 1)
                {
                    var notif = new LocalNotificationService();
                    notif.CreateNotification("Fund Buddy", "Please input new expense/fund to update your account.");
                }
                return isRunningTimer;
            });
            return StartCommandResult.NotSticky;
        }
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        public override void OnDestroy()
        {
            StopSelf();
            counter = 0;
            isRunningTimer = false;
            base.OnDestroy();
        }
    }
}