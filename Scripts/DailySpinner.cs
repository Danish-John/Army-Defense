using System;
using UnityEngine;
using UnityEngine.UI;
using EasyUI.PickerWheelUI;



public class DailySpinner : MonoBehaviour
{
    public static DailySpinner Instance;

    [SerializeField] private PickerWheel Spinner;
    [SerializeField] private Button SpinBtn;
    [SerializeField] private Text TimeText;
    [SerializeField] private Button AdBtn;



    private float timeRemaining;
    public bool timerIsRunning = false;

    private TimeSpan LastClaimTime;


    private void Start()
    {
        Instance = this;

        DateTime dt;
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("TimeRemainingForSpin")))
        {
            dt = DateTime.Parse(PlayerPrefs.GetString("TimeRemainingForSpin", ""));
        }
        else
        {
            dt = DateTime.MinValue;
        }
        TimeSpan tt = DateTime.Now.TimeOfDay - dt.TimeOfDay;
        timeRemaining = 10800 - ((float)tt.TotalSeconds);

        

        string LastSpinTime = PlayerPrefs.GetString("TimeRemainingForSpin", "");

        if (!string.IsNullOrEmpty(LastSpinTime))
        {
            LastClaimTime = dt.TimeOfDay;
        }
        else
        {
            LastClaimTime = TimeSpan.MinValue;
        }



        if (DateTime.Now.Date == dt.Date)
        {
            TimeSpan T1 = new TimeSpan(LastClaimTime.Hours, LastClaimTime.Minutes, LastClaimTime.Seconds);
            TimeSpan T2 = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            int hoursPassed = T2.Hours - T1.Hours;

            if (hoursPassed > 3)
            {
                //button on kro
                SpinBtn.interactable = true;
                //TimeText.gameObject.SetActive(false);
            }
            else
            {
                //button off kro
                SpinBtn.interactable = false;
                timerIsRunning = true;
                //TimeText.gameObject.SetActive(true);
            }
        }
        else
        {
            //button on kro
            SpinBtn.interactable = true;
            TimeText.gameObject.SetActive(false);
            AdBtn.gameObject.SetActive(false);
        }
    }


    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
                
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                //TimeText.gameObject.SetActive(false);
                TimeText.text = "";
                SpinBtn.interactable = true;
                AdBtn.gameObject.SetActive(false);
            }
        }
    }


    void DisplayTime(float timeToDisplay)
    {
        TimeSpan t = TimeSpan.FromSeconds(timeToDisplay);

        string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                        t.Hours,
                        t.Minutes,
                        t.Seconds);


        TimeText.text = answer + " left to claim next spin.";
    }


    public void Spin()
    {
        SpinBtn.interactable = false;
        TimeText.gameObject.SetActive(true);
        timeRemaining = 10800;
        PlayerPrefs.SetString("TimeRemainingForSpin", DateTime.Now.ToString());


        Spinner.Spin();


        Spinner.OnSpinEnd(WheelPiece =>
        {
            int amount = WheelPiece.Amount;

            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + amount);

            GameManager.Instance.ResetCoinsText();

            TimeText.gameObject.SetActive(true);

            timerIsRunning = true;

            AdBtn.gameObject.SetActive(true);
        });
    }



    public void AdBtnClick()
    {
        AdBtn.gameObject.SetActive(false);
        //#if !UNITY_EDITOR
        AdsManger_New.Instance.Show_Rewarded_Ads_Priority("SpinnerAd");
        //#endif
    }


    public void AdFunc()
    {
        timerIsRunning = false;
        TimeText.text = "";
        SpinBtn.interactable = true;
    }

}
