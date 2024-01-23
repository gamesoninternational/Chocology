using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BsplabsMobileAds
{
    public class interstitialads : MonoBehaviour
    {
        public bool ShowOnStart = false;
        public bool ShowOnEnable = false;
        public bool IsUsingRate = true;
        public int ShowAfterSeconds = 60;
        public int ShowAfterCalling = 5;
        public bool UseSettingFromServer = true;
        public bool UseAsSingleton = false;

        public static interstitialads instance;

        private void Awake()
        {
            if (UseAsSingleton)
            {
                if (instance == null)
                {
                    instance = this;
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }
            }
        }

        //private time

        // Start is called before the first frame update
        void Start()
        {
            if (UseSettingFromServer)
            {
                ShowAfterSeconds = PlayerPrefs.GetInt("settingdelaybetweeninterstitial", 120);
                ShowAfterCalling = PlayerPrefs.GetInt("settingmincallbetweeninterstitialcalls", 5);
            }
            //get time from playerpref
            PlayerPrefs.GetFloat("bspadstime", 0);
            PlayerPrefs.GetInt("bspadscalls", 0);

            if (ShowOnStart)
            {
                ShowInterstitialAds();
            }

        }

        //on enable
        private void OnEnable()
        {
            if (ShowOnEnable)
            {
                ShowInterstitialAds();
            }
        }

        public void ShowInterstitialAds()
        {
            if (BspLabsAdsMobile.Instance.IsInterstitialAvailable())
            {


                if (IsUsingRate)
                {
                    //RateGame.Instance.IncreaseCustomEvents();
                    if (CheckTime() || CheckCalls())
                    {
                        BspLabsAdsMobile.Instance.ShowInterstitial();
                        //set playerpref time with current time
                        PlayerPrefs.SetFloat("bspadstime", Time.time);
                        //set playerpref calls with current calls
                        PlayerPrefs.SetInt("bspadscalls", 0);
                    }
                }
                else
                {
                    if (CheckTime() || CheckCalls())
                    {
                        BspLabsAdsMobile.Instance.ShowInterstitial();
                        //set playerpref time with current time
                        PlayerPrefs.SetFloat("bspadstime", Time.time);
                        //set playerpref calls with current calls
                        PlayerPrefs.SetInt("bspadscalls", 0);
                    }
                }

                PlayerPrefs.Save();
            }
        }


        //bool check if current time is greater than time from playerpref + seconds
        private bool CheckTime()
        {
            if (Time.time > PlayerPrefs.GetFloat("bspadstime", 0) + ShowAfterSeconds)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //bool check if calling is greater than ShowAfterCalling
        private bool CheckCalls()
        {
            if (PlayerPrefs.GetInt("bspadscalls", 0) == ShowAfterCalling)
            {
                return true;
            }
            else
            {
                PlayerPrefs.SetInt("bspadscalls", PlayerPrefs.GetInt("bspadscalls", 0) + 1);
                return false;
            }


        }
    }
}