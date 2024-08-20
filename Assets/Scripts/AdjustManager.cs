using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.adjust.sdk;

public class AdjustManager : Singleton<AdjustManager>
{

    [SerializeField] private string PlayGameToken;
    [SerializeField] private string BackToHomeToken;
    [SerializeField] private string BonusLevelClearRewardClaimedToken;
    [SerializeField] private string BreakPiggyBankToken;
    [SerializeField] private string ContinueWith100CoinToken;
    [SerializeField] private string ContinueWithAdsToken;
    [SerializeField] private string LevelClearToken;
    [SerializeField] private string LevelFailedToken;
    [SerializeField] private string LuckySpinClaimedToken;
    [SerializeField] private string NextLevelToken;
    [SerializeField] private string RestartGameToken;
    [SerializeField] private string ShopOpenToken;
    [SerializeField] private string HintToken;
    [SerializeField] private string IAPPurchasedSuccessToken;
    [SerializeField] private string IAPPurchasedFailedToken;

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
    public void BackToHome() => DoEvent(BackToHomeToken);
    public void BonusLevelClearRewardClaimed() => DoEvent(BonusLevelClearRewardClaimedToken);
    public void BreakPiggyBank() => DoEvent(BreakPiggyBankToken);
    public void ContinueWith100Coin() => DoEvent(ContinueWith100CoinToken);
    public void ContinueWithAds() => DoEvent(ContinueWithAdsToken);
    public void LevelClear() => DoEvent(LevelClearToken);
    public void LevelFailed() => DoEvent(LevelFailedToken);
    public void LuckySpinClaimed() => DoEvent(LuckySpinClaimedToken);
    public void NextLevel() => DoEvent(NextLevelToken);
    public void RestartGame() => DoEvent(RestartGameToken);
    public void ShopOpen() => DoEvent(ShopOpenToken);
    public void Hint() => DoEvent(HintToken);
    public void IAPPurchasedSuccess() => DoEvent(IAPPurchasedSuccessToken);
    public void IAPPurchasedFailed() => DoEvent(IAPPurchasedFailedToken);

    void DoEvent(string _token)
    {
        AdjustEvent adjust = new(_token);
        Adjust.trackEvent(adjust);
    }

}
