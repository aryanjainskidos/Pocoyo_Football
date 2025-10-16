using UnityEngine;
using System;
using UnityEditor;
using Unity.Collections;

public class SAdsManager : MonoBehaviour
{
    public static SAdsManager instance = null;

    public enum AdPosition
    {
        Top,
        Bottom
    }

    public enum AdSize
    {
        BANNER,
        LARGER
    }

    public Action m_VideoRewardOK;
    public Action m_VideoRewardOpen;
    public Action m_VideoRewardClose;
    public Action m_VideoRewardFailShow;
    public Action m_BannerReady;
    public Action m_InterstitialOpen;
    public Action m_InterstitialClose;
    // public Action m_IronSourceInitialized;
    public Action m_InterstitialLoaded;

    public bool Consent = false;
    public bool DoNotSell = true;
    public String AppKey = "";
    public String AppKey_iOS = "";

    [Header("Banner")]
    public AdPosition position = AdPosition.Top;
    public AdSize size = AdSize.BANNER;
    [Header("Video Reward Data")]
    [ReadOnly]
    public string placementName;
    [ReadOnly]
    public string rewardName;
    [ReadOnly]
    public int rewardAmount;
    [Header("Mediation")]
    //[HideInInspector]
    public string AdmobID;
    //[HideInInspector]
    public string AdmobID_IOS;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    void OnApplicationPause(bool isPaused)
    {
        // IronSource.Agent.onApplicationPause(isPaused);
    }

    private void Start()
    {
        // IronSource.Agent.setConsent(Consent);
        // IronSource.Agent.setMetaData("do_not_sell", DoNotSell.ToString() );

        //Admob
        // IronSource.Agent.setMetaData("AdMob_TFCD", "true"); //For under 13
        // IronSource.Agent.setMetaData("AdMob_TFUA", "true"); //Is uder age for consent

        //AppLoving
        // IronSource.Agent.setMetaData("AppLovin_AgeRestrictedUser", "true"); //Under 13

        //Google Play AAIS stuff
        // IronSource.Agent.setMetaData("is_deviceid_optout", "true");
        // IronSource.Agent.setMetaData("is_child_directed", "true");

        //<uses-permission android:name="com.google.android.gms.permission.AD_ID" tools:node="remove"/>

#if UNITY_IOS
        // IronSource.Agent.init(AppKey_iOS);
#else
        // IronSource.Agent.init(AppKey);
#endif
        // IronSource.Agent.validateIntegration();

        // if (m_IronSourceInitialized != null)
        // {
        //     m_IronSourceInitialized.Invoke();
        // }
    }

    private void OnEnable()
    {
        //IronSourceEvents.onBannerAdLoadedEvent += HandleBannerLoaded;
        //IronSourceEvents.onBannerAdLoadFailedEvent += HandleBannerFailedToLoad;
        // IronSourceBannerEvents.onAdLoadedEvent += HandleBannerLoaded;
        // IronSourceBannerEvents.onAdLoadFailedEvent += HandleBannerFailedToLoad;

        //IronSourceEvents.onInterstitialAdReadyEvent += HandleInterstitialLoaded;
        //IronSourceEvents.onInterstitialAdLoadFailedEvent += HandleInterstitialFailedToLoad;
        //IronSourceEvents.onInterstitialAdClickedEvent += HandleInterstitialLeftApplication;
        //IronSourceEvents.onInterstitialAdOpenedEvent += HandleInterstitialOpening;
        //IronSourceEvents.onInterstitialAdClosedEvent += HandleInterstitialClosed;
        // IronSourceInterstitialEvents.onAdReadyEvent += HandleInterstitialLoaded;
        // IronSourceInterstitialEvents.onAdLoadFailedEvent += HandleInterstitialFailedToLoad;
        // IronSourceInterstitialEvents.onAdClickedEvent += HandleInterstitialLeftApplication;
        // IronSourceInterstitialEvents.onAdOpenedEvent += HandleInterstitialOpening;
        // IronSourceInterstitialEvents.onAdClosedEvent += HandleInterstitialClosed;

        //IronSourceEvents.onRewardedVideoAdOpenedEvent += HandleRewardBasedVideoStarted;
        //IronSourceEvents.onRewardedVideoAdClosedEvent += HandleRewardBasedVideoClosed;
        //IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += HandleRewardBasedVideoLoaded;
        //IronSourceEvents.onRewardedVideoAdRewardedEvent += HandleRewardBasedVideoRewarded;
        //IronSourceEvents.onRewardedVideoAdShowFailedEvent += HandleRewardBasedVideoFailedToLoad;
        // IronSourceRewardedVideoEvents.onAdOpenedEvent += HandleRewardBasedVideoStarted;
        // IronSourceRewardedVideoEvents.onAdClosedEvent += HandleRewardBasedVideoClosed;
        // IronSourceRewardedVideoEvents.onAdAvailableEvent += HandleRewardBasedVideoAvailable;
        // IronSourceRewardedVideoEvents.onAdUnavailableEvent += HandleRewardBasedVideoNotAvailable;
        // IronSourceRewardedVideoEvents.onAdRewardedEvent += HandleRewardBasedVideoRewarded;
        // IronSourceRewardedVideoEvents.onAdShowFailedEvent += HandleRewardBasedVideoFailedToLoad;
    }

#region BANNER
    public void LoadBanner()
    {
        Debug.Log("LOAD BANNER");
        // IronSourceBannerSize bannerSize = (size == AdSize.BANNER) ? IronSourceBannerSize.BANNER : IronSourceBannerSize.LARGE;
        // IronSourceBannerPosition bannerPos = (position == AdPosition.Top) ? IronSourceBannerPosition.TOP : IronSourceBannerPosition.BOTTOM;
        // IronSource.Agent.loadBanner(bannerSize, bannerPos);
        // IronSource.Agent.hideBanner();
    }

    public void HideBanner()
    {
        Debug.Log("BANNER HIDE");
        // IronSource.Agent.hideBanner();
    }

    public void ShowBanner()
    {
        Debug.Log("BANNER SHOW");
        // IronSource.Agent.displayBanner();
    }

    public void LoadAndShowBannerInPosition(AdPosition thisPosition)
    {
        Debug.Log("LOAD AND SHOW BANNER IN POSITION " + thisPosition.ToString());
        // IronSourceBannerSize bannerSize = (size == AdSize.BANNER) ? IronSourceBannerSize.BANNER : IronSourceBannerSize.LARGE;
        // IronSourceBannerPosition bannerPos = (thisPosition == AdPosition.Top) ? IronSourceBannerPosition.TOP : IronSourceBannerPosition.BOTTOM;
        // IronSource.Agent.destroyBanner();
        // IronSource.Agent.loadBanner(bannerSize, bannerPos);
    }

    public void HandleBannerLoaded (/* IronSourceAdInfo adInfo */)
	{
        if(m_BannerReady != null)
            m_BannerReady.Invoke();
	}

	public void HandleBannerFailedToLoad(/* IronSourceError error */)
	{
        Debug.LogError("BANNER FAILED - IronSource disabled");
	}
#endregion

#region INTERSTITIAL
    public void LoadInterstitial()
    {
        // IronSource.Agent.loadInterstitial();
    }

    public bool IsInterstitialLoad()
    {
#if UNITY_EDITOR
        return false;
#else
        // return IronSource.Agent.isInterstitialReady();
        return false;
#endif
    }

    public void ShowInterstitial()
    {
#if UNITY_EDITOR
        Invoke("HandleInterstitialClosed", 2f);
#else
        // if ( IronSource.Agent.isInterstitialReady() )
        // {
        //     Debug.Log("ShowInterstitial");
        //     IronSource.Agent.showInterstitial();
        // }
        Invoke("HandleInterstitialClosed", 2f);
#endif
    }

    public void HandleInterstitialLoaded (/* IronSourceAdInfo adInfo */)
	{
        if (m_InterstitialLoaded != null){
            m_InterstitialLoaded.Invoke();
        }
    }

	public void HandleInterstitialFailedToLoad(/* IronSourceError error */)
	{
        Debug.LogError("INTERSTITIAL FAILED - IronSource disabled");
    }

	public void HandleInterstitialOpening(/* IronSourceAdInfo adInfo */)
	{
#if UNITY_IOS
        //if ios platform pause game
            Time.timeScale = 0.0f;
#endif
        if (m_InterstitialOpen != null)
            m_InterstitialOpen.Invoke();
    }

	public void HandleInterstitialClosed(/* IronSourceAdInfo adInfo */)
	{
        //if ios platform unpause game
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Time.timeScale = 1.0f;
        }

        if (m_InterstitialClose != null)
            m_InterstitialClose.Invoke();
    }

	public void HandleInterstitialLeftApplication(/* IronSourceAdInfo adInfo */)
	{
    }
#endregion

#region VIDEOREWARD
    bool videoAvailable = false;

    public bool IsVideoRewardLoaded()
    {
        // return IronSource.Agent.isRewardedVideoAvailable();
        return false;
    }

    public void ShowRewardBasedVideo()
    {
        // if(IronSource.Agent.isRewardedVideoAvailable())
        //     IronSource.Agent.showRewardedVideo();
        // For now, just simulate success
        if(m_VideoRewardOK != null)
            m_VideoRewardOK.Invoke();
    }

    public void HandleRewardBasedVideoAvailable(/* IronSourceAdInfo adInfo */)
	{
        videoAvailable = true;
    }

    public void HandleRewardBasedVideoNotAvailable()
    {
        videoAvailable = false;
    }

    public void HandleRewardBasedVideoFailedToLoad(/* IronSourceError error, IronSourceAdInfo adInfo */)
	{
        Debug.LogError("VIDEOREWARD ERROR - IronSource disabled");
        if(m_VideoRewardFailShow != null)
            m_VideoRewardFailShow.Invoke();
    }

	public void HandleRewardBasedVideoStarted(/* IronSourceAdInfo adInfo */)
	{
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Time.timeScale = 0.0f;
        }

        if (m_VideoRewardOpen != null)
            m_VideoRewardOpen.Invoke();
    }

    public void HandleRewardBasedVideoClosed(/* IronSourceAdInfo adInfo */)
	{
        //if ios platform hide sound
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Time.timeScale = 1.0f;
        }

        Debug.Log("VIDEO CLOSED");

		if(m_VideoRewardClose != null)
            m_VideoRewardClose.Invoke();
    }

	public void HandleRewardBasedVideoRewarded(/* IronSourcePlacement placement, IronSourceAdInfo adInfo */)
	{
        Debug.Log("VIDEO REWARDED");

        // placementName = placement.getPlacementName();
        // rewardName = placement.getRewardName();
        // rewardAmount = placement.getRewardAmount();

		if(m_VideoRewardOK != null)
			m_VideoRewardOK.Invoke();
    }

	public void HandleRewardBasedVideoLeftApplication(/* IronSourceAdInfo adInfo */)
	{
        Debug.Log("VIDEO LEFT APPLICATION");
    }
#endregion
}
