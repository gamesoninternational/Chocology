using UnityEngine;

namespace BsplabsMobileAds
{
    public class TestAds : MonoBehaviour
    {
        private readonly float buttonWidth = Screen.width / 4;
        private readonly float buttonHeight = Screen.height / 13;
        private readonly int nrOfButtons = 4;
        private bool showDetails = false;
        private bool bottom = true;

        private void Start()
        {
            //if user consent was set, just initialize the SDK, else request user consent
            if (BspLabsAdsMobile.Instance.UserConsentWasSet())
            {
                BspLabsAdsMobile.Instance.Initialize();
            }
        }

        private void OnGUI()
        {
            //get user consent
            //if consent was not set display 2 buttons to get it and a message to inform the user about what he can do
            if (!BspLabsAdsMobile.Instance.UserConsentWasSet())
            {
#if UNITY_IOS
            
            BspLabsAdsMobile.Instance.Initialize();
#else
                GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Do you prefer random ads in your app or ads relevant to you? If you choose Random no personalized data will be collected. If you choose personal all data collected will be used only to serve ads relevant to you.");
                if (GUI.Button(new Rect(buttonWidth, Screen.height - 5 * buttonHeight, buttonWidth, buttonHeight), "Personalized"))
                {
                    BspLabsAdsMobile.Instance.SetUserConsent(true);
                    //BspLabsAdsMobile.Instance.SetCCPAConsent(false);
                    BspLabsAdsMobile.Instance.Initialize();
                }
                if (GUI.Button(new Rect(2 * buttonWidth, Screen.height - 5 * buttonHeight, buttonWidth, buttonHeight), "Random"))
                {
                    BspLabsAdsMobile.Instance.SetUserConsent(false);
                    BspLabsAdsMobile.Instance.Initialize();
                }
#endif
            }
            //if consent was set, display buttons for ads
            else
            {
                if (GUI.Button(new Rect(0, Screen.height - buttonHeight, buttonWidth, buttonHeight), "Show Details"))
                {
                    showDetails = !showDetails;
                }

                if (GUI.Button(new Rect(buttonWidth, Screen.height - buttonHeight, buttonWidth, buttonHeight), "Consent:\nTrue"))
                {
                    BspLabsAdsMobile.Instance.SetUserConsent(true);
                }

                if (GUI.Button(new Rect(2 * buttonWidth, Screen.height - buttonHeight, buttonWidth, buttonHeight), "Consent:\nFalse"))
                {
                    BspLabsAdsMobile.Instance.SetUserConsent(false);
                }

                if (GUI.Button(new Rect(buttonWidth, Screen.height - 2 * buttonHeight, buttonWidth, buttonHeight), "Show Ads"))
                {
                    BspLabsAdsMobile.Instance.RemoveAds(false);
                }

                if (GUI.Button(new Rect(2 * buttonWidth, Screen.height - 2 * buttonHeight, buttonWidth, buttonHeight), "Remove Ads"))
                {
                    BspLabsAdsMobile.Instance.RemoveAds(true);
                }

                if (BspLabsAdsMobile.Instance.IsRewardVideoAvailable())
                {
                    if (GUI.Button(new Rect(0, 0, buttonWidth, buttonHeight), "Show Rewarded"))
                    {
                        BspLabsAdsMobile.Instance.ShowRewardedVideo(CompleteMethod);
                    }
                }

                if (BspLabsAdsMobile.Instance.IsInterstitialAvailable())
                {
                    if (GUI.Button(new Rect(1 * buttonWidth, 0, buttonWidth, buttonHeight), "Show Interstitial"))
                    {
                        BspLabsAdsMobile.Instance.ShowInterstitial(InterstitialClosed);
                    }
                }


                if (GUI.Button(new Rect(2 * buttonWidth, 0, buttonWidth, buttonHeight), "Show Banner"))
                {
                    BspLabsAdsMobile.Instance.ShowBanner(BannerPosition.BOTTOM);
                }

                if (GUI.Button(new Rect(2 * buttonWidth, buttonHeight, buttonWidth, buttonHeight), "On Screen?"))
                {
                    BsplabsMobileAds.ScreenWriter.Write("Banner is on screen: " + BspLabsAdsMobile.Instance.IsBannerOnScreen());
                }

                if (GUI.Button(new Rect(3 * buttonWidth, 0, buttonWidth, buttonHeight), "Hide Banner"))
                {
                    BspLabsAdsMobile.Instance.HideBanner();
                }

                if (GUI.Button(new Rect(3 * buttonWidth, buttonHeight, buttonWidth, buttonHeight), "Switch Banner"))
                {
                    if (bottom)
                    {
                        BspLabsAdsMobile.Instance.ShowBanner(BannerPosition.BOTTOM);
                    }
                    else
                    {
                        BspLabsAdsMobile.Instance.ShowBanner(BannerPosition.TOP);
                    }
                    bottom = !bottom;
                }

                #region ForEasyDebugPurpose
                if (showDetails)
                {
                    int nr = 0;
                    //Debug.Log(BspLabsAdsMobile.Instance.GetRewardedAdvertisers().Count);
                    for (int i = 0; i < BspLabsAdsMobile.Instance.GetRewardedAdvertisers().Count; i++)
                    {
                        if (BspLabsAdsMobile.Instance.GetRewardedAdvertisers()[i].advertiserScript.IsRewardVideoAvailable())
                        {
                            if (GUI.Button(new Rect((nr % nrOfButtons) * buttonWidth, (2 + nr / nrOfButtons) * buttonHeight, buttonWidth, buttonHeight), BspLabsAdsMobile.Instance.GetRewardedAdvertisers()[i].advertiser + " Rewarded"))
                            {
                                BspLabsAdsMobile.Instance.GetRewardedAdvertisers()[i].advertiserScript.ShowRewardVideo(CompleteMethod);
                            }
                            nr++;
                        }
                    }
                    nr = 0;
                    //Debug.Log(BspLabsAdsMobile.Instance.GetInterstitialAdvertisers().Count);
                    for (int i = 0; i < BspLabsAdsMobile.Instance.GetInterstitialAdvertisers().Count; i++)
                    {
                        if (BspLabsAdsMobile.Instance.GetInterstitialAdvertisers()[i].advertiserScript.IsInterstitialAvailable() && BspLabsAdsMobile.Instance.CanShowAds())
                        {
                            if (GUI.Button(new Rect((nr % nrOfButtons) * buttonWidth, (5 + nr / nrOfButtons) * buttonHeight, buttonWidth, buttonHeight), BspLabsAdsMobile.Instance.GetInterstitialAdvertisers()[i].advertiser + " Interstitial"))
                            {
                                BspLabsAdsMobile.Instance.GetInterstitialAdvertisers()[i].advertiserScript.ShowInterstitial(InterstitialClosed);
                            }
                            nr++;
                        }
                    }
                    nr = 0;
                    for (int i = 0; i < BspLabsAdsMobile.Instance.GetBannerAdvertisers().Count; i++)
                    {
                        if (BspLabsAdsMobile.Instance.GetBannerAdvertisers()[i].advertiserScript.IsBannerAvailable() && BspLabsAdsMobile.Instance.CanShowAds())
                        {
                            if (GUI.Button(new Rect((nr % nrOfButtons) * buttonWidth, (8 + nr / nrOfButtons) * buttonHeight, buttonWidth, buttonHeight), BspLabsAdsMobile.Instance.GetBannerAdvertisers()[i].advertiser + " Banner"))
                            {
                                BspLabsAdsMobile.Instance.GetBannerAdvertisers()[i].advertiserScript.ShowBanner(BannerPosition.BOTTOM, BannerType.Banner, null);
                            }
                            nr++;
                        }
                    }

                }
                #endregion
            }
        }

        //callback called each time an interstitial is closed
        private void InterstitialClosed(string advertiser)
        {
            if (BspLabsAdsMobile.Instance.debug)
            {
                Debug.Log("Interstitial closed from: " + advertiser + " -> Resume Game ");
                BsplabsMobileAds.ScreenWriter.Write("Interstitial closed from: " + advertiser + " -> Resume Game ");
            }
        }

        //callback called each time a rewarded video is closed
        //if completed = true, rewarded video was seen until the end
        private void CompleteMethod(bool completed, string advertiser)
        {
            if (BspLabsAdsMobile.Instance.debug)
            {
                Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
                BsplabsMobileAds.ScreenWriter.Write("Closed rewarded from: " + advertiser + " -> Completed " + completed);
                if (completed == true)
                {
                    //give the reward
                }
                else
                {
                    //no reward
                }
            }
        }
    }
}