using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using UnityEngine.Events;
using System;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    public UnityEvent OnAdLoadedEvent;
    public UnityEvent OnAdFailedToLoadEvent;
    public UnityEvent OnAdOpeningEvent;
    public UnityEvent OnAdFailedToShowEvent;
    public UnityEvent OnUserEarnedRewardEvent;
    public UnityEvent OnAdClosedEvent;

    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    public RewardedAd rewardedAdFind;
    //public bool adsShow = false;

    #region BANNER AD UNIT ID
#if UNITY_EDITOR
    string bannerAdUnitId = "unused";
#elif UNITY_ANDROID
        //string bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111"; //sample
        string bannerAdUnitId = "ca-app-pub-7362583481441359/9146395536";
#elif UNITY_IPHONE
        string bannerAdUnitId = "ca-app-pub-3940256099942544/2934735716"; //sample
#else
        string bannerAdUnitId = "unexpected_platform";
#endif
    #endregion

    #region INTERSTITIAL AD UNIT ID

#if UNITY_EDITOR
    string interstitialAdUnitId = "unused";
#elif UNITY_ANDROID
        //string interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712"; //sample
        string interstitialAdUnitId = "ca-app-pub-7362583481441359/2006272119";
#elif UNITY_IPHONE
        string interstitialAdUnitId = "ca-app-pub-3940256099942544/4411468910"; //sample
#else
        string interstitialAdUnitId = "unexpected_platform";
#endif
    #endregion

    #region REWARDED ADS UNIT ID

#if UNITY_EDITOR
    const string rewardedAdFindUnitId = "unused";
#elif UNITY_ANDROID
        //const string rewardedAdFindUnitId = "ca-app-pub-3940256099942544/5224354917"; //SAMPLE
        const string rewardedAdFindUnitId = "ca-app-pub-7362583481441359/4249292075";     
#elif UNITY_IPHONE
        const string rewardedAdFindUnitId = "ca-app-pub-3940256099942544/1712485313"; //SAMPLE
#else
        const string rewardedAdFindUnitId = "unexpected_platform";
#endif

    #endregion

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        DontDestroyOnLoad(this);
    }

    void Start()
    {
#if UNITY_ANDROID || UNITY_IPHONE
        MobileAds.Initialize(InitializationStatus => { });
        this.RequestBanner(bannerAdUnitId);
        this.bannerView.Hide();
        this.RequestAndLoadInterstitialAd(interstitialAdUnitId);
        this.rewardedAdFind = RequestAndLoadRewardedAd(rewardedAdFindUnitId);
#endif
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

#region BANNER ADS
    private void RequestBanner(string adUnitId)
    {

        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

        // Load a banner ad
        bannerView.LoadAd(CreateAdRequest());
    }

    public void ShowBannerAd()
    {
        this.bannerView.Show();
    }

    public void HideBannerAd()
    {
        this.bannerView.Hide();
    }
#endregion

#region INTERSTITIAL ADS

    public void RequestAndLoadInterstitialAd(string adUnitId)
    {
        // Clean up interstitial before using it
        DestroyInterstitialAd();
        interstitialAd = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitialAd.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitialAd.OnAdClosed += HandleOnAdClosed;

        // Load an interstitial ad
        interstitialAd.LoadAd(this.CreateAdRequest());
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad is not ready yet");
        }
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        this.RequestAndLoadInterstitialAd(interstitialAdUnitId);
    }


#endregion

#region REWARDED ADS

    public RewardedAd RequestAndLoadRewardedAd(string adUnitId)
    {
        RewardedAd rewardedAd;
        // create new rewarded ad instance
        rewardedAd = new RewardedAd(adUnitId);

        //rewardedAd.OnAdLoaded += EnableFindButton;
        //rewardedAd.OnAdOpening += HandleOnAdOpening;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
       // rewardedAd.OnAdClosed += DisableFindButton;
        rewardedAd.OnUserEarnedReward += ReceiveFind;

        // Create empty ad request
        RequestRewardedVideoAd(rewardedAd);

        return rewardedAd;
    }

    private void RequestRewardedVideoAd(RewardedAd rewardedAd)
    {
        Debug.Log("request ads");
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }

    public void ShowFindRewardAd()
    {
        if (this.rewardedAdFind.IsLoaded())
        {
            this.rewardedAdFind.Show();
        }
        else
        {
            Debug.Log("rewarded Ad not loaded");
        }
    }
    public void EnableFindButton(object sender, EventArgs args)
    {
        //GameManager.Instance.findButton.interactable = true;
    }

    public void DisableFindButton(object sender, EventArgs args)
    {
        //GameManager.Instance.findButton.interactable = false;
    }

    public void HandleOnAdOpening(object sender, EventArgs args)
    {
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        RequestRewardedVideoAd((RewardedAd)sender);
    }

    public void ReceiveFind(object sender, EventArgs args)
    {
        Debug.Log("Rewarded");
        GameManager.Instance.FindButton();
    }

#endregion

    void Update()
    {
    }
}
