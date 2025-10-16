using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundManager;

public class SoundBank : MonoBehaviour
{
    public static SoundBank instance;

    public UISound UI;
    public UISound normal;
    public UISound normalInactive;
    public UISound danceLoop;
    public UISound animals;
    public UISound animalsResult;
    public UISound instruments;
    public UISound instrumentsExit;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        // SAdsManager.instance.m_InterstitialOpen -= MuteAudio;
        // SAdsManager.instance.m_InterstitialClose -= MuteAudio;

        // SAdsManager.instance.m_VideoRewardOpen -= MuteAudio;
        // SAdsManager.instance.m_VideoRewardClose -= MuteAudio;
    }

    private void MuteAudio()
    {
        SSoundManager.GetInstance().mute_all();
    }

    public void StopAllSounds()
    {
        UI.StopAll();
        normal.StopAll();
        normalInactive.StopAll();
        danceLoop.StopAll();
        animals.StopAll();
        animalsResult.StopAll();
        instruments.StopAll();
        instrumentsExit.StopAll();
    }

    public void PlayNormal(int idx)
    {
        normal.Play(idx);
    }

    public void Inactivity(int idx)
    {
        normalInactive.Play(idx);
    }

    public void HeadTouch(int idx)
    {
        normal.Play(5 + idx);
    }

    public void ToeTouch(int idx)
    {
        normal.Play(2 + idx);
    }

    public void TorsoTouch(int idx)
    {
        normal.Play(0 + idx);
    }

    public void HandTouch()
    {
        normal.Play(7);
    }

    public void Blink()
    {
        normal.Play(8);
    }

    public void DancePlayLoop(int idx)
    {
        danceLoop.StopAll();
        danceLoop.Play(idx);
    }

    public void AnimalPlay(int idx)
    {
        animals.Play(idx);
    }

    public void AnimalSuccess()
    {
        int index = Random.Range(0, 3);
        animalsResult.Play(index);
    }

    public void AnimalFail()
    {
        int index = Random.Range(3, 6);
        animalsResult.Play(index);
    }

    public void PlayEnterInstrument(int idx)
    {
        instruments.Play(idx);
    }

    public void PlayExitInstrument(int idx)
    {
        instrumentsExit.Play(idx);
    }
}
