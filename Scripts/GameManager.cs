using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject[] Levels;

    public string[] EnvironmentScenes;

    public UnityEvent OnMissionComplete;

    [HideInInspector] public bool OnceWela = false;

    [HideInInspector] public bool CoinSetOnce = false;

    public Sprite BlueBG;

    public Sprite GreenBG;

    public Button PoliceBtn;

    public Button BarrierBtn;

    public Button CanonButton;

    public Button RewAdButton;

    public Slider PowerSlider;

    public Text MainPanelCoinsText;

    bool RewOnce = false;

    bool GameStart = false;

    [HideInInspector] public bool isGameFail = false;

    public GameObject ControlerImage;

    [HideInInspector] public bool isGamePassed = false; 

    private void Awake()
    {
        StartCoroutine(WaitForEnvironemntSceneLoad());
    }


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        PlayerPrefs.GetInt("LevelFailOnce", 0);


    }



    public void StartGameManager()
    {
        Levels[PlayerPrefs.GetInt("LevelNumber", 0)].SetActive(true);
        GameController.Instance.PowerSlider.maxValue = LevelManager.Instance.Power;
        GameController.Instance.PowerSlider.value = LevelManager.Instance.Power;
        GameStart = true;
        if (AdsManger_New.Instance)
        {
            AdsManger_New.Instance.ShowSmartBannerTopMid();
        }
        

    }
    

    IEnumerator WaitForEnvironemntSceneLoad()
    {
        AsyncOperation LoadingEnvironmentScene = SceneManager.LoadSceneAsync(EnvironmentScenes[PlayerPrefs.GetInt("LevelNumber", 0)], LoadSceneMode.Additive);

        while (!LoadingEnvironmentScene.isDone)
        {
            yield return null;
        }
    }
    
    
    
    
    
    
    
    private void Update()
    {
        if (LevelManager.Instance)
        {
            if (LevelManager.Instance.AllEnemiesSpawned && !OnceWela)
            {
                WelaPanCheck();
            }
        }

        if (GameStart && !isGamePassed)
        {
            if (!GameObject.FindGameObjectWithTag("Enemy") && LevelManager.Instance.AllEnemiesSpawned)
            {
                isGamePassed = true;
                ControlerImage.SetActive(false);
                OnMissionComplete.Invoke();
            }
        }
        


        if (PowerSlider.value <= 7 && !RewOnce)
        {
            GameObject[] Police = GameObject.FindGameObjectsWithTag("Police");
            GameObject[] Canons = GameObject.FindGameObjectsWithTag("Canon");
            if (Police.Length <= 18 && Canons.Length <=2)
            {
                RewAdButton.gameObject.SetActive(true);
                Invoke("DestroyRewAdBtn", 10.02f);
                RewOnce = true;
            }
        }


    }


    public void RewBtnOnClick()
    {
        RewAdButton.interactable = false;
        //#if !UNITY_EDITOR
        AdsManger_New.Instance.Show_Rewarded_Ads_Priority("PowerFill");
        //#endif
        Destroy(RewAdButton.gameObject);
    }


    public void DestroyRewAdBtn()
    {
        if (RewAdButton)
        {
            Destroy(RewAdButton.gameObject);
        }
    }


    public void RewAdGameplayFunctionality()
    {
        PowerSlider.value = LevelManager.Instance.Power;
        UIHandlingScript.Instance.UpdatePowerText(PowerSlider.value.ToString());
        //RewAdButton.gameObject.SetActive(false);
    }



    public void BarrierButtonBGSet()
    {
        BarrierBtn.image.sprite = GreenBG;
        PoliceBtn.image.sprite = BlueBG;
        CanonButton.image.sprite = BlueBG;

    }



    public void PoliceButtonBGSet()
    {
        PoliceBtn.image.sprite = GreenBG;
        BarrierBtn.image.sprite = BlueBG;
        CanonButton.image.sprite = BlueBG;
    }



    public void CanonButtonBGSet()
    {
        CanonButton.image.sprite = GreenBG;
        PoliceBtn.image.sprite = BlueBG;
        BarrierBtn.image.sprite = BlueBG;
    }



    public void SetCoins()
    {
        if (!CoinSetOnce)
        {
            CoinSetOnce = true;
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + LevelManager.Instance.LevelRewardCoins);
        }
    }

    public void ResetCoinsText()
    {
        MainPanelCoinsText.text = PlayerPrefs.GetInt("Coins").ToString();
    }

    void WelaPanCheck()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] PoliceMen = GameObject.FindGameObjectsWithTag("Police");

        if (PoliceMen.Length >= Enemies.Length)
        {
            foreach (GameObject policemen in PoliceMen)
            {
                PoliceAI pulci = policemen.GetComponent<PoliceAI>(); 
                
                if (Enemies.Length == 6)
                {
                    OnceWela = true;
                    
                    GameObject tar = Enemies[Random.Range(0, Enemies.Length - 1)].GetComponentInParent<EnemyAI>().gameObject;
                    
                    if (!tar.GetComponentInParent<EnemyAI>().isDead && pulci && !pulci.isDead)
                    {
                        pulci.Target = tar.transform;
                        pulci.Police.SetDestination(tar.transform.position);
                    }
                }
            }
        }
    }



    //public void GameComplete()
    //{
    //    Time.timeScale = 0;
    //    UIHandlingScript.Instance.ShowMissionCompPanel();
    //}



    public void MissionCompNext()
    {
        PlayerPrefs.SetInt("LevelNumber", PlayerPrefs.GetInt("LevelNumber") + 1);
        
        int levellength = Levels.Length;
        
        if (PlayerPrefs.GetInt("LevelNumber") == levellength)
        {
            PlayerPrefs.SetInt("LevelNumber", 0);
        }
 
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
        

    }



    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
       
    }





}
