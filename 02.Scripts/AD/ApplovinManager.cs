using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ApplovinManager : MonoBehaviour
{   
    public static ApplovinManager I { get;  private set; }

    string adUnitId_reward = "07553bd32cb95a2c";
    int retryAttempt_reward;

    UnityEvent rewardEvent = new UnityEvent();

    static bool isAdLoaded = false;
    static bool isInitialized = false;

    float prevAdShowTime_reward = -3000f;
    const float adInterval_reward = 3 * 60f;

    void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        print("Hi Applovin");
        InitializeRewardedAds();
        InitializeInterstitialAds();
    }

    #region Reward

    public bool isRewardAdReady()
    {
        return 
            MaxSdk.IsRewardedAdReady(adUnitId_reward) && 
            (Time.time - prevAdShowTime_reward) > adInterval_reward;
    }

    public bool isRewardAdReady_lost()
    {
        return
            MaxSdk.IsRewardedAdReady(adUnitId_reward);
    }

    public void ShowRewardAd(UnityAction rewardAction)
    {
        if (MaxSdk.IsRewardedAdReady(adUnitId_reward))
        {
            prevAdShowTime_reward = Time.time;

            rewardEvent.RemoveAllListeners();
            rewardEvent.AddListener(rewardAction);

            MaxSdk.ShowRewardedAd(adUnitId_reward);
            print("Reward Showing..");
        } else
        {
            print("Reward Ad Not Ready");
        }
    }

    public void InitializeRewardedAds()
    {
        if (isInitialized) return;

        isInitialized = true;

        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        if (isAdLoaded) return;

        Utils.PrintDebug("Loading Ads...");
        MaxSdk.LoadRewardedAd(adUnitId_reward);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        isAdLoaded = true;
        Utils.PrintDebug($"Ad Loaded {MaxSdk.IsRewardedAdReady(adUnitId)}");

        // Reset retry attempt
        retryAttempt_reward = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        Utils.PrintDebug("Ad Load Failed");
        Utils.PrintDebug($"{MaxSdk.IsRewardedAdReady(adUnitId)}");

        retryAttempt_reward++;
        double retryDelay = System.Math.Pow(2, System.Math.Min(6, retryAttempt_reward));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {

        isAdLoaded = false;

        Utils.PrintDebug("Ad Displayed");
        Utils.PrintDebug($"{MaxSdk.IsRewardedAdReady(adUnitId)}");
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        Utils.PrintDebug("Ad Display Failed");
        Utils.PrintDebug($"{MaxSdk.IsRewardedAdReady(adUnitId)}");

        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        Utils.PrintDebug("Ad Clicked");
        Utils.PrintDebug($"{MaxSdk.IsRewardedAdReady(adUnitId)}");
    }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Utils.PrintDebug("Ad Hidden");
        Utils.PrintDebug($"{MaxSdk.IsRewardedAdReady(adUnitId)}");

        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        Utils.PrintDebug("Ad Receive Reward");
        Utils.PrintDebug($"{MaxSdk.IsRewardedAdReady(adUnitId)}");

        rewardEvent.Invoke();

        // The rewarded ad displayed and the user should receive the reward.
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        isAdLoaded = false;

        Utils.PrintDebug("Ad Revenue Paid");
        Utils.PrintDebug($"{MaxSdk.IsRewardedAdReady(adUnitId)}");

        // Ad revenue paid. Use this callback to track user revenue.
    }
    #endregion

    #region Interstitial

    string adUnitId_interstitial = "6d7fd3a50fe5a336";
    int retryAttempt_interstitial;

    float prevAdShowTime_interstitial = 0;
    const float adInterval_intersitial = 2 * 60f;

    public void ShowInterstitialAds()
    {
        float currTime = Time.time;
        if (currTime - prevAdShowTime_interstitial < adInterval_intersitial)
        {
            return;
        }

        prevAdShowTime_interstitial = currTime;

        if (MaxSdk.IsInterstitialReady(adUnitId_interstitial))
        {
            MaxSdk.ShowInterstitial(adUnitId_interstitial);
        }
    }

    public void InitializeInterstitialAds()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

        // Load the first interstitial
        LoadInterstitial();
    }

    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(adUnitId_interstitial);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        retryAttempt_interstitial = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttempt_interstitial++;
        double retryDelay = Mathf.Pow(2, Mathf.Min(6, retryAttempt_interstitial));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        LoadInterstitial();
    }

    #endregion
}
