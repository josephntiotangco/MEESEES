using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using MEESEES.Commons;
using MEESEES.Droid.Helpers;
using MEESEES.Models;
using Java.Lang;
using Android;
using MEESEES.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(LocalNotificationService))]
namespace MEESEES.Droid.Helpers
{
    public class LocalNotificationService : ILocalNotification
    {
        private Context mContext;
        private NotificationManager mNotificationManager;
        private NotificationCompat.Builder mBuilder;
        public static string NOTIFICATION_CHANNEL_ID = "10023";
        public LocalNotificationService()
        {
            mContext = global::Android.App.Application.Context;
        }
        public void CreateNotification(string title, string message, int schedule = 0)
        {
            var intent = new Intent(mContext, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtra(title, message);
            var pendingIntent = PendingIntent.GetActivity(mContext, 0, intent, PendingIntentFlags.OneShot);

            var sound = global::Android.Net.Uri.Parse(ContentResolver.SchemeAndroidResource + "://" + mContext.PackageName + "/" + Resource.Raw.notification);
            // Creating an Audio Attribute
            var alarmAttributes = new AudioAttributes.Builder()
                .SetContentType(AudioContentType.Sonification)
                .SetUsage(AudioUsageKind.Notification).Build();

            mBuilder = new NotificationCompat.Builder(mContext,NOTIFICATION_CHANNEL_ID);
            mBuilder.SetSmallIcon(Resource.Drawable.ic_launcher); // edit
            mBuilder.SetContentTitle(title)
                    .SetSound(sound)
                    .SetAutoCancel(true)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetChannelId(NOTIFICATION_CHANNEL_ID)
                    .SetPriority((int)NotificationPriority.High)
                    .SetVibrate(new long[0])
                    .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                    .SetVisibility((int)NotificationVisibility.Public)
                    .SetSmallIcon(Resource.Drawable.ic_launcher) // edit
                    .SetContentIntent(pendingIntent);



            NotificationManager notificationManager = mContext.GetSystemService(Context.NotificationService) as NotificationManager;

            if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.O)
            {
                NotificationImportance importance = global::Android.App.NotificationImportance.High;

                NotificationChannel notificationChannel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, title, importance);
                notificationChannel.EnableLights(true);
                notificationChannel.EnableVibration(true);
                notificationChannel.SetSound(sound, alarmAttributes);
                notificationChannel.SetShowBadge(true);
                notificationChannel.Importance = NotificationImportance.High;
                notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });

                if (notificationManager != null)
                {
                    mBuilder.SetChannelId(NOTIFICATION_CHANNEL_ID);
                    notificationManager.CreateNotificationChannel(notificationChannel);
                }
            }

            notificationManager.Notify(schedule, mBuilder.Build());
        }
    //    int _notificationIconId { get; set; }
    //    readonly DateTime _jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    //    internal string _randomNumber;

    //    public void LocalNotification(string title, string body, int id, DateTime notifyTime)
    //    {
    //        long repeatForMinute = 6000;
    //        long totalMilliSeconds = (long)(notifyTime.ToUniversalTime() - _jan1st1970).TotalMilliseconds;
    //        if (totalMilliSeconds < JavaSystem.CurrentTimeMillis()) { totalMilliSeconds = totalMilliSeconds + repeatForMinute; }

    //        var intent = CreateIntent(id);
    //        var localNotification = new LocalNotification();
    //        localNotification.Title = title;
    //        localNotification.Body = body;
    //        localNotification.Id = id;
    //        localNotification.NotifyTime = notifyTime;
    //        if (_notificationIconId != 0)
    //        {
    //            localNotification.IconId = _notificationIconId;
    //        }
    //        else
    //        {
    //            localNotification.IconId = Resource.Drawable.fundBuddyIcon30;
    //        }
    //        var serializedNotification = SerializeNotification(localNotification);
    //        intent.PutExtra(ScheduledAlarmHandler.LocalNotificationKey, serializedNotification);

    //        Random generator = new Random();
    //        _randomNumber = generator.Next(100000, 999999).ToString("D6");

    //        var pendingIntent = PendingIntent.GetBroadcast(mContext, Convert.ToInt32(_randomNumber), intent, PendingIntentFlags.Immutable);
    //        var alarmManager = GetAlarmManager();
    //        alarmManager.SetRepeating(AlarmType.RtcWakeup, totalMilliSeconds, repeatForMinute, pendingIntent);
    //    }
    //    public void Cancel(int id)
    //    {

    //        var intent = CreateIntent(id);
    //        var pendingIntent = PendingIntent.GetBroadcast(mContext, Convert.ToInt32(_randomNumber), intent, PendingIntentFlags.Immutable);
    //        var alarmManager = GetAlarmManager();
    //        alarmManager.Cancel(pendingIntent);
    //        var notificationManager = NotificationManagerCompat.From(mContext);
    //        notificationManager.CancelAll();
    //        notificationManager.Cancel(id);
    //    }

    //    public static Intent GetLauncherActivity()
    //    {
    //        var packageName = Application.Context.PackageName;
    //        return Application.Context.PackageManager.GetLaunchIntentForPackage(packageName);
    //    }


    //    private Intent CreateIntent(int id)
    //    {

    //        return new Intent(mContext, typeof(ScheduledAlarmHandler))
    //            .SetAction("LocalNotifierIntent" + id);
    //    }

    //    private AlarmManager GetAlarmManager()
    //    {

    //        var alarmManager = Application.Context.GetSystemService(Context.AlarmService) as AlarmManager;
    //        return alarmManager;
    //    }

    //    private string SerializeNotification(LocalNotification notification)
    //    {

    //        var xmlSerializer = new XmlSerializer(notification.GetType());

    //        using (var stringWriter = new StringWriter())
    //        {
    //            xmlSerializer.Serialize(stringWriter, notification);
    //            return stringWriter.ToString();
    //        }
    //    }
    //}
    //[BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
    //public class ScheduledAlarmHandler : BroadcastReceiver
    //{
    //    public static string NOTIFICATION_CHANNEL_ID = "10023";
    //    public const string LocalNotificationKey = "LocalNotification";
    //    public override void OnReceive(Context context, Intent intent)
    //    {
    //        var extra = intent.GetStringExtra(LocalNotificationKey);
    //        LocalNotification notification = DeserializeNotification(extra);
    //        //Generating notification    
    //        var builder = new NotificationCompat.Builder(Application.Context, NOTIFICATION_CHANNEL_ID)
    //            .SetContentTitle(notification.Title)
    //            .SetContentText(notification.Body)
    //            .SetSmallIcon(notification.IconId)
    //            .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone))
    //            .SetAutoCancel(true);

    //        var resultIntent = LocalNotificationService.GetLauncherActivity();
    //        resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
    //        var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(Application.Context);
    //        stackBuilder.AddNextIntent(resultIntent);

    //        Random random = new Random();
    //        int randomNumber = random.Next(9999 - 1000) + 1000;

    //        var resultPendingIntent =
    //            stackBuilder.GetPendingIntent(randomNumber, (int)PendingIntentFlags.Immutable);
    //        builder.SetContentIntent(resultPendingIntent);
    //        // Sending notification    
    //        var notificationManager = NotificationManagerCompat.From(Application.Context);
    //        notificationManager.Notify(randomNumber, builder.Build());
    //    }

    //    private LocalNotification DeserializeNotification(string notificationString)
    //    {

    //        var xmlSerializer = new XmlSerializer(typeof(LocalNotification));
    //        using (var stringReader = new StringReader(notificationString))
    //        {
    //            var notification = (LocalNotification)xmlSerializer.Deserialize(stringReader);
    //            return notification;
    //        }
    //    }

    }
}