
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

    //public ScrollRect scrollRect;
    //public Transform content;
    //public ItemLevel itemLevelPrefab;
    private void Awake()
    {
        instance = this;
    }

    enum LEVELPOPUP_ACTION_TYPE {
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
    public void SelectLevel(InfoLevel infoLevel) {
        infoLevelSelect = infoLevel;
        typeAction = LEVELPOPUP_ACTION_TYPE.SelectLevel;
        lockGroup.gameObject.SetActive(true);
        popup.GetComponent<BBUIView>().HideView();
    }

    public void HidePopup_Finished()
    {
        if (typeAction == LEVELPOPUP_ACTION_TYPE.SelectLevel) {
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


    public void InitLevels() {

        //for (int i = 0; i < Config.MAX_LEVEL; i++) {
        //    ItemLevel itemLevel = Instantiate(itemLevelPrefab, content);

        //    InfoLevel infoLevel = new InfoLevel(i + 1, 0);
        //    if (Config.currDic_LevelStars.ContainsKey(infoLevel.level)) {
        //        infoLevel.star = Config.currDic_LevelStars[infoLevel.level];
        //    }

        //    itemLevel.SetInitItemLevel(infoLevel);
        //}

        //scrollRect.verticalNormalizedPosition = 1f;
        int total_level_per_Map = Config.MAX_LEVEL / mapMaker.biomes.Count;
        int currentMap = 0;
        Config.GetCurrLevel();

        for (int map = mapMaker.biomes.Count - 1; map >= 0; map--)
        {
            
            int indexMap = map;
            int startLevel = (currentMap) * total_level_per_Map + 1;
            int endLevel = (currentMap + 1) * total_level_per_Map;

            mapMaker.biomes[indexMap].count = total_level_per_Map;
            mapMaker.biomes[indexMap].Create();
            mapMaker.biomes[indexMap].SetButtonsPositionOnCurve();


            for (int level = startLevel; level <= endLevel && level <= Config.MAX_LEVEL;)
            {

                for(int level_button = 0; level_button < mapMaker.biomes[indexMap].levelButtons.Count; level_button++)
                {
                    int currentLevel = level;
                    mapMaker.biomes[indexMap].levelButtons[level_button].button.onClick.AddListener(() => MenuManager.instance.SelectLevel(currentLevel));
                    mapMaker.biomes[indexMap].levelButtons[level_button].numberText.text = currentLevel.ToString();
                    if (Config.LevelStar(currentLevel) > 0)
                    {
                        mapMaker.biomes[indexMap].levelButtons[level_button].SetActive(false, Config.LevelStar(currentLevel), true);
                    }
                    else
                    {
                        
                        if(currentLevel == Config.currLevel)
                        {
                            mapMaker.biomes[indexMap].levelButtons[level_button].SetActive(true, Config.LevelStar(currentLevel), false);
                        }
                        else
                        {
                            mapMaker.biomes[indexMap].levelButtons[level_button].SetActive(false, Config.LevelStar(currentLevel), false);
                        }

                    }
                    
                    level++;  
                }
            }

            currentMap++;
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
