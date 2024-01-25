/*
 * Created on 2022
 *
 * Copyright (c) 2022 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Gley.MobileAds;
using BsplabsMobileAds;

public class ItemShop_VideoCoin : MonoBehaviour
{
    [Header("ITEM SHOP DATA")]
    public ConfigItemShopData itemShopData;

    public Text txtCounItem;
    public Text txtCountVideo;
    public BBUIButton btnBuy;
    public GameObject lockGroup;

    // Start is called before the first frame update
    void Start()
    {
        btnBuy.OnPointerClickCallBack_Completed.AddListener(TouchBuy);
        txtCounItem.text = $"x{itemShopData.countItem}";
    }

    private void OnEnable()
    {
        Init_ItemShop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TouchBuy()
    {
        lockGroup.SetActive(true);
        if (BspLabsAdsMobile.Instance.IsRewardVideoAvailable())
        {
            API.ShowRewardedVideo(CompleteMethod);
            BspLabsAdsMobile.Instance.ShowRewardedVideo(CompleteMethod);

        }
        else
        {
            lockGroup.gameObject.SetActive(false);
            NotificationPopup.instance.AddNotification("No Video Available!");
        }
        
    }

    private void CompleteMethod(bool completed)
    {
        
        if (completed == true)
        {
            lockGroup.SetActive(false);
            SoundManager.instance.PlaySound_Cash();

            Config.SetDaily_FreeCoin();
            Config.SetDaily_VideoCoin_AddCount();

            Config.BuySucces_ItemShop(itemShopData);

            if (Config.GetDaily_VideoCoin_Count() > 0)
            {
                txtCountVideo.text = $"{Config.GetDaily_VideoCoin_Count()}";
                btnBuy.Interactable = true;
            }
            else
            {
                txtCountVideo.text = $"0";
                btnBuy.Interactable = false;
            }
        }
        else
        {
            lockGroup.gameObject.SetActive(false);
            NotificationPopup.instance.AddNotification("Claim Reward Fail!");
        }
    }


    private void Init_ItemShop()
    {
        if (Config.CheckDaily_VideoCoin())
        {
            Config.SetDaily_VideoCoin();
            Config.SetDaily_VideoCoin_Count(Config.DAILY_VIDEOCOIN_MAX);

            txtCountVideo.text = $"{Config.GetDaily_VideoCoin_Count()}";
            btnBuy.Interactable = true;
        }
        else
        {
            if (Config.GetDaily_VideoCoin_Count() > 0)
            {
                txtCountVideo.text = $"{Config.GetDaily_VideoCoin_Count()}";
                btnBuy.Interactable = true;
            }
            else
            {
                
                txtCountVideo.text = $"0";
                btnBuy.Interactable = false;
            }
            
            
        }
    }
}