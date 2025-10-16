using System.Collections;
using System.Collections.Generic;
using SoundManager_Pocoyo_Football;
using UnityEngine;
using SoundManager;

public class SoundBank_B : MonoBehaviour
{
    [System.Serializable]
    public struct SoundBankItem
    {
        public string Name;
        public UISound Bank;
    };

    public static SoundBank_B instance;
    public List<SoundBankItem> BankItems;
    
    private Dictionary<string, UISound> Banks;

    private void Awake()
    {
        instance = this;

        Banks = new Dictionary<string, UISound>();
        foreach(var item in BankItems)
        {
            Banks.Add(item.Name, item.Bank);
        }
    }

    private void Start()
    {
        if (SAdsManager.instance != null)
        {
            SAdsManager.instance.m_InterstitialOpen += MuteAudio;
            SAdsManager.instance.m_InterstitialClose += MuteAudio;

            SAdsManager.instance.m_VideoRewardOpen += MuteAudio;
            SAdsManager.instance.m_VideoRewardClose += MuteAudio;
        }
    }

    private void OnDestroy()
    {
        if (SAdsManager.instance != null)
        {
            SAdsManager.instance.m_InterstitialOpen -= MuteAudio;
            SAdsManager.instance.m_InterstitialClose -= MuteAudio;

            SAdsManager.instance.m_VideoRewardOpen -= MuteAudio;
            SAdsManager.instance.m_VideoRewardClose -= MuteAudio;
        }
    }

    private void MuteAudio()
    {
        SSoundManager.GetInstance().mute_all();
    }

    public void StopAllSounds()
    {
        foreach(var pair in Banks)
        {
            pair.Value.StopAll();
        }
    }

    public void Play(string bankName, int idx)
    {
        if(Banks.ContainsKey(bankName))
        {
            Banks[bankName].Play(idx);
        }
    }

    public void Play(string bankName)
    {
        if (Banks.ContainsKey(bankName))
        {
            Banks[bankName].Play();
        }
    }

    public void PlayRange(string bankName, int startIdx, int endIdx)
    {
        if (Banks.ContainsKey(bankName))
        {
            int idx = Random.Range(startIdx, endIdx + 1); //end number
            Banks[bankName].Play(idx);
        }
    }

    public void StopAll(string bankName)
    {
        if (Banks.ContainsKey(bankName))
        {
            Banks[bankName].StopAll();
        }
    }
}
