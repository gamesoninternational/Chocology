//using Firebase;
//using Firebase.Analytics;
//using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    //public static FirebaseManager instance;
    //bool firebaseInitialized = false;
    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(this);
    //    }

    //}
    //private void Start()
    //{
    //    //Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
    //    //    Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
    //    //});
    //    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
    //    {
    //        if (task.Result == DependencyStatus.Available)
    //        {
    //            InitializeFirebase();
    //        }
    //        else
    //        {
    //            Debug.LogError(
    //             "Error Could not resolve all Firebase dependencies: " + task.Result);
    //        }
    //    });
    //}

    //void InitializeFirebase()
    //{
    //    Debug.Log("Enabling data collection.");
    //    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
    //    firebaseInitialized = true;

    //    Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    //    Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
    //    Debug.Log("Firebase Messaging Initialized");


    //    // This will display the prompt to request permission to receive
    //    // notifications if the prompt has not already been displayed before. (If
    //    // the user already responded to the prompt, thier decision is cached by
    //    // the OS and can be changed in the OS settings).
    //    Firebase.Messaging.FirebaseMessaging.RequestPermissionAsync().ContinueWithOnMainThread(
    //      task => {
    //          LogTaskCompletion(task, "RequestPermissionAsync");
    //      }
    //    );


    //    if (LoadSceneManager.instance.isActiveAndEnabled) {
    //        LoadSceneManager.instance.LoadMenuScene();
    //    }
    //}

    //public void LogLevelStart(int level)
    //{
    //    if (firebaseInitialized)
    //    {
    //        if (level < 381)
    //            Firebase.Analytics.FirebaseAnalytics.LogEvent("Lv_" + level + "_S");
    //        else
    //            Firebase.Analytics.FirebaseAnalytics.LogEvent("Lv_" + level + "_S", "Plus", (level - 380));
    //    }
    //}
    //public void LogLevelLose(int level)
    //{
    //    if (firebaseInitialized)
    //    {
    //        if (level < 381)
    //            Firebase.Analytics.FirebaseAnalytics.LogEvent("Lv_" + level + "_F");
    //        else
    //            Firebase.Analytics.FirebaseAnalytics.LogEvent("Lv_" + level + "_F", "Plus", (level - 380));
    //    }
    //}
    //public void LogLevelWin(int level)
    //{
    //    if (firebaseInitialized)
    //    {
    //        if (level < 381)
    //            Firebase.Analytics.FirebaseAnalytics.LogEvent("Lv_" + level + "_W");
    //        else
    //            Firebase.Analytics.FirebaseAnalytics.LogEvent("Lv_" + level + "_W", "Plus", (level - 380));
    //    }
    //}
    //public void LogShowInter(int curentLevel)
    //{
    //    if (firebaseInitialized)
    //    {
    //        Firebase.Analytics.FirebaseAnalytics.LogEvent("Ads_Inter", "CurentLevel", curentLevel);
    //    }
    //}
    //public void LogShowReward(string type)
    //{
    //    if (firebaseInitialized)
    //    {
    //        Firebase.Analytics.FirebaseAnalytics.LogEvent("Ads_Reward", "Type", type);
    //    }
    //}
    //public void LogRewarded()
    //{
    //    if (firebaseInitialized)
    //    {
    //        Firebase.Analytics.FirebaseAnalytics.LogEvent("Ads_Rewarded");
    //    }
    //}
    //public void LogShowNative(int curentLevel)
    //{
    //    if (firebaseInitialized)
    //    {
    //        Firebase.Analytics.FirebaseAnalytics.LogEvent("Ads_Native", "CurentLevel", curentLevel);
    //    }
    //}

    //public const string POWERUP_UNDO = "Undo";
    //public const string POWERUP_HINT = "Hint";
    //public const string POWERUP_SHUFFLE = "Shuffle";

    //public void LogUsePowerUp(string type, int curentLevel)
    //{
    //    if (firebaseInitialized)
    //    {
    //        Firebase.Analytics.FirebaseAnalytics.LogEvent("PowerUp_" + type, "CurentLevel", curentLevel);
    //    }
    //}


    //#region FIREBASE MESSAGE
    //public virtual void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    //{
    //    Debug.Log("Received a new message");
    //    var notification = e.Message.Notification;
    //    if (notification != null)
    //    {
    //        Debug.Log("title: " + notification.Title);
    //        Debug.Log("body: " + notification.Body);
    //        var android = notification.Android;
    //        if (android != null)
    //        {
    //            Debug.Log("android channel_id: " + android.ChannelId);
    //        }
    //    }
    //    if (e.Message.From.Length > 0)
    //        Debug.Log("from: " + e.Message.From);
    //    if (e.Message.Link != null)
    //    {
    //        Debug.Log("link: " + e.Message.Link.ToString());
    //    }
    //    if (e.Message.Data.Count > 0)
    //    {
    //        Debug.Log("data:");
    //        foreach (System.Collections.Generic.KeyValuePair<string, string> iter in
    //                 e.Message.Data)
    //        {
    //            Debug.Log("  " + iter.Key + ": " + iter.Value);
    //        }
    //    }
    //}

    //public virtual void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    //{
    //    Debug.Log("Received Registration Token: " + token.Token);

    //}

    //// Log the result of the specified task, returning true if the task
    //// completed successfully, false otherwise.
    //protected bool LogTaskCompletion(Task task, string operation)
    //{
    //    bool complete = false;
    //    if (task.IsCanceled)
    //    {
    //        Debug.Log(operation + " canceled.");
    //    }
    //    else if (task.IsFaulted)
    //    {
    //        Debug.Log(operation + " encounted an error.");
    //        foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
    //        {
    //            string errorCode = "";
    //            Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
    //            if (firebaseEx != null)
    //            {
    //                errorCode = String.Format("Error.{0}: ",
    //                  ((Firebase.Messaging.Error)firebaseEx.ErrorCode).ToString());
    //            }
    //            Debug.Log(errorCode + exception.ToString());
    //        }
    //    }
    //    else if (task.IsCompleted)
    //    {
    //        Debug.Log(operation + " completed");
    //        complete = true;
    //    }
    //    return complete;
    //}
    //#endregion

}
