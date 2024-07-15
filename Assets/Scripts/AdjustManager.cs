using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.adjust.sdk;

public class AdjustManager : Singleton<AdjustManager>
{

    [SerializeField] private string PlayGameToken;
    


    #region FIREBASE METHOD
    public void Start() => Initialize();

    void Initialize()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;

        if (!string.IsNullOrEmpty(GlobalVar.GetToknFCM()))
        {
            Adjust.setDeviceToken(GlobalVar.GetToknFCM());
        }
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
        GlobalVar.SaveTokenFCM(token.Token);

        Adjust.setDeviceToken(GlobalVar.GetToknFCM());
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }
    #endregion

    public void PlayGame() => DoEvent(PlayGameToken);
    

    //public void Setting() => DoEvent(SettingOpenToken);

    //public void OpenRewardedCoin() => DoEvent(RewardedCoin);

    //public void SpinChest() => DoEvent(spinChest);

    void DoEvent(string _token)
    {
        AdjustEvent adjust = new(_token);
        Adjust.trackEvent(adjust);
    }

}
