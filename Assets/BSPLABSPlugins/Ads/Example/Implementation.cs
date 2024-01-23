using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BsplabsMobileAds
{
    public class Implementation : MonoBehaviour
    {
        int coins = 0;
        public Text coinsText;
        public Button intersttialButton;
        public Button rewardedButton;

        /// <summary>
        /// Initialize the ads
        /// </summary>
        void Awake()
        {
            //BspLabsAdsMobile.Instance.Initialize();
        }


        void Start()
        {
            coinsText.text = coins.ToString();
        }

        /// <summary>
        /// Show banner, assigned from inspector
        /// </summary>
        public void ShawBanner()
        {
            BspLabsAdsMobile.Instance.ShowBanner(BannerPosition.BOTTOM);
        }

        public void HideBanner()
        {
            BspLabsAdsMobile.Instance.HideBanner();
        }


        /// <summary>
        /// Show Interstitial, assigned from inspector
        /// </summary>
        public void ShowInterstitial()
        {
            BspLabsAdsMobile.Instance.ShowInterstitial();
        }

        /// <summary>
        /// Show rewarded video, assigned from inspector
        /// </summary>
        public void ShowRewardedVideo()
        {
            BspLabsAdsMobile.Instance.ShowRewardedVideo(CompleteMethod);
        }


        /// <summary>
        /// This is for testing purpose
        /// </summary>
        void Update()
        {
            if (BspLabsAdsMobile.Instance.IsInterstitialAvailable())
            {
                intersttialButton.interactable = true;
            }
            else
            {
                intersttialButton.interactable = false;
            }

            if (BspLabsAdsMobile.Instance.IsRewardVideoAvailable())
            {
                rewardedButton.interactable = true;
            }
            else
            {
                rewardedButton.interactable = false;
            }
        }

        private void CompleteMethod(bool completed)
        {
            if (completed)
            {
                coins += 100;
            }

            coinsText.text = coins.ToString();
        }
    }
}