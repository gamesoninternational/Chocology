using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityEngine;
using System.Text.RegularExpressions;
using Redcode.Awaiting;
using UnityEngine.UI;
using System.Net;
using Newtonsoft.Json;
using UnityEngine.Android;
using UnityEngine.Playables;
using System.IO;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif

public static class GlobalVar
{
    #region PLAYER SAVE DATA
    public const string PlayerPrefs_MisteryChest = "mistery_chest";
    public const string PlayerPrefs_SpeedupChest = "speed_up_chest";
    public const string PlayerPrefs_FirebaseTokenFCM = "token_fcm";

    public static void SetMisteryChestTimer(string _timer)
    {

        PlayerPrefs.SetString(PlayerPrefs_MisteryChest, _timer);

        //set push notification
        CreateMisteryChestNotification(Convert.ToDateTime(_timer));
    }
    public static string GetMisteryChestTimer()
        => PlayerPrefs.GetString(PlayerPrefs_MisteryChest, "");

    public static void SetSpeedUpChest(bool _isUsed)
      => PlayerPrefs.SetInt(PlayerPrefs_SpeedupChest, (_isUsed) ? 1 : 0);

    public static bool IsSpeedUpUsed()
        => (PlayerPrefs.GetInt(PlayerPrefs_SpeedupChest, 0) == 0) ? false : true;

    public static void DeleteKey(string _dataKey)
        => PlayerPrefs.DeleteKey(_dataKey);

    public static void SaveTokenFCM(string _value)
        => PlayerPrefs.SetString(PlayerPrefs_FirebaseTokenFCM, _value);

    public static string GetToknFCM()
        => PlayerPrefs.GetString(PlayerPrefs_FirebaseTokenFCM, "");

    public static int GetCountDaysReward()
        => PlayerPrefs.GetInt("totalClaimDay", 1);

    public static void SetCountDaysReward(int value)
      => PlayerPrefs.SetInt("totalClaimDay", value);

    public static void SetDaysForClaim(string value)
    {

        PlayerPrefs.SetString("DayNextClaim", value);

        //set push notification
        CreateDailyRewardClaimNotification(Convert.ToDateTime(value));
    }

    public static string GetDaysForClaim()
      => PlayerPrefs.GetString("DayNextClaim", "");

    #endregion

    public static bool IsCanClaimDailyReward(int dayClick)
    {
        if (TimerFromServer.instance == null)
        {

        }
        bool _isDone = false;
        DateTime _currdate = TimerFromServer.instance.CurrentServerTime(out _isDone);
        if (!string.IsNullOrEmpty(GetDaysForClaim()))
        {

            DateTime lastDate = Convert.ToDateTime(GetDaysForClaim());

            if (dayClick == GetCountDaysReward() && _currdate >= lastDate)
            {
                return true;
            }
            else return false;

        }
        else
        {
            if (dayClick == 1)
                return true;
            else return false;
        }

    }


    #region FIREBASE ANALYTICS
    /// <summary>
    /// Insert the analytics
    /// </summary>
    /// <param name="_parameterName">Parameter name</param>
    public static void CallAnalytics(string _parameterName)
    {
        //hanling exception for firebase
        try
        {
            Debug.Log($"Calling {_parameterName}");
            if (Application.isMobilePlatform)
            {
                //Firebase.Analytics.FirebaseAnalytics.LogEvent(_parameterName);

            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    #endregion


    #region NOTIFICATION

    #region Handler

#if UNITY_ANDROID
    /// <summary>
    /// checker android notification permission
    /// </summary>
    /// <returns></returns>
    public static bool HasNotificationPermissionAndroid()
    {

        return Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS");

    }
#endif
    /// <summary>
    /// enable android notification permission
    /// </summary>
    public static void RequestNotificationPermissionAndroid()
    {
#if UNITY_ANDROID
        Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
#endif
    }

    /// <summary>
    /// request notification ios
    /// </summary>
    /// <returns></returns>
#if UNITY_IOS
    public static async Task<bool> RequestNotificationPermissionIOS()
    {

        try
        {
            using var req = new AuthorizationRequest(
         AuthorizationOption.Alert | AuthorizationOption.Badge, true);
            while (!req.IsFinished)
            {

                await new WaitForSeconds(Time.deltaTime);
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }

}
#endif
    #endregion

    public static void CreateMisteryChestNotification(DateTime _fireTime)
    => CreateNotification("mistery_chest_id", "Mistery Chest", "Mistery Chest", "Hey players,your chest is ready to claim up!!", _fireTime);

    public static void CreateDailyRewardClaimNotification(DateTime _fireTime)
    => CreateNotification("daily_reward_claim", "Daily Reward", "Daily Reward", "Hey players,your daily reward is ready to claim up!!", _fireTime);

    public static void CreateSpinClaimNotification(DateTime _fireTime)
    => CreateNotification("spin_claim", "Daily Reward", "Daily Reward", "Hey players, your free spin is ready to claim up!!", _fireTime);

    public static bool TryGetNotifImagePath(string path)
    {
        string urlImage = Path.Combine(Application.persistentDataPath, path);
        if (File.Exists(Path.Combine(Application.persistentDataPath, path)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void CreateNotification(string _id, string _name, string _notifTitle, string _notifText, DateTime _fireTime, bool _loop = false, bool _cancelIfSame = false, TimeSpan _repeateTime = default, string _notifImage = "")
    {

#if UNITY_ANDROID

        //cancel the duplicate notification that we create
        if (_cancelIfSame)
            AndroidNotificationCenter.CancelNotification(_id.GetHashCode());

        var channel = new AndroidNotificationChannel()
        {
            Id = _id,
            Name = _name,
            Importance = Importance.Default,
            Description = _notifTitle,
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        var notif = new AndroidNotification();
        notif.Title = _notifTitle;
        notif.Text = _notifText;
        notif.FireTime = _fireTime;
        notif.ShouldAutoCancel = true;
        if (TryGetNotifImagePath(_notifImage))
        {
            string urlImage = Path.Combine(Application.persistentDataPath, _notifImage);
            notif.BigPicture = new BigPictureStyle
            {
                Picture = urlImage
            };

        }

        if (_loop)
        {

            notif.RepeatInterval = (_repeateTime == default) ? new TimeSpan(24, 0, 0) : _repeateTime;
        }
        var idNotif = AndroidNotificationCenter.SendNotification(notif, _id);
#endif

#if UNITY_IOS

   if(_cancelIfSame)
            iOSNotificationCenter.RemoveScheduledNotification(_id);

var notifIos = new iOSNotification()
        {
            Identifier = _id,
            Title = _notifTitle,
            Body = _notifText,
            Subtitle = _notifText,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = _id,
            ThreadIdentifier = "thread1",
            Trigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = _fireTime - TimerFromServer.instance.dateTimeNowOnServer,
                Repeats = _loop 
            }
        };
        iOSNotificationCenter.ScheduleNotification(notifIos);
#endif
    }

    #endregion
}

