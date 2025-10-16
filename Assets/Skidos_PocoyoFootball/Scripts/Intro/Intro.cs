using SoundManager;
using System.Collections;
using System.Collections.Generic;
using SoundManager_Pocoyo_Football;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zinkia;

public class Intro : MonoBehaviour
{
    public GameObject _introIAPControl;
    public UIMicrophoneIntro microIntro;
    //public TrSoundsData voices;

    void Start()
    {
        QualitySettings.vSyncCount = 0; // 1 == 60 frames / 2 == 30 frames
        Application.targetFrameRate = 25;
        Input.multiTouchEnabled = false;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        SGamePackageSave.GetInstance();
        Translation.GetInstance();

        SPermanetVariables.isVideoRewarded = false;
        SPermanetVariables.isVideoRewarded2 = false;
        SPermanetVariables.isVideoRewarded3 = false;

        //LogoAnimation.instance.AnimationEnded.AddListener(LoadUserData);
    }

    public void LoadUserData()
    {
        //LogoAnimation.instance.AnimationEnded.RemoveListener(LoadUserData);
        //Load previous data
        SSaveLoad.loadFinish += SetupSystems;
        SSaveLoad.load();
    }

    public void SetupSystems()
    {
        //if (Debug.isDebugBuild)
        //{
        //    SGamePackageSave.GetInstance().m_IsGameBought = true;
        //    SGamePackageSave.GetInstance().teamLockStatusFlags = 0b_111111111;
        //}

        Translation.GetInstance().SetSelectedLanguage(Translation.Language.LNG_NULL); //Get the device language
        Translation.GetInstance().resetLanguage(_introIAPControl);
        SSoundManager.GetInstance();


        microIntro.microphoneFinish += GotoNextScene;
        microIntro.showMicrophoneDialog();
    }

    public void GotoNextScene()
    {
        Debug.Log("GotoNextScene");
        microIntro.microphoneFinish -= GotoNextScene;
        StartCoroutine(AsynchronousLoad(1));
	}

    IEnumerator AsynchronousLoad(int _story_idx)
    {
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(_story_idx, LoadSceneMode.Single);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            if (ao.progress == 0.9f)
            {
                ao.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
