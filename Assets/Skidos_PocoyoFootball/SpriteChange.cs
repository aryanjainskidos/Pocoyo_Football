using System;
using UnityEngine;
using UnityEngine.UI;
using SoundManager;

public class SpriteChange : MonoBehaviour
{
    [SerializeField] private Image targetImage;   
    [SerializeField] private Sprite spriteOn;     
    [SerializeField] private Sprite spriteOff;    

    private bool isOn = true;
    
    [Header("Toggle Type")]
    public bool isMusicToggle = false;  
    public bool isSfxToggle = false;

    [Header("Voice Over or BGM Audio")]
    public AudioSource audioSource; // assign your AudioSource here

    void Start()
    {
        if (isMusicToggle)
        {
            isOn = !SSoundManager.GetInstance().IsMusicMuted();
            targetImage.sprite = isOn ? spriteOn : spriteOff;

            // Control the AudioSource based on the initial mute state
            if (audioSource != null)
                audioSource.mute = !isOn;
        }
        else if (isSfxToggle)
        {
            isOn = !SSoundManager.GetInstance().IsSfxMuted();
            targetImage.sprite = isOn ? spriteOn : spriteOff;
        }
    }

    public void OnClick()
    {
        isOn = !isOn;
        targetImage.sprite = isOn ? spriteOn : spriteOff;
        
        if (isMusicToggle)
        {
            SSoundManager.GetInstance().ToggleMusicMute();

            // Turn on/off this AudioSource
            if (audioSource != null)
            {
                audioSource.mute = !isOn;
            }
        }
        else if (isSfxToggle)
        {
            SSoundManager.GetInstance().ToggleSfxMute();
        }
    }
}