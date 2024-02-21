using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBar : MonoBehaviour
{
    public static LoadingBar instance;
    public Slider LoadingSlider;
    public GameObject MainObj;
    private float timer;
    public float maxDummyLoadingTimer;
    private float LerpingValue;
    private bool isDummyLoading;

    public static bool NewGame;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
           
        else
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
    }
    void Start()
    {
        NewGame = true;
        timer = 0;
        //LoadScene("MainMenu");
        isDummyLoading = true;
        StartCoroutine(DummyLoadingScreen());

    }

    private void FixedUpdate()
    {
        if(isDummyLoading)
        {
            LerpingValue = Mathf.Lerp(LoadingSlider.value, timer / maxDummyLoadingTimer, 0.01f);
            LoadingSlider.value = LerpingValue;
        }
        
    }
    IEnumerator DummyLoadingScreen()
    {
        while(timer <= maxDummyLoadingTimer)
        {
            if(timer== maxDummyLoadingTimer)
            {

                MainObj.SetActive(false);
                LoadingSlider.value = 0;
                SceneManager.LoadSceneAsync("Gameplay");
                
                isDummyLoading = false;
            }
          //LoadingSlider.value = timer / maxDummyLoadingTimer;
            timer++;

            yield return new WaitForSeconds(4f);
        }


    }

    public void LoadScene(string sceneName)
    {
        MainObj.SetActive(true);
        LoadingSlider.value = 0;
        StartCoroutine(Startloading(sceneName));
    }
    IEnumerator Startloading(string scenename)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scenename);

       
        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingSlider.value = progress;

            yield return null;
        }
    }
}
