using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Redcode.Awaiting;

public class TimerFromServer : MonoBehaviour
{
    public static TimerFromServer instance;

    async void Start()
    {
        instance = this;
        DontDestroyOnLoad(this);

        NotificationPermission();

        dateTimeNowOnServer = await GetRealDateTimeFromAPTask();
        await StartCoroutine(CountDownTimer());
    }

    /// <summary>
    /// for requesting permissions
    /// </summary>
    void NotificationPermission()
    {
#if UNITY_ANDROID
        if (!GlobalVar.HasNotificationPermissionAndroid())
            GlobalVar.RequestNotificationPermissionAndroid();
#endif

        try
        {

#if UNITY_IOS
        var result = GlobalVar.RequestNotificationPermissionIOS();
#endif
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    IEnumerator CountDownTimer()
    {
        print("Calling");
        while (true)
        {
            if (doneGetTime)
            {
                dateTimeNowOnServer = dateTimeNowOnServer.AddSeconds(1);
                yield return new WaitForSeconds(1f);
            }
            else
            {
                yield return null;
            }
        }
    }


    public DateTime dateTimeNowOnServer { get; set; }
    public bool doneGetTime { get; set; }
    private const string WORLD_TIME_API_URL = "https://worldtimeapi.org/api/ip";
    struct TimeData
    {
        //public string client_ip;
        //...
        public string datetime;
        //..
    }

    /// <summary>
    /// Async to call real time API
    /// </summary>
    /// <returns></returns>
    public async Task<DateTime> GetRealDateTimeFromAPTask()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(WORLD_TIME_API_URL))
        {
            // Request and wait for the desired page.
            await webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    // Success
                    TimeData timeData = JsonUtility.FromJson<TimeData>(webRequest.downloadHandler.text);
                    dateTimeNowOnServer = ParseDateTime(timeData.datetime);
                    Debug.Log("Success.");
                    doneGetTime = true;
                    return dateTimeNowOnServer;
            }

            // Return a default DateTime if there was an error
            return await GetRealDateTimeFromAPTask();
        }
    }

    /// <summary>
    /// parse the json date time from real time API
    /// </summary>
    /// <param name="datetime"></param>
    /// <returns></returns>
    static DateTime ParseDateTime(string datetime)
    {
        //match 0000-00-00
        string date = Regex.Match(datetime, @"^\d{4}-\d{2}-\d{2}").Value;

        //match 00:00:00
        string time = Regex.Match(datetime, @"\d{2}:\d{2}:\d{2}").Value;

        return DateTime.Parse(string.Format("{0} {1}", date, time));
    }

    public DateTime CurrentServerTime(out bool _gotTime)
    {

        _gotTime = doneGetTime;


        if (_gotTime)
        {
            return dateTimeNowOnServer;
        }
        else
        {

            return DateTime.MinValue;
        }

    }
}
