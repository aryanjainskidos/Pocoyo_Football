using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NativeGalleryNamespace;


public class Main : MonoBehaviour
{
    public enum UIModes
    {
        UIMODE_NULL = -1,
        UIMODE_DEFAULT = 0,
        UIMODE_VUVUCELA = 1,
        UIMODE_BOMBO = 2,
        UIMODE_SILBATO = 3,
        UIMODE_JEMBE = 4,
        UIMODE_BOCINA = 5,
        UIMODE_GAME = 6,
        UIMODE_BAILE = 7,
        UIMODE_CLOTHES = 8,
        UIMODE_AR = 9,
    }

    public enum TouchAnimation
    {
        TA_HEAD,
        TA_TOE,
        TA_HAND,
        TA_TORSO,
    }

    public GameObject Menu;
    public GameObject InfoScreen;
    public GameObject MarketScreen;
    public GameObject ParentalControlPopup;
    public CheckParental CheckParental;
    public GameObject RewardPopup;
    [Header("Teams IAP")]
    public GameObject TeamMarketScreen;
    public Zinkia.IAPControler teamMarketIAP;
    [Header("UI Tweens")]
    public UIPositionTween Instrumentos;
    public UIPositionTween Bailes;
    public UIPositionTween TwoKeys;
    public UIPositionTween Customize;
    public UIPositionTween Colors;
    public UIPositionTween BallGame;

    [Header("Toggles")]
    public Toggle DefaultMainMenu;
    public Toggle DefaultInstrument;
    public Toggle DefaultDance;

    [Header("AR Stuff")]
    public Toggle AROption;
    public WebCamBackground WebCam;
    public GameObject WebImage;
    public GameObject AR_UI;
    public PositionAxisTween ARCamera;
    public FOVTween ARCameraFOV;
    public List<Toggle> DisableToggles;
    public GameObject HideBeforePhoto;
    public GameObject SavedMsg;
    public GameObject ErrorMsg;
    public GameObject ARWarning;

    [Header("Buy Event")]
    public UnityEvent UnlockTheItems;
    public PhysicsRaycaster raycaster;
    public UnityEvent UnlockTheRewardedItems;
    int rewardedElement = -1;

    UIModes LastUIMode = UIModes.UIMODE_NULL;
    UIModes AdsUIMode = UIModes.UIMODE_NULL;

    #region TOGGLE_BUTTONS
    //Main menu functions
    public void MainMenu(bool value)
    {
        if (value)
        {
            AdsUIMode = UIModes.UIMODE_DEFAULT;
            // HideBanner();
            // ShowInterstitial();
            SetUIMode(UIModes.UIMODE_DEFAULT);
        }
    }

    public void Instrument(bool value)
    {
        if (value)
        {
            AdsUIMode = UIModes.UIMODE_VUVUCELA;
            // HideBanner();
            // ShowInterstitial();
            SetUIMode(UIModes.UIMODE_VUVUCELA);
            DefaultInstrument.SetIsOnWithoutNotify(true);
        }
    }

    public void Game(bool value)
    {
        if (value)
        {
            AdsUIMode = UIModes.UIMODE_GAME;
            // HideBanner();
            // ShowInterstitial();
            SetUIMode(UIModes.UIMODE_GAME);
        }
    }

    public void Clothes(bool value)
    {
        if (value)
        {
            AdsUIMode = UIModes.UIMODE_CLOTHES;
            // HideBanner();
            // ShowInterstitial();
            SetUIMode(UIModes.UIMODE_CLOTHES);
        }
    }

    public void Dance(bool value)
    {
        if (value)
        {
            AdsUIMode = UIModes.UIMODE_BAILE;
            // HideBanner();
            // ShowInterstitial();
            SetUIMode(UIModes.UIMODE_BAILE);
            DefaultDance.SetIsOnWithoutNotify(true);
        }
    }

    public void ARMode(bool value)
    {
        if (value) 
            muestraARMode();
        else
            ocultaARMode();
    }
 
    //Instrument functions
    public void Vuvucela(bool value)
    {
        if (value) SetUIMode(UIModes.UIMODE_VUVUCELA);
    }

    public void BomboKeys(bool value)
    {
        if (value) SetUIMode(UIModes.UIMODE_BOMBO);
    }

    public void Silvato(bool value)
    {
        if (value) SetUIMode(UIModes.UIMODE_SILBATO);
    }

    public void JembeKeys(bool value)
    {
        if (value) SetUIMode(UIModes.UIMODE_JEMBE);
    }

    public void BocinaKeys(bool value)
    {
        if (value) SetUIMode(UIModes.UIMODE_BOCINA);
    }

    #endregion

    private void Start()
    {
        // IAPControl.GameIsBought += GameIsBought;

        MarketScreen.SetActive(false);
        TeamMarketScreen.SetActive(false);
        InfoScreen.SetActive(false);
        ParentalControlPopup.SetActive(false);
        RewardPopup.SetActive(false);

        ARWarning.SetActive(false);

#if !UNITY_EDITOR
       
#endif
        

        SetUIMode(UIModes.UIMODE_DEFAULT);

        //HideMsg();
    }

    public void SetUIMode(UIModes uiMode)
    {
        if (uiMode == LastUIMode) return;

        //SoundBank.instance.StopAllSounds();
        SoundBank_B.instance.StopAllSounds();

        AnimationCtrl.instance.SetUIMode(uiMode);

        switch (LastUIMode)
        {
            case UIModes.UIMODE_DEFAULT:
                break;
            case UIModes.UIMODE_VUVUCELA:
                salirDeTwoKeys(uiMode);
                break;
            case UIModes.UIMODE_BOMBO:
                salirDeTwoKeys(uiMode);
                break;
            case UIModes.UIMODE_SILBATO:
                salirDeTwoKeys(uiMode);
                break;
            case UIModes.UIMODE_JEMBE:
                salirDeTwoKeys(uiMode);
                break;
            case UIModes.UIMODE_BOCINA:
                salirDeTwoKeys(uiMode);
                break;
            case UIModes.UIMODE_GAME:
                ocultaGame();
                break;
            case UIModes.UIMODE_BAILE:
                ocultaBailes();
                // Paramos la canción que esté tocando, si hay alguna.
                break;
            case UIModes.UIMODE_CLOTHES:
                ocultaClothes();
                break;
            //case UIModes.UIMODE_AR:
            //    ocultaARMode();
            //    break;
            default:
                break;
        }

        switch (uiMode)
        {
            case UIModes.UIMODE_DEFAULT:
                ocultaTodo();
                break;
            case UIModes.UIMODE_VUVUCELA:
                PianoKey.Instrument = (int)PianoKey.TwoKeyInstrument.KEY_VUVUCELA;
                if (LastUIMode < UIModes.UIMODE_VUVUCELA || LastUIMode > UIModes.UIMODE_BOCINA)
                {
                    muestraTwoKeys();
                    muestraInstrumentos();
                }
                break;
            case UIModes.UIMODE_BOMBO:
                PianoKey.Instrument = (int)PianoKey.TwoKeyInstrument.KEY_BOMBO;
                if (LastUIMode < UIModes.UIMODE_VUVUCELA || LastUIMode > UIModes.UIMODE_BOCINA)
                {
                    muestraTwoKeys();
                    muestraInstrumentos();
                }
                break;
            case UIModes.UIMODE_SILBATO:
                PianoKey.Instrument = (int)PianoKey.TwoKeyInstrument.KEY_SILBATO;
                if (LastUIMode < UIModes.UIMODE_VUVUCELA || LastUIMode > UIModes.UIMODE_BOCINA)
                {
                    muestraTwoKeys();
                    muestraInstrumentos();
                }
                break;
            case UIModes.UIMODE_JEMBE:
                PianoKey.Instrument = (int)PianoKey.TwoKeyInstrument.KEY_DJEMBE;
                if (LastUIMode < UIModes.UIMODE_VUVUCELA || LastUIMode > UIModes.UIMODE_BOCINA)
                {
                    muestraTwoKeys();                    
                    muestraInstrumentos();
                }
                break;
            case UIModes.UIMODE_BOCINA:
                PianoKey.Instrument = (int)PianoKey.TwoKeyInstrument.KEY_BOCINA;
                if (LastUIMode < UIModes.UIMODE_VUVUCELA || LastUIMode > UIModes.UIMODE_BOCINA)
                {
                    muestraTwoKeys();
                    muestraInstrumentos();
                }
                break;
            case UIModes.UIMODE_GAME:
                if (LastUIMode >= UIModes.UIMODE_VUVUCELA && LastUIMode <= UIModes.UIMODE_BOCINA)
                    AnimationCtrl.instance.uiChanged.AddListener(muestraGame);
                else
                    muestraGame();
                break;
            case UIModes.UIMODE_BAILE:
                muestraBailes();                
                break;
            case UIModes.UIMODE_CLOTHES:
                muestraClothes();
                break;
            //case UIModes.UIMODE_AR:
            //    muestraARMode();
            //    break;
            default:
                break;
        }

        LastUIMode = uiMode;
    }

#region SHOW_HIDE_UI

    private void EntrarEnTwoKeys()
    {
        if (LastUIMode < UIModes.UIMODE_VUVUCELA || LastUIMode > UIModes.UIMODE_BOCINA)
        {
            muestraTwoKeys();
            muestraInstrumentos();
        }
    }

    private void salirDeTwoKeys(UIModes uiMode)
    {
        if (uiMode < UIModes.UIMODE_VUVUCELA || uiMode > UIModes.UIMODE_BOCINA)
        {
            ocultaTwoKeys();
            ocultaInstrumentos();
        }
    }

    private void ocultaTodo()
    {
        ocultaGame();
        ocultaInstrumentos();
        ocultaTwoKeys();
        ocultaBailes();
        ocultaClothes();
    }

    private void muestraGame()
    {
        AnimationCtrl.instance.uiChanged.RemoveListener(muestraGame);
        AROption.interactable = false;
        BallGame.PlayTween();
    }

    private void ocultaGame()
    {
        AROption.interactable = true;
        BallGame.ReverseTween();
    }

    private void muestraInstrumentos()
    {
        Instrumentos.PlayTween();
    }

    private void ocultaInstrumentos()
    {
        Instrumentos.ReverseTween();
    }

    private void muestraBailes()
    {
        SoundBank_B.instance.StopAll("Celebration");
        SoundBank_B.instance.Play("Celebration", 0);
        Bailes.PlayTween();
    }

    private void ocultaBailes()
    {
        SoundBank_B.instance.StopAll("Celebration");
        Bailes.ReverseTween();
    }

    private void muestraClothes()
    {
        AROption.interactable = false;
        Customize.PlayTween();
    }

    private void ocultaClothes()
    {
        AROption.interactable = true;
        Customize.ReverseTween();
    }

    private void muestraARMode()
    {
        foreach (Toggle t in DisableToggles)
            t.interactable = false;
        DefaultMainMenu.SetIsOnWithoutNotify(true);
        SetUIMode(UIModes.UIMODE_DEFAULT);
        ARCamera.PlayTween();
        ARCameraFOV.PlayTween();
        WebImage.SetActive(true);
        AR_UI.SetActive(true);
        WebCam.PlayWebCam();

        ARWarning.SetActive(true);
    }

    private void ocultaARMode()
    {
        DefaultMainMenu.SetIsOnWithoutNotify(true);
        SetUIMode(UIModes.UIMODE_DEFAULT);
        WebCam.StopWebCam();
        ARCamera.ReverseTween();
        ARCameraFOV.ReverseTween();
        WebImage.SetActive(false);
        AR_UI.SetActive(false);
        foreach (Toggle t in DisableToggles)
            t.interactable = true;

        ARWarning.SetActive(false);
    }

    private void muestraTwoKeys()
    {
        TwoKeys.PlayTween();
    }

    private void ocultaTwoKeys()
    {
        TwoKeys.ReverseTween();
    }

    #endregion

    public void PlayTouchEvent(int typeIdx)
    {
        TouchAnimation type = (TouchAnimation)typeIdx;
        switch (type)
        {
            case TouchAnimation.TA_HEAD:
                AnimationCtrl.instance.PlayHeadTouch();
                break;
            case TouchAnimation.TA_TOE:
                AnimationCtrl.instance.PlayToeTouch();
                break;
            case TouchAnimation.TA_HAND:
                AnimationCtrl.instance.PlayHandTouch();
                break;
            case TouchAnimation.TA_TORSO:
                AnimationCtrl.instance.PlayChestTouch();
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Back();
    }

    public void GotoInfoScreen()
    {
        HideBanner();
        AnimationCtrl.instance.NotInMainScreen();
        InfoScreen.SetActive(true);
        TalkListen.instance.DisableTalking();
    }

    private int marketType = 0;
    public void ParentalControlSuccess()
    {
        HideBanner();
        AnimationCtrl.instance.NotInMainScreen();
        ParentalControlPopup.SetActive(false);
        if (marketType == 0)
            GotoMarketScreen();
        else
            GotoTeamMarketScreen();
    }

    public void GotoMarketScreen()
    {

        MarketScreen.SetActive(true);
    }

    public void GotoTeamMarketScreen()
    {

        TeamMarketScreen.SetActive(true);
    }

    public void GotoParentalControl(int mType)
    {
        HideBanner();
        AnimationCtrl.instance.NotInMainScreen();
        ParentalControlPopup.SetActive(true);
        CheckParental.show();
        TalkListen.instance.DisableTalking();
        marketType = mType;
    }

    // public void GotoRewardedVideo(int rewardType) //order in rewarded array
    // {
    //     rewardedElement = rewardType;
    //     // HideBanner();
    //     AnimationCtrl.instance.NotInMainScreen();
    //     RewardPopup.SetActive(true);
    //     TalkListen.instance.DisableTalking();
    // }

    public void Back()
    {
        if(ParentalControlPopup.activeSelf)
        {
            ParentalControlPopup.SetActive(false);
            AnimationCtrl.instance.BackToMainScreen();
            if (LastUIMode == UIModes.UIMODE_DEFAULT)
                TalkListen.instance.EnableTalking();
            // ShowBanner();
        }
        else if (RewardPopup.activeSelf)
        {
            rewardedElement = -1;
            RewardPopup.SetActive(false);
            AnimationCtrl.instance.BackToMainScreen();
            if (LastUIMode == UIModes.UIMODE_DEFAULT)
                TalkListen.instance.EnableTalking();
            // ShowBanner();
        }
        else if(MarketScreen.activeSelf)
        {
            MarketScreen.SetActive(false);
            AnimationCtrl.instance.BackToMainScreen();
            if (LastUIMode == UIModes.UIMODE_DEFAULT)
                TalkListen.instance.EnableTalking();
            // ShowBanner();
        }
        else if (TeamMarketScreen.activeSelf)
        {
            TeamMarketScreen.SetActive(false);
            AnimationCtrl.instance.BackToMainScreen();
            if (LastUIMode == UIModes.UIMODE_DEFAULT)
                TalkListen.instance.EnableTalking();
            // ShowBanner();
        }
        else if(InfoScreen.activeSelf)
        {
            // ShowInterstitial();
        }
        else
        {
            SSaveLoad.save();
            Application.Quit();
        }
    }

    void GameIsBought()
    {
        // HideBanner();
        UnlockTheItems.Invoke();
    }

#region ADS
    public void ShowBanner()
    {
#if !UNITY_EDITOR
        // if (!SGamePackageSave.GetInstance().m_IsGameBought)
        //     SAdsManager.instance.ShowBanner();
#endif
    }

    public void HideBanner()
    {
#if !UNITY_EDITOR
        // if (!SGamePackageSave.GetInstance().m_IsGameBought)
        //     SAdsManager.instance.HideBanner();
#endif
    }

    public void ShowInterstitial()
    {
#if !UNITY_EDITOR
        // if (SGamePackageSave.GetInstance().m_IsGameBought)
        // {
        //     ClosedInterstitial();
        //     return;
        // }

        // if (SAdsManager.instance.IsInterstitialLoad())
        // {
        //     SAdsManager.instance.ShowInterstitial();
        // }
//         else
//             ClosedInterstitial();
// #else
//         ClosedInterstitial();
#endif
    }

    void ClosedInterstitial()
    {
#if !UNITY_EDITOR
        // if (!SGamePackageSave.GetInstance().m_IsGameBought)
        // {
        //     // SAdsManager.instance.LoadInterstitial();
        // }
#endif
        if (InfoScreen.activeSelf)
        {
            InfoScreen.SetActive(false);
            AnimationCtrl.instance.BackToMainScreen();
            if (LastUIMode == UIModes.UIMODE_DEFAULT)
                TalkListen.instance.EnableTalking();
        }
        else
            SetUIMode(AdsUIMode);
        // ShowBanner();
    }

//     public void ShowRewarded()
//     {
// #if !UNITY_EDITOR
//         if (SGamePackageSave.GetInstance().m_IsGameBought)
//         {
//             ClosedRewarded();
//             return;
//         }

//         if (SAdsManager.instance.IsVideoRewardLoaded())
//         {
//             SAdsManager.instance.ShowRewardBasedVideo();
//         }
//         else
//             ClosedRewarded();
// #else
//         //Only testing purpouse
//         UnlockedRewarded();
//         ClosedRewarded();
// #endif
//     }

    void ClosedRewarded()
    {
        rewardedElement = -1;
        RewardPopup.SetActive(false);
        AnimationCtrl.instance.BackToMainScreen();
        if (LastUIMode == UIModes.UIMODE_DEFAULT)
            TalkListen.instance.EnableTalking();
        // ShowBanner();
    }

    void UnlockedRewarded()
    {
        SPermanetVariables.isVideoRewarded = SPermanetVariables.isVideoRewarded || (rewardedElement == 0);
        SPermanetVariables.isVideoRewarded2 = SPermanetVariables.isVideoRewarded2 || (rewardedElement == 1);
        SPermanetVariables.isVideoRewarded3 = SPermanetVariables.isVideoRewarded3 || (rewardedElement == 2);
        UnlockTheRewardedItems.Invoke();
        /*
        RewardPopup.SetActive(false);
        AnimationCtrl.instance.BackToMainScreen();
        if (LastUIMode == UIModes.UIMODE_DEFAULT)
            TalkListen.instance.EnableTalking();
        ShowBanner();
        */
    }
#endregion

    public void GotoApp1()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.zinkia.pocoyo.numbers2");
#else
        Application.OpenURL("https://apps.apple.com/us/app/pocoyo-123-space-adventure/id1504480822");
#endif
    }

    public void GotoApp2()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.zinkia.pocoyo.alphabet2");
#else
        Application.OpenURL("https://apps.apple.com/us/app/pocoyo-abc-adventure/id1410665834");
#endif
    }

    public void TakeScreenShoot()
    {
        StartCoroutine("TakePhoto");
    }

    IEnumerator TakePhoto()
    {
        HideBeforePhoto.GetComponent<Canvas>().enabled = false;

        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        // Save the screenshot to Gallery/Photos
        string fieName = DateTime.Now.ToString("yyyyMMDDHHmmss") + ".png";
        
        try
        {
            NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ss, "PocoyoFootball", fieName, SaveResult);
            Debug.Log("Permission result: " + permission);
        }
        catch (System.Exception e)
        {
            Debug.LogError("NativeGallery error: " + e.Message);
            // Fallback: just save to local storage
            SaveScreenshotLocally(ss, fieName);
        }

        // To avoid memory leaks
        Destroy(ss);

        HideBeforePhoto.GetComponent<Canvas>().enabled = true;
    }
    
    // Fallback method for saving screenshots locally
    private void SaveScreenshotLocally(Texture2D texture, string fileName)
    {
        try
        {
            byte[] bytes = texture.EncodeToPNG();
            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllBytes(path, bytes);
            Debug.Log("Screenshot saved locally to: " + path);
            SaveResult(true, path);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save screenshot locally: " + e.Message);
            SaveResult(false, "");
        }
    }

    void SaveResult(bool success, string path)
    {
        Debug.Log("Media save result: " + success + " " + path);
        if (success)
            SavedMsg.SetActive(true);
        else
            ErrorMsg.SetActive(true);

        //Invoke("HideMsg", 0.5f);
    }

    //void HideMsg()
    //{
    //    SavedMsg.SetActive(false);
    //    ErrorMsg.SetActive(false);
    //}

    public void PrivacyPolicyLink()
    {
        Application.OpenURL("https://www.animaj.com/privacy-policy");
    }

}
