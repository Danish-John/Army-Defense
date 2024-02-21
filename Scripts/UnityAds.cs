using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class UnityAds : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{


    [HideInInspector] string _InterAdUnitId = "Android_Interstitial";
    [HideInInspector] string _RewardAdUnitId = "Android_Rewarded";
    [HideInInspector] public bool interReady, rewardReady;
    public Action RewardVideoCompletedAction;
    public static UnityAds Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            //Instance = this;
        }
    }

    public void InitializeAds(string unityId, bool testOrNot)
    {
        Advertisement.Initialize(unityId, testOrNot, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        LoadInterstitialAd();
        LoadRewardedAd();

    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
    public void LoadInterstitialAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _InterAdUnitId);
        Advertisement.Load(_InterAdUnitId, this);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowInterstitialAd()
    {
        if (Application.internetReachability != NetworkReachability.ReachableViaCarrierDataNetwork &&
            Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork)
            return;
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + _InterAdUnitId);
        Advertisement.Show(_InterAdUnitId, this);
    }
    public void LoadRewardedAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _RewardAdUnitId);
        Advertisement.Load(_RewardAdUnitId, this);

    }

    // Show the loaded content in the Ad Unit:
    public void ShowRewardedAd()
    {
        if (Application.internetReachability != NetworkReachability.ReachableViaCarrierDataNetwork &&
            Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork)
            return;
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + _RewardAdUnitId);
        Advertisement.Show(_RewardAdUnitId, this);
    }
    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
        switch (adUnitId)
        {
            case "Android_Interstitial":
                {
                    interReady = true;
                    LoadRewardedAd();
                }
                break;
            case "Android_Rewarded":
                {
                    rewardReady = true;
                }
                break;
        }

    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {

    }

    public void OnUnityAdsShowClick(string adUnitId)
    {

    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId == _RewardAdUnitId && showCompletionState == UnityAdsShowCompletionState.SKIPPED)
        {

            //Notify User about not getting reward

        }
        if (adUnitId == _RewardAdUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            //Place Your Reward Functionality Here
            GiveReward();
            AdsManger_New.Instance.Get_Reward();

        }
        LoadInterstitialAd();
    }
    void GiveReward()
    {
        // complted?.Invoke();
        try
        {
            RewardVideoCompletedAction?.Invoke();
        }
        catch (Exception ex)
        {
            if (Debug.isDebugBuild) Debug.LogException(ex);
        }
    }
}