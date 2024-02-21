using UnityEngine;
using EasyUI.PickerWheelUI;
using DG.Tweening;


public class Spinner : MonoBehaviour
{
    public PickerWheel LevelCompSpinWheel;
    public PickerWheel LevelFailSpinWheel;

    public GameObject LevelCompNextBtn;
    public GameObject LevelCompRestartBtn;
    public GameObject LevelCompNoThanksBtn;


    public GameObject LevelFailRestartBtn;
    public GameObject LevelFailNoThanksBtn;


    // Start is called before the first frame update
    void Start()
    {

    }

    
    public void LevelCompSpin()
    {
        LevelCompSpinWheel.Spin();

        LevelCompNoThanksBtn.GetComponent<DOTweenAnimation>().enabled = false;
        LevelCompNoThanksBtn.SetActive(false);


        LevelCompSpinWheel.OnSpinEnd(WheelPiece =>
        {
            int multiplier = WheelPiece.Amount;

            if (multiplier != 0)
            {

                int NowCoins = LevelManager.Instance.LevelRewardCoins * multiplier;

                UIHandlingScript.Instance.LevelCompCoinsText.text = "+" + NowCoins.ToString();

                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - LevelManager.Instance.LevelRewardCoins);

                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + NowCoins);

            }
            //else
            //{
            //    return;
            //}

            LevelCompNextBtn.SetActive(true);
            LevelCompRestartBtn.SetActive(true);
        });

        
    }



    public void LevelFailSpin()
    {
        LevelFailSpinWheel.Spin();
        
        LevelFailNoThanksBtn.SetActive(false);


        LevelFailSpinWheel.OnSpinEnd(WheelPiece =>
        {
            int multiplier = WheelPiece.Amount;

            if (multiplier != 0)
            {

                int NowCoins = LevelManager.Instance.LevelFailCoins * multiplier;

                UIHandlingScript.Instance.LevelFailCoinsText.text = "+" + NowCoins.ToString();

                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - LevelManager.Instance.LevelFailCoins);

                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + NowCoins);

            }
            //else
            //{
            //    return;
            //}

            LevelFailRestartBtn.SetActive(true);
        });
    }



}
