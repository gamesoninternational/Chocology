using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    #region SINGLETON
    public static LoadingManager Instance { get; private set; }
    void SingletonSetup()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [SerializeField] private GameObject panelLoading;
    Coroutine _routineOpenLoading;
    private void Awake()
    {
        SingletonSetup();
    }

    public void SpawnLoading()
    {
        Debug.Log("Spawn loading");
        _routineOpenLoading = StartCoroutine(CloseLoadingRoutine());
        panelLoading.SetActive(true);
    }
    public void CloseLoading()
    {
        Debug.Log("Close loading");
        StopCoroutine(_routineOpenLoading);
        panelLoading.SetActive(false);
    }

    IEnumerator CloseLoadingRoutine()
    {
        int i = 10;
        while (i > 0)
        {
            i--;
            yield return new WaitForSeconds(1f);
        }
        panelLoading.SetActive(false);
    }
}
