using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;
using GoogleMobileAds.Common;

public class AdsManger_New : MonoBehaviour
{

    public string rewardType;

    private AppOpenAd adAppOpen;
    public static AdsManger_New Instance;
    //All Ads Test IDs
    private string appID = "ca-app-pub-3940256099942544~3347511713";
    private string bannerID = "ca-app-pub-3940256099942544/6300978111";
    private string bannerID2 = "ca-app-pub-3940256099942544/6300978111";
    private string rect_bannerID = "ca-app-pub-3940256099942544/6300978111";
    private string rewardedID = "ca-app-pub-3940256099942544/5224354917";
    private string interstitialID = "ca-app-pub-3940256099942544/1033173712";
    //private string appOpnId = "ca-app-pub-3940256099942544/3419835294";
    
    
    
    
    ////Live
    //private string bannerID2 = "ca-app-pub-9684838765220423/1583517436";
    //private string rect_bannerID = "ca-app-pub-9684838765220423/5742012072";
    //private string appID = "ca-app-pub-9692774323588175~3513308290";
    //private string bannerID = "ca-app-pub-9692774323588175/8764677111";
    //private string rewardedID = "ca-app-pub-9692774323588175/3692313837";
    //private string interstitialID = "ca-app-pub-9692774323588175/3026223136";
    //private string Unity_ID = "1173013";
   // private string appOpnId = "ca-app-pub-9684838765220423/9167914862"; 


    //Banners Postions
    private AdPosition Small_Top_Left_Pos = AdPosition.Top;
    private AdPosition smartBannerPlaceTopMid = AdPosition.Top;
    private AdPosition smartBannerPlaceBottomMid = AdPosition.Bottom;

    //Large Banner Positioins

    private AdPosition Large_Bottom_Leftt_Pos = AdPosition.BottomLeft;


    //Reward Objects
    public RewardedAd rewardedAd;
    public RewardedInterstitialAd rewardedInterstitialAd;

    [SerializeField]
    private bool enableTestMode;
    //Banner Views
    private BannerView Small_Banner_View_Left_Top;
    private BannerView smartBannerTopMid;
    //Large Banner Views
    private BannerView Large_Banner_View_Left_Bottom;
    //Intersitial Banner
    private InterstitialAd interstitial;
  
   
   
    //Check Internet and Intitalization
    private bool isInternet = false;
    private bool isAdInitialized = false;
    public bool reqRewardedAdVideo = true;
  

    private AppOpenAd ad;
    private bool isShowingAd = false;
    [SerializeField] private Text Timer;
    [SerializeField] private Canvas AdsBreak;
    public static bool S_M_Banner, S_TL_Banner, L_BL_Banner, Not_Show;

    public bool Unity_Test_Mode;


    private void Awake()
    {

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

    }
    private bool CheckInitialization()
    {
        if (isAdInitialized)
        {
            isAdInitialized = true;
            return isAdInitialized;
        }
        else
        {
            isAdInitialized = false;
            InitializeAds();
            return false;
        }

    }
   

    public bool IsInternetConnection()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            isInternet = true;
        }
        else
            isInternet = false;

        return isInternet;
    }
    private void Start()
    {
        try
        {


#if UNITY_EDITOR
           // enableTestMode = true;
#endif
           
            if (IsInternetConnection())
            {
                InitializeAds();
            }
            else
                isAdInitialized = false;
        }
        catch (Exception exe)
        {
            Debug.Log(exe);

        }
    }
    void InitializeAds()
    {
        //LoadAd();
        //AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        isAdInitialized = true;
        MobileAds.Initialize(initStatus => { });
        //UnityAds.Instance.InitializeAds(Unity_ID,Unity_Test_Mode);
       
       
if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
        {
          
           
            
            try
            {
                RequestSmartBannerTopMid();
            }
            catch (Exception exe)
            {
                Debug.Log(exe);
            }
           
            
            try
            {
                RequestInterstitial();

            }
            catch (Exception exe)
            {
                Debug.Log(exe);

            }
        }
       
        ///////
        ///
        reqRewardedAdVideo = true;
        try
        {
           RequestReward();
        }
        catch (Exception exe)
        {
            Debug.Log(exe);
        }
       
       

    }
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }
    //Request Sending Small Banner
    public void Small_Banner_Req_Left_Top()
    {

        if (Small_Banner_View_Left_Top == null)
        {
            this.Small_Banner_View_Left_Top = new BannerView(bannerID, AdSize.Banner, Small_Top_Left_Pos);            //AdSize.Banner
            // Register for ad events.
            this.Small_Banner_View_Left_Top.OnAdLoaded += this.HandleAdLoadedLevels_Left_Top;
           
            // Load a banner ad.
            this.Small_Banner_View_Left_Top.LoadAd(this.CreateAdRequest());
            //if(SceneManager.GetActiveScene().buildIndex!=0)
            //{
            this.Small_Banner_View_Left_Top.Hide();
            //}
        }
    }
    //Request Sending Large Banner
    public void Large_Banner_Req_Left_Bottom()
    {

        if (Large_Banner_View_Left_Bottom == null)
        {
            //this.Large_Banner_View_Left_Bottom = new BannerView(rect_bannerID, AdSize.MediumRectangle, Large_Bottom_Leftt_Pos);
            // Register for ad events.
            this.Large_Banner_View_Left_Bottom.OnAdLoaded += this.HandleAdLoadedLevels_Left_Bottom_Large;

            // Load a banner ad.
            this.Large_Banner_View_Left_Bottom.LoadAd(this.CreateAdRequest());
            //if(SceneManager.GetActiveScene().buildIndex!=0)
            //{
            this.Large_Banner_View_Left_Bottom.Hide();
            //}
        }

    }
    //Show  Banner Functions   
    public void Small_Banner_Show_Left_Top()
    {
        if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
        {
            if (IsInternetConnection())
            {
                if (CheckInitialization())
                {
                    try
                    {
                        Banner_Setting("STL");
                        Small_Banner_View_Left_Top.Show();
                    }
                    catch (Exception exe)
                    {
                        Debug.Log(exe);
                    }
                }
            }
        }
    }
   // show Large Banner
    public void Large_Banner_Show_Left_Bottom()
    {
        if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
        {

            if (IsInternetConnection())
            {

                if (CheckInitialization())
                {

                    try
                    {

                        Banner_Setting("LBL");
                        Large_Banner_View_Left_Bottom.Show();

                    }
                    catch (Exception exe)
                    {
                        Debug.Log(exe);

                    }
                }
            }
        }
    }
    //Hide Banner Function
    public void Small_Banner_Hide_Left_Top()
    {
        if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
        {
            try
            {
                if (CheckInitialization())
                    Small_Banner_View_Left_Top.Hide();
            }
            catch (Exception exe)
            {
                Debug.Log(exe);

            }
        }
    }
   //Hide Large Banner
    public void Large_Banner_Hide_Left_Bottom()
    {
        if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
        {
            try
            {
                if (CheckInitialization())
                    Large_Banner_View_Left_Bottom.Hide();
            }
            catch (Exception exe)
            {
                Debug.Log(exe);

            }
        }
    }
    //Destroy Banner
    public void Small_Banner_Destroy_Left_Top()
    {
        Small_Banner_View_Left_Top.Destroy();
    }
   //Destroy Banner
    public void Large_Banner_Destroy_Left_Bottom()
    {
        Large_Banner_View_Left_Bottom.Destroy();
    }
    //Request Interstitial and Show
    private void RequestInterstitial()
    {
        this.interstitial = null;
        // Create an interstitial.
        this.interstitial = new InterstitialAd(interstitialID);
        // Register for ad events.
        this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
        this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
        // this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;
        // Load an interstitial ad.
        this.interstitial.LoadAd(this.CreateAdRequest());
    }
    public void ShowInterstitial()
    {
        if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
        {
            if (IsInternetConnection())
            {
                if (CheckInitialization())
                {
                    try
                    {
                        if (interstitial.IsLoaded())
                        {
                            interstitial.Show();
                        }
                        //else if (UnityAds.Instance.interReady)
                        //{
                        //    UnityAds.Instance.ShowInterstitialAd();
                        //}
                    }
                    catch (Exception exe)
                    {
                        Debug.Log(exe);

                    }
                }
            }
        }
    }
  
    ////App Open Start
    //public void ShowAppOpenAd()
    //{

    //    ShowAdIfAvailable();
    //}

    //private bool IsAdAvailable
    //{
    //    get
    //    {
    //        return adAppOpen != null;
    //    }
    //}

    //public void LoadAd()
    //{
    //    //AdRequest request = new AdRequest.Builder().Build();

    //    // Load an app open ad for portrait orientation
    //    //AppOpenAd.LoadAd(appOpnId, ScreenOrientation.Landscape, request, ((appOpenAd, error) =>
    //    //{
    //    //    if (error != null)
    //    //    {
    //    //        // Handle the error.
    //    //        Debug.LogFormat("Failed to load the ad. (reason: {0})", error.LoadAdError.GetMessage());
    //    //        return;
    //    //    }

    //    //    // App open ad is loaded.
    //    //    adAppOpen = appOpenAd;
    //    //}));
    //}
    //public void ShowAdIfAvailable()
    //{

    //    if (!IsAdAvailable || isShowingAd || !Not_Show)
    //    {
    //        return;
    //    }
    //    Hide_All_banner();
    //    AdsBreak.enabled = true;
    //    adAppOpen.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent;
    //    adAppOpen.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent;
    //    adAppOpen.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent;
    //    adAppOpen.OnAdDidRecordImpression += HandleAdDidRecordImpression;
    //    adAppOpen.OnPaidEvent += HandlePaidEvent;

    //    adAppOpen.Show();
    //    // Debug.Log("dsaf");
    //}

    //private void HandleAdDidDismissFullScreenContent(object sender, EventArgs args)
    //{
    //    Debug.Log("Closed app open ad");
    //    // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
    //    adAppOpen = null;
    //    isShowingAd = false;
    //    AdsBreak.enabled = false;

    //    Hide_onAPpp_open();
    //    LoadAd();
    //}

    //private void HandleAdFailedToPresentFullScreenContent(object sender, AdErrorEventArgs args)
    //{
      
    //    Debug.LogFormat("Failed to present the ad (reason: {0})", args.AdError.GetMessage());
    //    // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
    //   // Hide_onAPpp_open();
    //    AdsBreak.enabled = false;
    //    adAppOpen = null;
    //    LoadAd();
    //}

    //private void HandleAdDidPresentFullScreenContent(object sender, EventArgs args)
    //{
    //    Debug.Log("Displayed app open ad");
    //    isShowingAd = true;
    //}

    //private void HandleAdDidRecordImpression(object sender, EventArgs args)
    //{
      
    //   // Hide_onAPpp_open();
    //    Debug.Log("Recorded ad impression");
    //}

    //private void HandlePaidEvent(object sender, AdValueEventArgs args)
    //{
      
    //    Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
    //            args.AdValue.CurrencyCode, args.AdValue.Value);
    //}

    //public void OnAppStateChanged(AppState state)
    //{
    //    if (state == AppState.Foreground)
    //    {
    //        // TODO: Show an app open ad if available.
    //        ShowAdIfAvailable();
    //    }
    //}
    //End off App Open Ads

    //Request Reward
    public void RequestReward()
    {
        try
        {
            if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
            {
                if (this.reqRewardedAdVideo == true)
                {
                    this.rewardedAd = null;
                    this.rewardedAd = new RewardedAd(rewardedID);
                    this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
               //     this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
                    this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
                    this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
                    this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
                    this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;


                    this.rewardedAd.LoadAd(this.CreateAdRequest());
                    reqRewardedAdVideo = false;
                }
            }
            }
        catch (Exception exe)
        {
            Debug.Log(exe);

        }
    }
    public void UserChoseToWatchAd()
    {
        if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
        {
            if (IsInternetConnection())
            {
                if (CheckInitialization())
                {
                    try
                    {
                        if (this.rewardedAd.IsLoaded())
                        {
                            this.rewardedAd.Show();
                        }
                        else
                        {
                            Adsnotavailable_Fun();
                        }
                    }
                    catch (Exception exe)
                    {
                        Debug.Log(exe);

                    }
                }
            }
        }
        }
    
    
   
    #region Small Banner Levels callback handlers
    //Top Left banner Handler
    public void HandleAdLoadedLevels_Left_Top(object sender, EventArgs args)
    {
        try
        {
           // SmallBannerOnceLoadedLevels = true;
        }
        catch (Exception exe)
        {
            Debug.Log(exe);

        }
    }
   
    public void HandleAdLeftApplicationLevels_Left_Top(object sender, EventArgs args)
    {
        try
        {
            this.Small_Banner_View_Left_Top.OnAdLoaded -= this.HandleAdLoadedLevels_Left_Top;
        }
        catch (Exception exe)
        {
            Debug.Log(exe);

        }
        try
        {
           // this.Small_Banner_View_Left_Top.OnAdLeavingApplication -= this.HandleAdLoadedLevels_Left_Top;
        }
        catch (Exception exe)
        {
            Debug.Log(exe);

        }
    }


    //Bottom  Right  Large banner Handler
   
    public void HandleAdLoadedLevels_Left_Bottom_Large(object sender, EventArgs args)
    {
        try
        {
            // SmallBannerOnceLoadedLevels = true;
        }
        catch (Exception exe)
        {
            Debug.Log(exe);

        }
    }

   

    //Bottom Left banner Handler
    public void HandleAdLoadedBottomLeft(object sender, EventArgs args)
    {
        try
        {
         //   SmallBannerOnceLoadedBottomLeft = true;
        }
        catch (Exception exe)
        {
            Debug.Log(exe);

        }
    }
  

    #endregion

    #region RewardedAd Handling
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
            + args.Message);
      
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        try
        {
            rewardType = "";
            //For Testing Show
            //dummyyy
            //	RequestReward();
            //dummy
            Adsnotavailable_Fun();
        }
        catch (Exception exe)
        {
            Debug.Log(exe);
        }


        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
            + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
        Not_Show = true;
        this.reqRewardedAdVideo = true;
        try
        {
            
            this.RequestReward();
        }
        catch (Exception exe)
        {
            Debug.Log(exe);

        }
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        



        StartCoroutine(wait_Reward(args));
    }
    #endregion



  
    private void userEarnedRewardCallback(Reward reward)
    {
        try
        {

          // TODO: Reward the user.
          StartCoroutine(rewardUser(reward));
        }
        catch (Exception exe)
        {
            Debug.Log(exe);

        }
    }
 

    
    IEnumerator rewardUser(Reward args)
    {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(wait_Reward(args));
    }
  

    #region Interstitial callback handlers
    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {

    }
    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        try
        {
            Not_Show = true;
            RequestInterstitial();
            // Closeadbbreakunity();
        }
        catch (Exception exe)
        {
            Debug.Log(exe);
        }

        try
        {
        
           // StartCoroutine(wait_Reward());
        }
        catch (Exception exe)
        {
            Debug.Log(exe);

        }
    }
   
   
    #endregion
   


    //==================================================================================================================//

    //=============================================  Rewarded Ads with Priority  =======================================//


    public void Show_Rewarded_Ads_Priority(string rewType)
    {
        rewardType = rewType;

        try
        {
            Not_Show = false;
            if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
            {
                if (IsInternetConnection())
                {
                    if (rewardedAd.IsLoaded())
                    {
                        rewardedAd.Show();
                    }
                    //else if(UnityAds.Instance.rewardReady){
                    //    UnityAds.Instance.ShowRewardedAd();
                    //}
                    else
                    {
                       
                        //else
                        //{
                        //    RequestRewardBasedVideo(RewardedVideoID);
                        //}
                    }
                }

            }
        }
        catch (Exception exe)
        {
            Debug.Log(exe);
        }

    }
   
    //==================================================================================================================//

    //=============================================  Priority Ads  =====================================================//



    public void Show_inter_Admob()
    {

       
        if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
        {
          
            ShowUnityAdAdmobWithAdBreak();
        }
    }
   
   
    #region  Give Reward Funtion For All
    public void Get_Reward()
    {
        //Time.timeScale = 1;
        //string type = args.Type;
        //double amount = args.Amount;
        //MonoBehaviour.print(
        //    "HandleRewardedAdRewarded event received for "
        //    + amount.ToString() + " " + type);


        switch (rewardType)
        {


            case "PowerFill":

                GameManager.Instance.RewAdGameplayFunctionality();
                break;

            case "SpinnerAd":
                DailySpinner.Instance.AdFunc();
                break;

            rewardType = "";
        }

        Time.timeScale = 1f;
        //if (MenuManager.Menu_instance)
        //{
        //    MenuManager.Menu_instance.OnRewardedComplete();
        //}
        //if (GameManager.gamemanager_instance)
        //{
        //    GameManager.gamemanager_instance.OnRewardedComplete();
        //}
        //Not_Show = true;

    }




    IEnumerator wait_Reward(Reward args)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Get_Reward();
    }



    public void check_on_PC(Reward args)
    {


        StartCoroutine(wait_Reward(args));
    }
    #endregion

    #region Adsnotavailable_Function
    public void Adsnotavailable_Fun()
    {
        try
        {
           
            
        }
        catch (Exception exe)
        {
            Debug.Log(exe);
        }
    }
    #endregion


    #region Smart Banner Top Mid
    private void RequestSmartBannerTopMid()
    {
        if (smartBannerTopMid == null)
        {
           
            this.smartBannerTopMid = new BannerView(bannerID, AdSize.Banner, smartBannerPlaceBottomMid);
            // Register for ad events.
            this.smartBannerTopMid.OnAdLoaded += this.HandleAdLoadedSmartBannerTopMid;
            //this.smallbannerViewTM.OnAdLeavingApplication += this.HandleAdLeftApplicationSTM;
            // Called when an ad request failed to load.
            this.smartBannerTopMid.OnAdFailedToLoad += this.HandleOnAdFailedToLoadSmartBannerTopMid;
            // Load a banner ad.
            this.smartBannerTopMid.LoadAd(this.CreateAdRequest());
            this.smartBannerTopMid.Hide();
        }
    }

    public void ShowSmartBannerTopMid()
    {
        try
        {

            if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
            {

                if (IsInternetConnection())
                {

                    if (CheckInitialization())
                    {
                        Banner_Setting("SM");
                        smartBannerTopMid.Show();
                        Debug.Log("Smart Banner Shown");
                    }
                }
            }
        }
        catch (Exception exe)
        {
            Debug.Log(exe);
        }
    }

   public void HideSmartBannerTopMid()
    {
        try
        {
            if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
            {
                if (CheckInitialization())
                    smartBannerTopMid.Hide();
            }
        }
        catch (Exception exe)
        {
            Debug.Log(exe);
        }
    }
    public static bool adLoadedd2 = false;
    public void HandleAdLoadedSmartBannerTopMid(object sender, EventArgs args)
    {

        //SmallBannerOnceLoadedTopMid = true;
        adLoadedd2 = true;

    }



    public void HandleOnAdFailedToLoadSmartBannerTopMid(object sender, EventArgs args)
    {
        this.smartBannerTopMid.OnAdLoaded -= this.HandleAdLoadedSmartBannerTopMid;
        //this.smallbannerViewTM.OnAdLeavingApplication -= this.HandleAdLeftApplicationSTM;
    }
    public void HandleOnAdFailedToLoadSmartBannerTopMid(object sender, AdFailedToLoadEventArgs args)
    {

        //	MonoBehaviour.print("HandleFailedToReceiveAd Banner Menu event received with message: " + args.Message);
        //RequestBannerTM();

    }



    public void HideSmartBannerTopMidOnRemoveAd()
    {
        try
        {
            if (CheckInitialization())
                smartBannerTopMid.Hide();
        }
        catch (Exception exe)
        {
            Debug.Log(exe);
        }
    }

    public void DestroySmartBannerTopMid()
    {
        try
        {
            smartBannerTopMid.Destroy();
        }
        catch (Exception exe)
        {
            Debug.Log(exe);
        }
    }
    #endregion



    //intersitial Ads Show Ads
    public void ShowUnityAdAdmobWithAdBreak()
    {
        if (isInternet == true)
        {
            StartCoroutine(Adsbreak());

        }
        else
        {
            //if (InGameUi.instance != null)
            //{
            //    InGameUi.instance.EnableScreen(InGameUi.instance.ScreenToShow);
            //}
            Closeadbbreakunity();
        }

        // StartCoroutine(showAdmobUnityInterAdBreak());

    }
    public void Admob_intersitial() {
        if (IsInternetConnection())
        {
            if (CheckInitialization())
            {
                try
                {
                    if (interstitial.IsLoaded())
                    {
                        Not_Show = false;
                        interstitial.Show();
                    }
                    //else if (UnityAds.Instance.interReady)
                    //{
                    //    UnityAds.Instance.ShowInterstitialAd();
                    //}
                    else
                    {
                        RequestInterstitial();
                        //UnityAds.Instance.LoadInterstitialAd();
                    }

                }
                catch (Exception exe)
                {
                    Debug.Log(exe);

                }
            }
        }
    }
    IEnumerator Adsbreak()
    {
        // Time.timeScale = 1;
        if (!AdsBreak.isActiveAndEnabled)
        {
       
            AdsBreak.enabled = true;
        }
        Timer.gameObject.SetActive(true);
        Timer.text = "3";
       
        yield return new WaitForSecondsRealtime(0.5f);
        Timer.text = "2";
        //if (InGameUi.instance != null)
        //{

        //    InGameUi.instance.EnableScreen(InGameUi.instance.ScreenToShow);
        //}

       // StartCoroutine(showAdmobUnityInterAdBreak()); // my addition dummbee

        yield return new WaitForSecondsRealtime(0.6f);
        Timer.text = "1";
        Admob_intersitial();
        yield return new WaitForSecondsRealtime(0.7f);
        if (AdsBreak.isActiveAndEnabled)
        {
            AdsBreak.enabled = false;
            Closeadbbreakunity();
        }

    }

    public void Closeadbbreakunity()
    {
        AdsBreak.enabled = false;
        Timer.text = "3";
        /*     Call your level complete and level fail function in this condition    */
        if (Adsbreak() != null)
        {
            StopCoroutine(Adsbreak());
        }
    }

    public void Banner_Setting(string banner) {
        switch (banner) {
            case "SM":
                S_M_Banner = true;
                S_TL_Banner = false;
                L_BL_Banner = false;
                break;
            case "STL":
                S_M_Banner = false;
                S_TL_Banner = true;
                L_BL_Banner = false;
                break;
            case "LBL":
                S_M_Banner = false;
                S_TL_Banner = false;
                L_BL_Banner = true;
                break;
        
        }
       
    }
    public void Hide_onAPpp_open() {
       
        if (S_M_Banner)
        {
            AdsManger_New.Instance.ShowSmartBannerTopMid();
            AdsManger_New.Instance.Small_Banner_Hide_Left_Top();
            AdsManger_New.Instance.Large_Banner_Hide_Left_Bottom();
        }
        else if (S_TL_Banner)
        {
            AdsManger_New.Instance.Small_Banner_Show_Left_Top();
            AdsManger_New.Instance.HideSmartBannerTopMid();
            AdsManger_New.Instance.Large_Banner_Hide_Left_Bottom();
        }
        else if (L_BL_Banner) {
            AdsManger_New.Instance.Small_Banner_Hide_Left_Top();
            AdsManger_New.Instance.HideSmartBannerTopMid();
            AdsManger_New.Instance.Large_Banner_Show_Left_Bottom();
        }
       
    }
    public void Hide_All_banner() {
        AdsManger_New.Instance.Small_Banner_Hide_Left_Top();
        AdsManger_New.Instance.HideSmartBannerTopMid();
        AdsManger_New.Instance.Large_Banner_Hide_Left_Bottom();

    }




}

