using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Coffee.UIEffects;

public class DailyReward : MonoBehaviour
{
    public static DailyReward Instance;

    public Text MainTimerText;

    public Button[] Btns;

    public int[] DailyCoins;

    int BtnToBeUnlockedToday;

    private DateTime LastClaimTime;

    bool IsTimeRunning = false;


    void Start()
    {
        Instance = this;

        foreach (Button btn in Btns)
        {
            btn.interactable = false;
            btn.gameObject.GetComponent<UIShiny>().enabled = false;
        }

        string LastTime = PlayerPrefs.GetString("LastClaimTime", "");

        BtnToBeUnlockedToday = PlayerPrefs.GetInt("RewardBtnToday", 0);


        if (!string.IsNullOrEmpty(LastTime))
        {
            LastClaimTime = DateTime.Parse(LastTime);
        }
        else
        {
            LastClaimTime = DateTime.MinValue;
        }


        if (DateTime.Today > LastClaimTime)
        {
            //button on kro
            Debug.Log("dt today = " + DateTime.Today);
            Debug.Log("lastclaimtime = " + LastClaimTime);
            Btns[BtnToBeUnlockedToday].interactable = true;
            Btns[BtnToBeUnlockedToday].gameObject.GetComponent<UIShiny>().enabled = true;
            IsTimeRunning = false;
        }
        else
        {
            //button off kro
            IsTimeRunning = true;
            MainTimerText.text = GetTimeToNextClaim();
        }

    }

    private void Update()
    {
        if (IsTimeRunning)
        {
            MainTimerText.text = GetTimeToNextClaim();
        }
    }

    private string GetTimeToNextClaim()
    {
        int hours = Mathf.FloorToInt((float)(DateTime.Today.AddDays(1) - DateTime.Now).TotalHours);
        int minutes = Mathf.FloorToInt((float)(DateTime.Today.AddDays(1) - DateTime.Now).TotalMinutes) % 60;
        int seconds = Mathf.FloorToInt((float)(DateTime.Today.AddDays(1) - DateTime.Now).TotalSeconds) % 60;
        return (hours + " hours and " + minutes +" mins " + seconds + " secs left to claim next prize.");
    }


    public void OnClaimDailyRewardbBtn()
    {
        PlayerPrefs.SetString("LastClaimTime", DateTime.Now.ToString());
    }



    public void ClaimGift(int a)
    {
        IsTimeRunning = true;
        Btns[a].interactable = false;
        Btns[a].gameObject.GetComponent<UIShiny>().enabled = false;
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + DailyCoins[a]);
        GameManager.Instance.ResetCoinsText();
        MainTimerText.text = GetTimeToNextClaim();
        OnClaimDailyRewardbBtn();
        PlayerPrefs.SetInt("RewardBtnToday", PlayerPrefs.GetInt("RewardBtnToday") + 1);
        if (PlayerPrefs.GetInt("RewardBtnToday") == 6)
        {
            PlayerPrefs.SetInt("RewardBtnToday", 0);
        }
    }
}
