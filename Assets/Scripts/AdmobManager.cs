/*
 * Created on 2022
 *
 * Copyright (c) 2022 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */

//using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
//using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
//using GoogleMobileAds.Api;
using Gley.MobileAds;
using BsplabsMobileAds;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager instance;
    



    public enum ADS_CALLBACK_STATE
    {
        SUCCESS,
        FAIL
    }

   

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {


        //API.Initialize();
        BspLabsAdsMobile.Instance.Initialize();

      //  Debug.Log("GDPR status check :" + API.GDPRConsentWasSet());
        if (!API.GDPRConsentWasSet())
        {
            API.ShowBuiltInConsentPopup(PopupClosed);
        }

    }

    private void PopupClosed()
    {
        //Resume game
    }
}
