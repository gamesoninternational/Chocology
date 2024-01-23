using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BsplabsMobileAds
{
    public class bannerads : MonoBehaviour
    {
        [SerializeField] BannerPosition bannerPosition;
        [SerializeField] bool removeads = false;

        [SerializeField] bool ForceRemoveAdsOnUpdate = false;
        public void Start()
        {
            if (!removeads)
            {
                if (bannerPosition == BannerPosition.TOP)
                {
                    PlayerPrefs.SetInt("bannerposition", 1);
                }
                if (!BspLabsAdsMobile.Instance.IsBannerOnScreen())
                {
                    if (BspLabsAdsMobile.Instance.IsBannerAvailable())
                    {
                        if (IsBuyItem()) return;
                        if (IsAdsRemoved()) return;

                        BspLabsAdsMobile.Instance.ShowBanner(bannerPosition, BannerType.Banner);
                    }
                }
            }
            else
            {
                if (BspLabsAdsMobile.Instance.IsBannerOnScreen() || !BspLabsAdsMobile.Instance.IsBannerAvailable())
                {
                    BspLabsAdsMobile.Instance.HideBanner();
                }
            }
        }

        public void RemoveBanner()
        {
            if (BspLabsAdsMobile.Instance.IsBannerOnScreen())
            {
                BspLabsAdsMobile.Instance.HideBanner();
            }
        }

        private void Update()
        {
            if (ForceRemoveAdsOnUpdate)
            {
                if (BspLabsAdsMobile.Instance.IsBannerOnScreen())
                {
                    BspLabsAdsMobile.Instance.HideBanner();
                }
            }
        }

        public static bool IsAdsRemoved()
        {
            int status = PlayerPrefs.GetInt("remove_ads", 0);
            return (status == 0) ? false : true;

        }

        public static bool IsBuyItem()
        {
            int status = PlayerPrefs.GetInt("buy_item", 0);
            return (status == 0) ? false : true;
        }


    }
}