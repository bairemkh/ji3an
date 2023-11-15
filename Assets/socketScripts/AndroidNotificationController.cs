using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.Android;

public class AndroidNotificationController : MonoBehaviour
{
    public void RequestAuthorization()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
    }

    public void RegisterNotificationChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public void SendNotification(string title, string content, int fireTimeInSeconds)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = content;
        notification.FireTime = System.DateTime.Now.AddSeconds(fireTimeInSeconds);

        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }
}