using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIHandlingScript : MonoBehaviour
{
    public static UIHandlingScript Instance;
    
    public Text PowerLeftText;

    public GameObject GeneralUICanvas;

    public GameObject MissionCompletePanel;

    public GameObject MissionFailPanel;

    public Text LevelNumber;

    float LevelNum;

    public Text LevelFailCoinsText;
    
    public Text LevelCompCoinsText;

    public Sprite[] BarrierRenders;

    public Sprite[] PoliceRenders;

    public Image BarrierBtnImage;

    public Image PoliceBtnImage;




    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        SetUiOnStart();

    }

    public void SetUiOnStart()
    {
        PowerLeftText.text = GameController.Instance.PowerSlider.maxValue.ToString();
        LevelNum = PlayerPrefs.GetInt("LevelNumber") + 1;
        LevelNumber.text = "Level " + LevelNum.ToString();

        BarrierBtnImage.sprite = BarrierRenders[PlayerPrefs.GetInt("BarrierUpgradeNumber")];
        PoliceBtnImage.sprite = PoliceRenders[PlayerPrefs.GetInt("PoliceUpgradeNumber", 0)];

    }


    public void UpdatePowerText(string Power)
    {
        PowerLeftText.text = Power;
    }



    public void ShowMissionCompPanel()
    {
        GeneralUICanvas.SetActive(false);
        MissionCompletePanel.SetActive(true);
        LevelCompCoinsText.text = "+" + LevelManager.Instance.LevelRewardCoins.ToString();
    }







    public void ShowMissionFailPanel()
    {
        GeneralUICanvas.SetActive(false);
        MissionFailPanel.SetActive(true);
        LevelFailCoinsText.text = "+" + LevelManager.Instance.LevelFailCoins.ToString();
        
    }
}
