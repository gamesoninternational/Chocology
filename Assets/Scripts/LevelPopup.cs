
/*
 * Created on 2022
 *
 * Copyright (c) 2022 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using Mkey;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelPopup : MonoBehaviour
{
    public static LevelPopup instance;
    public BBUIButton btnClose;
    public GameObject popup;
    public GameObject lockGroup;
    public Text txtCounStar;
    public Mkey.MapMaker mapMaker;
    public bool DoneCreateMap { get; set; }
    //public ScrollRect scrollRect;
    //public Transform content;
    //public ItemLevel itemLevelPrefab;
    private void Awake()
    {
        instance = this;
    }

    enum LEVELPOPUP_ACTION_TYPE
    {
        SelectLevel,
        Close
    }

    LEVELPOPUP_ACTION_TYPE typeAction = LEVELPOPUP_ACTION_TYPE.Close;
    // Start is called before the first frame update
    void Start()
    {
        btnClose.OnPointerClickCallBack_Completed.AddListener(TouchClose);

        popup.GetComponent<BBUIView>().HideBehavior.onCallback_Completed.AddListener(HidePopup_Finished);
    }

    // Update is called once per frame
    void Update()
    {
        //if (isShowView) {
        //    scrollRect.velocity = Vector2.zero;
        //}
    }

    public void ShowLevelPopup()
    {
        gameObject.SetActive(true);
        InitViews();
    }

    public void TouchClose()
    {
        typeAction = LEVELPOPUP_ACTION_TYPE.Close;
        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();
        if (GamePlayManager.instance != null && GamePlayManager.instance.isActiveAndEnabled)
        {
            GamePlayManager.instance.SetUnPause();
        }
    }

    private InfoLevel infoLevelSelect;
    public void SelectLevel(InfoLevel infoLevel)
    {
        infoLevelSelect = infoLevel;
        typeAction = LEVELPOPUP_ACTION_TYPE.SelectLevel;
        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();
    }

    public void HidePopup_Finished()
    {
        if (typeAction == LEVELPOPUP_ACTION_TYPE.SelectLevel)
        {
            Config.isSelectLevel = true;
            Config.currSelectLevel = infoLevelSelect.level;

            {
                SceneManager.LoadScene("Play");
            }

        }
        gameObject.SetActive(false);
    }


    //bool isShowView = false;
    public void InitViews()
    {
        //isShowView = true;
        lockGroup.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
        btnClose.gameObject.SetActive(false);
        txtCounStar.text = $"{Config.GetCountStars()}/{Config.MAX_LEVEL * 3}";
        //foreach (Transform child in content)
        //{
        //    Destroy(child.gameObject);
        //}

        InitView_ShowView();

    }


    public void InitLevels()
    {
        int currentMap = 0;
        Config.GetCurrLevel();

        if (!DoneCreateMap)
            foreach (var a in mapMaker.biomes)
            {
                //spawn the map button
                a.Create();
                a.snapButtonsToCurve = true;
                a.SetButtonsPositionOnCurve();
            }

        if (!DoneCreateMap)
        {
            DoneCreateMap = true;
            int currentlvl = 1;
            for (int map = mapMaker.biomes.Count - 1; map >= 0; map--)
            {

                int indexMap = map;
                int startLevel = (currentMap) * mapMaker.biomes[indexMap].count + 1;
                int endLevel = startLevel + mapMaker.biomes[indexMap].count;

                foreach (var lvlButton in mapMaker.biomes[indexMap].levelButtons)
                {
                    lvlButton.numberText.text = currentlvl.ToString();
                    lvlButton.button.onClick.AddListener(() => MenuManager.instance.SelectLevel(int.Parse(lvlButton.numberText.text)));
                    if (Config.currLevel >= currentlvl)
                    {
                        lvlButton.SetActive(true, Config.LevelStar(currentlvl), true);
                    }
                    else
                    {
                        lvlButton.SetActive(false, Config.LevelStar(currentlvl), false);
                    }

                    currentlvl++;
                }
                currentMap++;
            }
        }

        MapController.Instance.SetActiveButton();
    }

    public void InitView_ShowView()
    {
        Sequence sequenceShowView = DOTween.Sequence();
        sequenceShowView.InsertCallback(0.01f, () =>
        {
            popup.gameObject.SetActive(true);
            popup.GetComponent<BBUIView>().ShowView();


        });
        sequenceShowView.InsertCallback(0.4f, () =>
        {
            btnClose.gameObject.SetActive(true);
            btnClose.gameObject.GetComponent<BBUIView>().ShowView();

            InitLevels();
        });


    }
}
